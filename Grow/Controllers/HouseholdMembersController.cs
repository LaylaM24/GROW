using Grow.Data;
using Grow.Models;
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

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Households");
        }

        public PartialViewResult CreateMember(int? ID)
        {
            ViewData["HouseholdID"] = ID.GetValueOrDefault();

            PopulateDropDownLists();

            return PartialView("_CreateMember");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,DOB,Phone,Email,IncomeVerified,IncomeAmount,DataConsent,HouseholdID,GenderID")] Member member,
            string[] selectedOptions, List<IFormFile> theFiles, string IncomeSource1, double IncomeAmount1, string IncomeSource2, double IncomeAmount2,
            string IncomeSource3, double IncomeAmount3)
        {
            try
            {
                if (selectedOptions != null)
                {
                    foreach (var restriction in selectedOptions)
                    {
                        var restrictionToAdd = new MemberRestriction { MemberID = member.ID, DietaryRestrictionID = int.Parse(restriction) };
                        member.MemberRestrictions.Add(restrictionToAdd);
                    }
                }
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

            PopulateDropDownLists(member);
            return View(member);
        }

        public PartialViewResult EditMember(int ID)
        {
            // Get the member to Edit
            var member = _context.Members.Find(ID);

            PopulateDropDownLists();

            return PartialView("_EditMember", member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,DOB,Phone,Email,IncomeVerified,IncomeAmount,DataConsent,HouseholdID,GenderID")] Member member,
            string[] selectedOptions, List<IFormFile> theFiles, string IncomeSource1, double IncomeAmount1, string IncomeSource2, double IncomeAmount2,
            string IncomeSource3, double IncomeAmount3)
        {
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

            PopulateDropDownLists(memberToUpdate);
            return View(memberToUpdate);
        }

        public PartialViewResult DeleteMember(int ID)
        {
            // Get the member to Delete
            var member = _context.Members.Find(ID);

            return PartialView("_DeleteMember", member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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

        private void PopulateDropDownLists(Member member = null)
        {
            var genders = from t1 in _context.Genders
                          orderby t1.GenderType
                          select t1;

            var dRestrictions = from t1 in _context.DietaryRestrictions
                                orderby t1.Restriction
                                select t1;

            var hConcerns = from t1 in _context.HealthConcerns
                            orderby t1.Concern
                            select t1;

            var iSources = from t1 in _context.IncomeSources
                           orderby t1.Source
                           select t1;

            ViewData["Genders"] = new SelectList(genders, "ID", "GenderType", member?.GenderID);
            ViewData["Restrictions"] = new SelectList(dRestrictions, "ID", "Restriction");
            ViewData["Concerns"] = new SelectList(hConcerns, "ID", "Concern");
            ViewData["Sources"] = new SelectList(iSources, "ID", "Source");
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

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.ID == id);
        }
    }
}
