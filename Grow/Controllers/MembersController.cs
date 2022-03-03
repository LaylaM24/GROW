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
                          .Include(m => m.MemberConcerns)
                          .ThenInclude(m => m.HealthConcern)
                          .Include(m => m.MemberRestrictions)
                          .ThenInclude(m => m.DietaryRestriction)
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
                .Include(m => m.MemberConcerns)
                .ThenInclude(m => m.HealthConcern)
                .Include(m => m.MemberRestrictions)
                .ThenInclude(m => m.DietaryRestriction)
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

            var memberHealth = new Member();
            PopulateAssignedHealthConcernsData(memberHealth);

            PopulateDropDownLists();
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,DOB,Phone,Email,IncomeVerified,IncomeAmount,DataConsent,HouseholdID,GenderID")] Member member,
            string[] selectedOptions, List<IFormFile> theFiles, string IncomeSource1, double IncomeAmount1, string IncomeSource2, double IncomeAmount2, 
            string IncomeSource3, double IncomeAmount3, string[] selectedHealthOptions, string[] selectedRestrictionOptions)
        {
            ViewDataReturnURL();

            try
            {
                UpdateMemberRestriction(selectedRestrictionOptions, member);
                UpdateMemberConcerns(selectedHealthOptions, member);
                if (ModelState.IsValid)
                {
                    _context.Add(member);
                    await AddDocumentsAsync(member, theFiles);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { member.ID });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }

            PopulateAssignedHealthConcernsData(member);
            PopulateAssignedRestrictionData(member);
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
                .Include(m => m.MemberConcerns)
                    .ThenInclude(m => m.HealthConcern)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (member == null)
            {
                return NotFound();
            }
            PopulateAssignedHealthConcernsData(member);
            PopulateAssignedRestrictionData(member);
            PopulateDropDownLists(member);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,DOB,Phone,Email,IncomeVerified,IncomeAmount,DataConsent,HouseholdID,GenderID")] Member member,
            string[] selectedOptions, List<IFormFile> theFiles, string IncomeSource1, double IncomeAmount1, string IncomeSource2, double IncomeAmount2,
            string IncomeSource3, double IncomeAmount3, string[] selectedHealthOptions, string[] selectedRestrictionOptions)
        {
            ViewDataReturnURL();

            var memberToUpdate = await _context.Members
                .Include(m => m.MemberIncomes)
                    .ThenInclude(m => m.IncomeSource)
                .Include(m => m.MemberDocuments)
                .Include(m => m.MemberConcerns)
                .ThenInclude(m => m.HealthConcern)
                .Include(m => m.MemberRestrictions)
                .ThenInclude(m => m.DietaryRestriction)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (memberToUpdate == null)
            {
                return NotFound();
            }

            UpdateMemberConcerns(selectedHealthOptions, memberToUpdate);
            UpdateMemberRestriction(selectedRestrictionOptions, memberToUpdate);
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
            PopulateAssignedHealthConcernsData(memberToUpdate);
            PopulateAssignedRestrictionData(memberToUpdate);
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
                .Include(m => m.MemberConcerns)
                .ThenInclude(m => m.HealthConcern)
                .Include(m => m.MemberRestrictions)
                .ThenInclude(m => m.DietaryRestriction)
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
                .Include(m => m.MemberConcerns)
                .ThenInclude(m => m.HealthConcern)
                .Include(m => m.MemberRestrictions)
                .ThenInclude(m => m.DietaryRestriction)
                .FirstOrDefaultAsync(m => m.ID == id);

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return Redirect(ViewData["returnURL"].ToString());
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
        private void PopulateDropDownLists(Member member = null)
        {
            MemberIncome memberStatus = null;
            if (member != null)
            {
                memberStatus = _context.MemberIncomes
                .FirstOrDefault(m => m.MemberID == member.ID);
            }

            var isQuery = from fs in _context.IncomeSources
                          orderby fs.Source
                          select fs;

            var gQuery = from g in _context.Genders
                         select g;

            var mQuery = from m in _context.Households
                         orderby m.StreetName
                         select m;

            ViewData["IncomeSourceID"] = new SelectList(isQuery, "ID", "Source", memberStatus?.IncomeSourceID);
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

        // List boxes
        private void PopulateAssignedHealthConcernsData(Member member)
        {
            var allHealthOptions = _context.HealthConcerns;
            var currentHealthOptions = new HashSet<int>(member.MemberConcerns.Select(b => b.HealthConcernID));
            //Instead of one list with a boolean, we will make two lists
            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();
            foreach (var s in allHealthOptions)
            {
                if (currentHealthOptions.Contains(s.ID))
                {
                    selected.Add(new ListOptionVM
                    {
                        ID = s.ID,
                        DisplayText = s.Concern
                    });
                }
                else
                {
                    available.Add(new ListOptionVM
                    {
                        ID = s.ID,
                        DisplayText = s.Concern
                    });
                }
            }

            ViewData["selOpts"] = new MultiSelectList(selected.OrderBy(s => s.DisplayText), "ID", "DisplayText");
            ViewData["availOpts"] = new MultiSelectList(available.OrderBy(s => s.DisplayText), "ID", "DisplayText");
        }
        private void UpdateMemberConcerns(string[] selectedHealthOptions, Member memberToUpdate)
        {
            if (selectedHealthOptions == null)
            {
                memberToUpdate.MemberConcerns = new List<MemberConcern>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedHealthOptions);
            var currentOptionsHS = new HashSet<int>(memberToUpdate.MemberConcerns.Select(b => b.HealthConcernID));
            foreach (var s in _context.HealthConcerns)
            {
                if (selectedOptionsHS.Contains(s.ID.ToString()))//it is selected
                {
                    if (!currentOptionsHS.Contains(s.ID))//but not currently in the Member's collection - Add it!
                    {
                        memberToUpdate.MemberConcerns.Add(new MemberConcern
                        {
                            HealthConcernID = s.ID,
                            MemberID = memberToUpdate.ID
                        });
                    }
                }
                else //not selected
                {
                    if (currentOptionsHS.Contains(s.ID))
                    {
                        MemberConcern specToRemove = memberToUpdate.MemberConcerns.FirstOrDefault(d => d.HealthConcernID == s.ID);
                        _context.Remove(specToRemove);
                    }
                }
            }
        }
        private void PopulateAssignedRestrictionData(Member member)
        {
            var allRestrictionOptions = _context.DietaryRestrictions;
            var currentRestrictionOptions = new HashSet<int>(member.MemberRestrictions.Select(b => b.DietaryRestrictionID));
            //Instead of one list with a boolean, we will make two lists
            var selectedR = new List<ListOptionVM>();
            var availableR = new List<ListOptionVM>();
            foreach (var s in allRestrictionOptions)
            {
                if (currentRestrictionOptions.Contains(s.ID))
                {
                    selectedR.Add(new ListOptionVM
                    {
                        ID = s.ID,
                        DisplayText = s.Restriction
                    });
                }
                else
                {
                    availableR.Add(new ListOptionVM
                    {
                        ID = s.ID,
                        DisplayText = s.Restriction
                    });
                }
            }

            ViewData["selOptsR"] = new MultiSelectList(selectedR.OrderBy(s => s.DisplayText), "ID", "DisplayText");
            ViewData["availOptsR"] = new MultiSelectList(availableR.OrderBy(s => s.DisplayText), "ID", "DisplayText");
        }
        private void UpdateMemberRestriction(string[] selectedRestrictionOptions, Member memberToUpdate)
        {
            if (selectedRestrictionOptions == null)
            {
                memberToUpdate.MemberRestrictions = new List<MemberRestriction>();
                return;
            }

            var selectedOptionsR = new HashSet<string>(selectedRestrictionOptions);
            var currentOptionsR = new HashSet<int>(memberToUpdate.MemberRestrictions.Select(b => b.DietaryRestrictionID));
            foreach (var s in _context.DietaryRestrictions)
            {
                if (selectedOptionsR.Contains(s.ID.ToString()))//it is selected
                {
                    if (!currentOptionsR.Contains(s.ID))//but not currently in the Member's collection - Add it!
                    {
                        memberToUpdate.MemberRestrictions.Add(new MemberRestriction
                        {
                            DietaryRestrictionID = s.ID,
                            MemberID = memberToUpdate.ID
                        });
                    }
                }
                else //not selected
                {
                    if (currentOptionsR.Contains(s.ID))
                    {
                        MemberRestriction specToRemove = memberToUpdate.MemberRestrictions.FirstOrDefault(d => d.DietaryRestrictionID == s.ID);
                        _context.Remove(specToRemove);
                    }
                }
            }
        }
        //
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
