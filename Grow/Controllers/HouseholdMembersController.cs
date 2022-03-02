using Grow.Data;
using Grow.Models;
using Grow.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Controllers
{
    public class HouseholdMembersController : Controller
    {
        private readonly GrowContext _context;

        public HouseholdMembersController(GrowContext context)
        {
            _context = context;
        }

        // GET: Members
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Households");
        }

        public PartialViewResult CreateMember(int? ID)
        {
            // Save household ID for form
            ViewData["HouseholdID"] = ID.GetValueOrDefault();

            PopulateDropDownLists();

            return PartialView("_CreateMember");
        }

        public PartialViewResult EditMember(int ID)
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
                .FirstOrDefault(x => x.ID == ID);

            PopulateDropDownLists();

            return PartialView("_EditMember", member);
        }

        public PartialViewResult DeleteMember(int Id)
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
                .FirstOrDefault(x => x.ID == Id);

            return PartialView("_DeleteMember", member);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,DOB,Phone,Email,IncomeVerified,IncomeAmount,DataConsent,HouseholdID,GenderID")] Member member,
            string[] selectedOptions, List<IFormFile> theFiles, string IncomeSource1, double IncomeAmount1, string IncomeSource2, double IncomeAmount2,
            string IncomeSource3, double IncomeAmount3)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(member);
                    _context.SaveChanges();

                    // Add Member Documents
                    if(theFiles.Count() > 0)
                    {
                        await AddDocumentsAsync(member, theFiles);
                    }

                    // Add Member Income Sources

                    // Add Member Restrictions

                    // Add Member Concerns

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

                    return Json(new { success = true });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch(Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            PopulateDropDownLists(member);
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,DOB,Phone,Email,IncomeVerified,IncomeAmount,DataConsent,HouseholdID,GenderID")] Member member,
            string[] selectedOptions, List<IFormFile> theFiles, string IncomeSource1, double IncomeAmount1, string IncomeSource2, double IncomeAmount2,
            string IncomeSource3, double IncomeAmount3)
        {
            var memberToUpdate = _context.Members
                .Include(x => x.Gender)
                .Include(x => x.MemberIncomes)
                    .ThenInclude(x => x.IncomeSource)
                .Include(x => x.MemberRestrictions)
                    .ThenInclude(x => x.DietaryRestriction)
                .Include(x => x.MemberConcerns)
                    .ThenInclude(x => x.HealthConcern)
                .FirstOrDefault(x => x.ID == id);

            if (memberToUpdate == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update Member
                    _context.Update(memberToUpdate);
                    await _context.SaveChangesAsync();

                    // Update Member Documents
                    await AddDocumentsAsync(memberToUpdate, theFiles);

                    // Update Member Income Sources

                    // Update Member Restrictions

                    // Update Member Concerns

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

                    return Json(new { success = true });
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

            PopulateDropDownLists(memberToUpdate);
            return View(memberToUpdate);
        }

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

            if(member != null)
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

                return Json(new { success = true });
            }
            
            return View(member);
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

            if(memberToUpdate != null)
            {
                double incomeTotal = 0;
                foreach(var i in memberToUpdate.MemberIncomes)
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

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.ID == id);
        }
    }
}
