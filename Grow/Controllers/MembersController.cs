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
            return RedirectToAction("Index", "Households");
            //CookieHelper.CookieSet(HttpContext, ControllerName() + "URL", "", -1);

            //ViewData["Filtering"] = "";

            //string[] sortOptions = new[] { "Membership No.", "Member", "Age", "Annual Income" };

            //var members = from m in _context.Members
            //              .Include(m => m.MemberIncomes)
            //              .ThenInclude(m => m.IncomeSource)
            //              .Include(m => m.Gender)
            //              .Include(m => m.Household)
            //              .Include(m => m.MemberConcerns)
            //              .ThenInclude(m => m.HealthConcern)
            //              .Include(m => m.MemberRestrictions)
            //              .ThenInclude(m => m.DietaryRestriction)
            //              select m;

            //if (!String.IsNullOrEmpty(SearchString))
            //{
            //    members = members.Where(m => m.LastName.ToUpper().Contains(SearchString.ToUpper())
            //                           || m.FirstName.ToUpper().Contains(SearchString.ToUpper()));
            //    ViewData["Filtering"] = " show";
            //}

            //if (!String.IsNullOrEmpty(actionButton))
            //{
            //    page = 1;

            //    if (actionButton != "Filter")
            //    {
            //        if (actionButton == sortField)
            //        {
            //            sortDirection = sortDirection == "asc" ? "desc" : "asc";
            //        }
            //        sortField = actionButton;
            //    }
            //}

            //if (sortField == "Membership No.")
            //{
            //    if (sortDirection == "asc")
            //    {
            //        members = members
            //            .OrderBy(m => m.HouseholdID);
            //    }
            //    else
            //    {
            //        members = members
            //            .OrderByDescending(m => m.HouseholdID);
            //    }
            //}
            //else if (sortField == "Age")
            //{
            //    if (sortDirection == "asc")
            //    {
            //        members = members
            //            .OrderByDescending(m => m.DOB);
            //    }
            //    else
            //    {
            //        members = members
            //            .OrderBy(m => m.DOB);
            //    }
            //}
            //else
            //{
            //    if (sortDirection == "asc")
            //    {
            //        members = members
            //            .OrderBy(m => m.LastName)
            //            .ThenBy(m => m.FirstName);
            //    }
            //    else
            //    {
            //        members = members
            //            .OrderByDescending(m => m.LastName)
            //            .ThenByDescending(m => m.FirstName);
            //    }
            //}
            //ViewData["sortField"] = sortField;
            //ViewData["sortDirection"] = sortDirection;

            //int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID);
            //ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            //var pagedData = await PaginatedList<Member>.CreateAsync(members.AsNoTracking(), page ?? 1, pageSize);
            //return View(pagedData);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
        public IActionResult Create(int? id)
        {
            // Save household ID for form
            ViewData["HouseholdID"] = id.GetValueOrDefault();

            PopulateDropDownLists();

            // List Box Stuff
            PopulateAssignedRestrictionData(new Member());
            PopulateAssignedHealthConcernsData(new Member());

            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,DOB,Phone,Email,IncomeVerified,IncomeAmount,DataConsent,Notes,HouseholdID,GenderID")] Member member,
            string[] selectedOptions, List<IFormFile> theFiles, string[] selectedHealthOptions, string[] selectedRestrictionOptions, string IncomeSource1,
            double IncomeAmount1, string IncomeSource2, double IncomeAmount2, string IncomeSource3, double IncomeAmount3)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(member);
                    _context.SaveChanges();

                    // Add Member Income Sources
                    Dictionary<int, double> sources = new Dictionary<int, double>();
                    if (IncomeSource1 != null)
                        sources.Add(Convert.ToInt32(IncomeSource1), IncomeAmount1);
                    if (IncomeSource2 != null)
                        sources.Add(Convert.ToInt32(IncomeSource2), IncomeAmount2);
                    if (IncomeSource3 != null)
                        sources.Add(Convert.ToInt32(IncomeSource3), IncomeAmount3);

                    UpdateMemberIncomes(sources, member);

                    // Add Member Restrictions
                    UpdateMemberRestriction(selectedRestrictionOptions, member);

                    // Add Member Concerns
                    UpdateMemberConcerns(selectedHealthOptions, member);

                    // Add Member Documents
                    AddDocumentsAsync(member, theFiles);

                    // Update Calculated Member & Household fields
                    UpdateCalculatedFields(member);

                    // Add entry to Membership changes table
                    _context.Add(new MembershipChange
                    {
                        HouseholdID = member.HouseholdID,
                        ChangeType = "Add Member",
                        ChangeDescription = $"Added member {member.FullName}.",
                        ChangeDate = DateTime.Today
                    });
                    _context.SaveChanges();

                    return RedirectToAction("Edit", "Households", new { id = member.HouseholdID });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            PopulateAssignedHealthConcernsData(member);
            PopulateAssignedRestrictionData(member);
            PopulateDropDownLists(member);
            ViewData["HouseholdID"] = member.HouseholdID;

            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Get the Member to edit
            var member = _context.Members
                .Include(x => x.Gender)
                .Include(x => x.MemberIncomes)
                    .ThenInclude(x => x.IncomeSource)
                .Include(x => x.MemberRestrictions)
                    .ThenInclude(x => x.DietaryRestriction)
                .Include(x => x.MemberConcerns)
                    .ThenInclude(x => x.HealthConcern)
                .Include(x => x.MemberDocuments)
                .FirstOrDefault(x => x.ID == id);

            PopulateDropDownLists(member);

            // List Box Stuff
            PopulateAssignedHealthConcernsData(member);
            PopulateAssignedRestrictionData(member);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,DOB,Phone,Email,IncomeVerified,IncomeAmount,DataConsent,Notes,HouseholdID,GenderID")] Member member,
            string[] selectedOptions, List<IFormFile> theFiles, string IncomeSource1, double IncomeAmount1, string IncomeSource2, double IncomeAmount2,
            string IncomeSource3, double IncomeAmount3, string[] selectedHealthOptions, string[] selectedRestrictionOptions)
        {
            var memberToUpdate = _context.Members
                .Include(x => x.Gender)
                .Include(x => x.MemberIncomes)
                    .ThenInclude(x => x.IncomeSource)
                .Include(x => x.MemberRestrictions)
                    .ThenInclude(x => x.DietaryRestriction)
                .Include(x => x.MemberConcerns)
                    .ThenInclude(x => x.HealthConcern)
                .Include(x => x.MemberDocuments)
                .FirstOrDefault(x => x.ID == id);

            if (memberToUpdate == null)
            {
                return NotFound();
            }

            // Update Member Fields
            memberToUpdate.FirstName = member.FirstName;
            memberToUpdate.LastName = member.LastName;
            memberToUpdate.DOB = member.DOB;
            memberToUpdate.GenderID = member.GenderID;
            memberToUpdate.Phone = member.Phone;
            memberToUpdate.Email = member.Email;
            memberToUpdate.Notes = member.Notes;
            memberToUpdate.DataConsent = member.DataConsent;
            memberToUpdate.IncomeVerified = member.IncomeVerified;

            if (ModelState.IsValid)
            {
                try
                {
                    // Update Member
                    _context.Update(memberToUpdate);
                    await _context.SaveChangesAsync();

                    // Update Member Income Sources
                    Dictionary<int, double> sources = new Dictionary<int, double>();
                    if (IncomeSource1 != null)
                        sources.Add(Convert.ToInt32(IncomeSource1), IncomeAmount1);
                    if (IncomeSource2 != null)
                        sources.Add(Convert.ToInt32(IncomeSource2), IncomeAmount2);
                    if (IncomeSource3 != null)
                        sources.Add(Convert.ToInt32(IncomeSource3), IncomeAmount3);

                    UpdateMemberIncomes(sources, memberToUpdate);

                    // Update Member Restrictions
                    UpdateMemberRestriction(selectedRestrictionOptions, memberToUpdate);

                    // Update Member Concerns
                    UpdateMemberConcerns(selectedHealthOptions, memberToUpdate);

                    // Add Member Documents
                    AddDocumentsAsync(member, theFiles);

                    // Update Calculated Member & Household fields
                    UpdateCalculatedFields(memberToUpdate);

                    // Add entry to Membership changes table
                    _context.Add(new MembershipChange
                    {
                        HouseholdID = member.HouseholdID,
                        ChangeType = "Edit Member",
                        ChangeDescription = $"Edited member {member.FullName}.",
                        ChangeDate = DateTime.Today
                    });
                    _context.SaveChanges();

                    return RedirectToAction("Edit", "Households", new { id = member.HouseholdID });
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!MemberExists(memberToUpdate.ID))
                    {
                        ModelState.AddModelError("", "Unable to update. Member not found.");
                    }
                    else
                    {
                        ModelState.AddModelError("", e.Message);
                    }
                }
            }

            PopulateAssignedHealthConcernsData(memberToUpdate);
            PopulateAssignedRestrictionData(memberToUpdate);
            PopulateDropDownLists(memberToUpdate);
            return View(memberToUpdate);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Get the Member to delete
            Member member = _context.Members
                .Include(x => x.Gender)
                .Include(x => x.MemberIncomes)
                    .ThenInclude(x => x.IncomeSource)
                .Include(x => x.MemberRestrictions)
                    .ThenInclude(x => x.DietaryRestriction)
                .Include(x => x.MemberConcerns)
                    .ThenInclude(x => x.HealthConcern)
                .FirstOrDefault(x => x.ID == id);

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = _context.Members
                .Include(x => x.Gender)
                .Include(x => x.MemberIncomes)
                    .ThenInclude(x => x.IncomeSource)
                .Include(x => x.MemberRestrictions)
                    .ThenInclude(x => x.DietaryRestriction)
                .Include(x => x.MemberConcerns)
                    .ThenInclude(x => x.HealthConcern)
                .FirstOrDefault(x => x.ID == id);

            if (member != null)
            {
                var deletedMember = member;

                _context.Members.Remove(member);
                await _context.SaveChangesAsync();

                // Add entry to Membership changes table
                _context.Add(new MembershipChange
                {
                    HouseholdID = deletedMember.HouseholdID,
                    ChangeType = "Delete Member",
                    ChangeDescription = $"Deleted member {deletedMember.FullName}.",
                    ChangeDate = DateTime.Today
                });
                _context.SaveChanges();

                return RedirectToAction("Edit", "Households", new { id = member.HouseholdID });
            }

            return View(member);
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
                        m.MemberID = member.ID;

                        _context.Add(m);
                        _context.SaveChanges();
                    };
                }
            }
        }

        private void PopulateDropDownLists(Member member = null)
        {
            var isQuery = from s in _context.IncomeSources
                          orderby s.Source
                          select s;

            var gQuery = from g in _context.Genders
                         select g;

            var hcQuery = from c in _context.HealthConcerns
                          orderby c.Concern
                          select c;

            var drQuery = from r in _context.DietaryRestrictions
                          orderby r.Restriction
                          select r;

            ViewData["IncomeSources"] = new SelectList(isQuery, "ID", "Source");

            if (member?.MemberIncomes.Count() >= 1)
                ViewData["IncomeSources1"] = new SelectList(isQuery, "ID", "Source", member?.MemberIncomes.ElementAt(0).IncomeSourceID);
            else
                ViewData["IncomeSources1"] = new SelectList(isQuery, "ID", "Source");

            if (member?.MemberIncomes.Count() >= 2)
                ViewData["IncomeSources2"] = new SelectList(isQuery, "ID", "Source", member?.MemberIncomes.ElementAt(1).IncomeSourceID);
            else
                ViewData["IncomeSources2"] = new SelectList(isQuery, "ID", "Source");

            if (member?.MemberIncomes.Count() == 3)
                ViewData["IncomeSources3"] = new SelectList(isQuery, "ID", "Source", member?.MemberIncomes.ElementAt(2).IncomeSourceID);
            else
                ViewData["IncomeSources3"] = new SelectList(isQuery, "ID", "Source");

            ViewData["Genders"] = new SelectList(gQuery, "ID", "GenderType", member?.GenderID);
            ViewData["HealthConcerns"] = new SelectList(hcQuery, "ID", "Concern");
            ViewData["DietaryRestrictions"] = new SelectList(drQuery, "ID", "Restriction");
        }

        private async void UpdateCalculatedFields(Member member)
        {
            // Update Member Income Amount
            var memberToUpdate = _context.Members
                .Include(x => x.MemberIncomes)
                .FirstOrDefault(x => x.ID == member.ID);

            if (memberToUpdate != null)
            {
                double incomeTotal = 0;
                foreach (var i in memberToUpdate.MemberIncomes)
                {
                    incomeTotal += i.IncomeAmount;
                }

                memberToUpdate.IncomeAmount = incomeTotal;
                _context.SaveChanges();
            }

            // Update Household Fields
            var household = _context.Households.FirstOrDefault(x => x.ID == member.HouseholdID);

            List<Member> members = _context.Members.Where(x => x.HouseholdID == household.ID).ToList();

            // Reset calculated fields
            household.Active = false;
            household.NumOfMembers = 0;
            household.LICOVerified = false;
            household.LICOVerifiedDate = null;
            household.IncomeTotal = 0.00;

            if (members != null)
            {
                // Get Total Income and Number of Members
                foreach (var m in members)
                {
                    household.NumOfMembers += 1;
                    household.IncomeTotal += m.IncomeAmount;
                }

                // Now check for LICO Verification
                if (household.NumOfMembers > 0)
                {
                    var cutOff = _context.LowIncomeCutOffs.Where(x => x.NumberOfMembers == household.NumOfMembers).FirstOrDefault();

                    if (household.IncomeTotal <= cutOff.YearlyIncome)
                    {
                        household.LICOVerified = true;
                        household.LICOVerifiedDate = DateTime.Today;

                        // If LICO is verified, set household to Active
                        household.Active = true;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<FileContentResult> Download(int id)
        {
            var theFile = await _context.MemberDocuments
                .Where(f => f.ID == id)
                .FirstOrDefaultAsync();
            return File(theFile.Content, theFile.MimeType, theFile.FileName);
        }

        public async void DeleteFile(int id)
        {
            var theFile = await _context.MemberDocuments
                .Where(f => f.ID == id)
                .FirstOrDefaultAsync();

            if(theFile != null)
            {
                _context.MemberDocuments.Remove(theFile);
                _context.SaveChanges();
            }
        }

        private string ControllerName()
        {
            return this.ControllerContext.RouteData.Values["controller"].ToString();
        }

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

        private void UpdateMemberIncomes(Dictionary<int, double> incomeSources, Member memberToUpdate)
        {
            if (memberToUpdate.ID == null)
            {
                return;
            }

            List<MemberIncome> incomesToAdd = new List<MemberIncome>();

            foreach (var i in incomeSources)
            {
                if (i.Key > -1 && i.Value > 0)
                {
                    incomesToAdd.Add(new MemberIncome { MemberID = memberToUpdate.ID, IncomeSourceID = i.Key, IncomeAmount = i.Value });
                }
            }

            // Remove all existing income sources
            List<MemberIncome> sources = _context.MemberIncomes.Where(x => x.MemberID == memberToUpdate.ID).ToList();

            if (sources?.Count() > 0)
            {
                _context.RemoveRange(sources);
            }

            // Add new sources
            if (incomesToAdd.Count() > 0)
            {
                _context.AddRange(incomesToAdd);
            }

            _context.SaveChanges();
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.ID == id);
        }
    }
}
