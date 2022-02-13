using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grow.Data;
using Grow.Models;
using Grow.Utilities;
using Grow.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Grow.Controllers
{
    public class MembersController : Controller
    {
        private readonly GrowContext _context;

        public MembersController(GrowContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index(string SearchString, int? page, int? pageSizeID,
            string actionButton, string sortDirection = "dec", string sortField = "Membership No.")
        {
            CookieHelper.CookieSet(HttpContext, ControllerName() + "URL", "", -1);

            ViewData["Filtering"] = "";

            string[] sortOptions = new[] { "Membership No.", "Member", "Age", "Annual Income" };

            var members = from m in _context.Members
                          .Include(m => m.MemberIncomes)
                          .ThenInclude(m => m.IncomeSource)
                          .Include(m => m.Gender)
                          .Include(m => m.Household)
                          select m;

            if (!String.IsNullOrEmpty(SearchString))
            {
                members = members.Where(m => m.LastName.ToUpper().Contains(SearchString.ToUpper())
                                       || m.FirstName.ToUpper().Contains(SearchString.ToUpper()));
                ViewData["Filtering"] = " show";
            }

            if (!String.IsNullOrEmpty(actionButton))
            {
                page = 1;

                if (actionButton != "Filter")
                {
                    if (actionButton == sortField)
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton;
                }
            }

            if (sortField == "Membership No.")
            {
                if (sortDirection == "asc")
                {
                    members = members
                        .OrderBy(m => m.HouseholdID);
                }
                else
                {
                    members = members
                        .OrderByDescending(m => m.HouseholdID);
                }
            }
            else if (sortField == "Age")
            {
                if (sortDirection == "asc")
                {
                    members = members
                        .OrderByDescending(m => m.DOB);
                }
                else
                {
                    members = members
                        .OrderBy(m => m.DOB);
                }
            }
            else
            {
                if (sortDirection == "asc")
                {
                    members = members
                        .OrderBy(m => m.LastName)
                        .ThenBy(m => m.FirstName);
                }
                else
                {
                    members = members
                        .OrderByDescending(m => m.LastName)
                        .ThenByDescending(m => m.FirstName);
                }
            }
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID);
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Member>.CreateAsync(members.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewDataReturnURL();

            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.MemberIncomes)
                .ThenInclude(m => m.IncomeSource)
                .Include(m => m.MemberDocuments)
                .Include(m => m.Gender)
                .Include(m => m.Household)
                .Include(m => m.MemberRestrictions)
                    .ThenInclude(mr => mr.DietaryRestriction)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            ViewDataReturnURL();

            var member = new Member();
            PopulateAssignedRestrictionData(member);
            PopulateAssignedStatusData(member);

            PopulateDropDownLists();
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,DOB,Phone,Email,IncomeVerified,IncomeAmount,DataConsent,HouseholdID,GenderID")] Member member,
            string[] selectedOptions, List<IFormFile> theFiles/*, int FinancialStatus, IFormFile file*/)
        {
            ViewDataReturnURL();

            try
            {
                if (selectedOptions != null)
                {
                    foreach (var restriction in selectedOptions)
                    {
                        var restrictionToAdd = new MemberRestriction { MemberID = member.ID, DietaryRestrictionID = int.Parse(restriction) };
                        member.MemberRestrictions.Add(restrictionToAdd);
                    }

                    foreach (var status in selectedOptions)
                    {
                        var statusToAdd = new MemberIncome { MemberID = member.ID, IncomeSourceID = int.Parse(status) };
                        member.MemberIncomes.Add(statusToAdd);
                    }
                }
                if (ModelState.IsValid)
                {
                    await AddDocumentsAsync(member, theFiles);
                    _context.Add(member);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { member.ID });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }

            PopulateAssignedRestrictionData(member);
            PopulateAssignedStatusData(member);
            PopulateDropDownLists(member);
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewDataReturnURL();

            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.MemberIncomes)
                    .ThenInclude(m => m.IncomeSource)
                .Include(m => m.MemberDocuments)
                .Include(m => m.MemberRestrictions)
                    .ThenInclude(m => m.DietaryRestriction)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (member == null)
            {
                return NotFound();
            }

            PopulateAssignedRestrictionData(member);
            PopulateAssignedStatusData(member);
            PopulateDropDownLists(member);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,DOB,Phone,Email,IncomeVerified,IncomeAmount,DataConsent,HouseholdID,GenderID")] Member member,
            string[] selectedOptions, List<IFormFile> theFiles/*, int FinancialStatus, IFormFile file*/)
        {
            ViewDataReturnURL();

            var memberToUpdate = await _context.Members
                .Include(m => m.MemberIncomes)
                    .ThenInclude(m => m.IncomeSource)
                .Include(m => m.MemberDocuments)
                .Include(m => m.MemberRestrictions)
                    .ThenInclude(m => m.DietaryRestriction)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (memberToUpdate == null)
            {
                return NotFound();
            }

            UpdateMemberRestriction(selectedOptions, memberToUpdate);
            UpdateMemberStatus(selectedOptions, memberToUpdate);

            if (ModelState.IsValid)
            {
                try
                {
                    await AddDocumentsAsync(memberToUpdate, theFiles);
                    _context.Update(memberToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(memberToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { memberToUpdate.ID });
            }

            PopulateAssignedRestrictionData(memberToUpdate);
            PopulateAssignedStatusData(memberToUpdate);
            PopulateDropDownLists(memberToUpdate);
            return View(memberToUpdate);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewDataReturnURL();

            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.MemberIncomes)
                .ThenInclude(m => m.IncomeSource)
                .Include(m => m.Gender)
                .Include(m => m.Household)
                .Include(m => m.MemberRestrictions)
                    .ThenInclude(mr => mr.DietaryRestriction)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewDataReturnURL();

            var member = await _context.Members
                .Include(m => m.MemberIncomes)
                .ThenInclude(m => m.IncomeSource)
                .Include(m => m.Gender)
                .Include(m => m.Household)
                .Include(m => m.MemberRestrictions)
                    .ThenInclude(mr => mr.DietaryRestriction)
                .FirstOrDefaultAsync(m => m.ID == id);

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return Redirect(ViewData["returnURL"].ToString());
        }

        private void PopulateAssignedRestrictionData(Member member)
        {
            var allOptions = _context.DietaryRestrictions;
            var currentOptionIDs = new HashSet<int>(member.MemberRestrictions.Select(mr => mr.DietaryRestrictionID));
            var checkBoxes = new List<CheckOptionVM>();

            foreach (var option in allOptions)
            {
                checkBoxes.Add(new CheckOptionVM
                {
                    ID = option.ID,
                    DisplayText = option.Restriction,
                    Assigned = currentOptionIDs.Contains(option.ID)
                });
            }
            ViewData["RestrictionOptions"] = checkBoxes;
        }

        private void PopulateAssignedStatusData(Member member)
        {
            var allOptions = _context.IncomeSources;
            var currentOptionIDs = new HashSet<int>(member.MemberIncomes.Select(m => m.IncomeSourceID));
            var checkBoxes = new List<CheckOptionVM>();

            foreach (var option in allOptions)
            {
                checkBoxes.Add(new CheckOptionVM
                {
                    ID = option.ID,
                    DisplayText = option.Source,
                    Assigned = currentOptionIDs.Contains(option.ID)
                });
            }
            ViewData["StatusOptions"] = checkBoxes;
        }

        private void UpdateMemberStatus(string[] selectedOptions, Member memberToUpdate)
        {
            if (selectedOptions == null)
            {
                memberToUpdate.MemberIncomes = new List<MemberIncome>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var memberOptionsHS = new HashSet<int>
                (memberToUpdate.MemberIncomes.Select(m => m.IncomeSourceID));

            foreach (var option in _context.IncomeSources)
            {
                if (selectedOptionsHS.Contains(option.ID.ToString()))
                {
                    if (!memberOptionsHS.Contains(option.ID))
                    {
                        memberToUpdate.MemberIncomes.Add(new MemberIncome { MemberID = memberToUpdate.ID, IncomeSourceID = option.ID });
                    }
                }
                else
                {
                    if (memberOptionsHS.Contains(option.ID))
                    {
                        MemberIncome statusToRemove = memberToUpdate.MemberIncomes.SingleOrDefault(m => m.IncomeSourceID == option.ID);
                        _context.Remove(statusToRemove);
                    }
                }
            }
        }

        private void UpdateMemberRestriction(string[] selectedOptions, Member memberToUpdate)
        {
            if (selectedOptions == null)
            {
                memberToUpdate.MemberRestrictions = new List<MemberRestriction>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var memberOptionsHS = new HashSet<int>
                (memberToUpdate.MemberRestrictions.Select(mr => mr.DietaryRestrictionID));

            foreach (var option in _context.DietaryRestrictions)
            {
                if (selectedOptionsHS.Contains(option.ID.ToString()))
                {
                    if (!memberOptionsHS.Contains(option.ID))
                    {
                        memberToUpdate.MemberRestrictions.Add(new MemberRestriction { MemberID = memberToUpdate.ID, DietaryRestrictionID = option.ID });
                    }
                }
                else
                {
                    if (memberOptionsHS.Contains(option.ID))
                    {
                        MemberRestriction restrictionToRemove = memberToUpdate.MemberRestrictions.SingleOrDefault(mr => mr.DietaryRestrictionID == option.ID);
                        _context.Remove(restrictionToRemove);
                    }
                }
            }
        }

        private void PopulateDropDownLists(Member member = null)
        {
            MemberIncome memberStatus = null;
            if (member != null)
            {
                memberStatus = _context.MemberIncomes
                .FirstOrDefault(m => m.MemberID == member.ID);
            }

            var fsQuery = from fs in _context.IncomeSources
                          orderby fs.Source
                          select fs;

            var gQuery = from g in _context.Genders
                         select g;

            var mQuery = from m in _context.Households
                         orderby m.StreetName
                         select m;

            ViewData["FinancialSituationID"] = new SelectList(fsQuery, "ID", "Status", memberStatus?.IncomeSourceID);
            ViewData["GenderID"] = new SelectList(gQuery, "ID", "GenderType", member?.GenderID);
            ViewData["MembershipID"] = new SelectList(mQuery, "ID", "StreetName", member?.HouseholdID);
        }

        private async Task AddDocumentsAsync(Member member, List<IFormFile> theFiles)
        {
            foreach (var f in theFiles)
            {
                if (f != null)
                {
                    string mimeType = f.ContentType;
                    string fileName = Path.GetFileName(f.FileName);
                    long fileLength = f.Length;

                    if (!(fileName == "" || fileLength == 0))
                    {
                        MemberDocument m = new MemberDocument();
                        using (var memoryStream = new MemoryStream())
                        {
                            await f.CopyToAsync(memoryStream);
                            m.Content = memoryStream.ToArray();
                        }
                        m.MimeType = mimeType;
                        m.FileName = fileName;
                        member.MemberDocuments.Add(m);
                    };
                }
            }
        }
        public async Task<FileContentResult> Download(int id)
        {
            var theFile = await _context.MemberDocuments
                .Where(f => f.ID == id)
                .FirstOrDefaultAsync();
            return File(theFile.Content, theFile.MimeType, theFile.FileName);
        }
        private string ControllerName()
        {
            return this.ControllerContext.RouteData.Values["controller"].ToString();
        }

        private void ViewDataReturnURL()
        {
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, ControllerName());
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.ID == id);
        }
    }
}
