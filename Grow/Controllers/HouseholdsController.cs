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

namespace Grow.Controllers
{
    public class HouseholdsController : Controller
    {
        private readonly GrowContext _context;
        //for sending email
        private readonly IMyEmailSender _emailSender;
        public HouseholdsController(GrowContext context, IMyEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;

        }

        // GET: Households
        public async Task<IActionResult> Index(string SearchString, int? CityID, int? page, int? pageSizeID,
            string actionButton, string sortDirection = "asc", string sortField = "Household")
        {
            ViewData["Filtering"] = "";

            PopulateDropDownLists();

            string[] sortOptions = new[] { "Household", "Membership No.", "Address", "City", "Postal Code", "Total Income" };

            var households = from h in _context.Households
                             .Include(h => h.City)
                             .AsNoTracking()
                             select h;

            if (CityID.HasValue)
            {
                households = households.Where(h => h.CityID == CityID);
                ViewData["Filtering"] = " show";
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                households = households.Where(h => h.StreetNumber.ToUpper().Contains(SearchString.ToUpper())
                                                || h.StreetName.ToUpper().Contains(SearchString.ToUpper()));
                ViewData["Filtering"] = " show";
            }

            if (!String.IsNullOrEmpty(actionButton))
            {
                page = 1;

                if (sortOptions.Contains(actionButton))
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
                    households = households
                        .OrderBy(h => h.MembershipNumber);
                }
                else
                {
                    households = households
                        .OrderByDescending(h => h.MembershipNumber);
                }
            }
            else if (sortField == "Address")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(h => h.StreetName)
                        .ThenBy(h => h.StreetNumber);
                }
                else
                {
                    households = households
                        .OrderByDescending(h => h.StreetName)
                        .ThenByDescending(h => h.StreetNumber);
                }
            }
            else if (sortField == "City")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(h => h.City.CityName);
                }
                else
                {
                    households = households
                        .OrderByDescending(h => h.City.CityName);
                }
            }
            else if (sortField == "Postal Code")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(h => h.PostalCode);
                }
                else
                {
                    households = households
                        .OrderByDescending(h => h.PostalCode);
                }
            }
            else if (sortField == "Total Income")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(h => h.IncomeTotal);
                }
                else
                {
                    households = households
                        .OrderByDescending(h => h.IncomeTotal);
                }
            }
            else
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(h => h.HouseholdName);
                }
                else
                {
                    households = households
                        .OrderByDescending(h => h.HouseholdName);
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID);
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Household>.CreateAsync(households.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        // GET: Households/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var household = await _context.Households
                .Include(h => h.City)
                .Include(h => h.Members)
                .ThenInclude(m => m.Gender)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (household == null)
            {
                return NotFound();
            }

            return View(household);
        }

        // GET: Households/Create
        public IActionResult Create()
        {
            // Get next available Membership No.
            ViewData["NextMembershipNo"] = _context.Households.OrderByDescending(x => x.MembershipNumber).FirstOrDefault().MembershipNumber + 1;

            ViewBag.GrowAddress = _context.GROWAddresses.FirstOrDefault();

            PopulateDropDownLists();
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembershipNumber,StreetNumber,StreetName,ApartmentNumber,PostalCode,HouseholdName,RenewalDate,CityID")] Household household)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Set calculated fields to defaults
                    household.Active = false;
                    household.NumOfMembers = 0;
                    household.CreatedDate = DateTime.Today;
                    household.LICOVerified = false;
                    household.LICOVerifiedDate = null;
                    household.IncomeTotal = 0.00;
                    household.RenewalDate = new DateTime(DateTime.Today.Year + 1, DateTime.Today.Month, DateTime.Today.Day);

                    // Add Household
                    _context.Add(household);
                    await _context.SaveChangesAsync();

                    // Add entry to Membership Changes
                    _context.Add(new MembershipChange
                    {
                        HouseholdID = household.ID,
                        ChangeType = "Create",
                        ChangeDescription = "Household Created",
                        ChangeDate = DateTime.Today
                    });
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Edit", "Households", new { id = household.ID });
                }
                catch(Exception e) 
                {
                    if (e.GetBaseException().Message.Contains("UNIQUE constraint failed"))
                    {
                        ModelState.AddModelError("MembershipNumber", "Unable to save changes. Duplicate Membership Numbers are not allowed.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }

            // Get next available Membership No.
            ViewData["NextMembershipNo"] = _context.Households.OrderByDescending(x => x.MembershipNumber).FirstOrDefault().MembershipNumber + 1;

            ViewBag.GrowAddress = _context.GROWAddresses.FirstOrDefault();

            PopulateDropDownLists(household);
            return View(household);
        }

        // GET: Households/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var household = await _context.Households
                .Include(x => x.Members)
                .ThenInclude(x => x.Gender)
                .FirstOrDefaultAsync(x => x.ID == id);

            if (household == null)
            {
                return NotFound();
            }

            ViewBag.GrowAddress = _context.GROWAddresses.FirstOrDefault();

            PopulateDropDownLists(household);
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MembershipNumber,CreatedDate,RenewalDate,StreetNumber,StreetName,ApartmentNumber,PostalCode,HouseholdName,CityID,City,Members")] Household household)
        {
            if (id != household.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get Household before update
                    var originalHousehold = _context.Households
                        .Include(x => x.City)
                        .AsNoTracking()
                        .FirstOrDefault(x => x.ID == id);

                    // Get updated City for comparison
                    household.City = _context.Cities.FirstOrDefault(x => x.ID == household.CityID);

                    // Update calculated fields
                    UpdateCalculatedFields(household);

                    // Update Household
                    _context.Update(household);

                    // Add entry to Membership Changes
                    string changes = "";
                    if(household.MembershipNumber != originalHousehold.MembershipNumber)
                    {
                        changes += $"- Membership No. changed from {originalHousehold.MembershipNumber} to {household.MembershipNumber}. \n";
                    }
                    if (household.HouseholdName != originalHousehold.HouseholdName)
                    {
                        changes += $"- Household Name changed from {originalHousehold.HouseholdName} to {household.HouseholdName}. \n";
                    }
                    if (household.Address != originalHousehold.Address)
                    {
                        changes += $"- Address changed from {originalHousehold.Address} to {household.Address}. \n";
                    }
                    if (household.CityID != originalHousehold.CityID)
                    {
                        changes += $"- City changed from {originalHousehold.City.CityName} to {household.City.CityName}. \n";
                    }
                    if (household.PostalCode != originalHousehold.PostalCode)
                    {
                        changes += $"- Postal Code changed from {originalHousehold.PostalCode} to {household.PostalCode}. \n";
                    }
                    if (household.RenewalDate != originalHousehold.RenewalDate)
                    {
                        changes += $"- Renewal Date changed from {originalHousehold.RenewalDate} to {household.RenewalDate}. \n";
                    }

                    _context.Add(new MembershipChange
                    {
                        HouseholdID = household.ID,
                        ChangeType = "Edit",
                        ChangeDescription = changes,
                        ChangeDate = DateTime.Today
                    });

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "Households", new { id = household.ID });
                }
                catch (Exception e)
                {
                    if (e.GetBaseException().Message.Contains("UNIQUE constraint failed"))
                    {
                        ModelState.AddModelError("MembershipNumber", "Unable to save changes. Duplicate Membership Numbers are not allowed.");
                    }
                    else if (!HouseholdExists(household.ID))
                    {
                        ModelState.AddModelError("", "Unable to save changes. Household does not exist.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }

            ViewBag.GrowAddress = _context.GROWAddresses.FirstOrDefault();

            PopulateDropDownLists(household);
            return View(household);
        }

        // GET: Households/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var household = await _context.Households
                .Include(x => x.Members)
                .ThenInclude(x => x.Gender)
                .Include(h => h.City)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (household == null)
            {
                return NotFound();
            }

            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var household = await _context.Households
                .Include(x => x.Members)
                .ThenInclude(x => x.Gender)
                .Include(h => h.City)
                .FirstOrDefaultAsync(m => m.ID == id);

            try
            {
                _context.Households.Remove(household);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Unable to delete Household. Households cannot be deleted if they contain any Members.");
            }

            return View(household);
        }

        public PartialViewResult MemberList(int id)
        {
            var household = _context.Households
                .Include(x => x.Members)
                .ThenInclude(x => x.Gender)
                .Where(x => x.ID == id)
                .FirstOrDefault();

            return PartialView("_MemberList", household);
        }

        private async void UpdateCalculatedFields(Household household)
        {
            // Get all members
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
        }

        private void PopulateDropDownLists(Household household = null)
        {
            var cQuery = from c in _context.Cities
                         orderby c.CityName
                         select c;

            ViewData["CityID"] = new SelectList(cQuery, "ID", "CityName", household?.CityID);
        }

        private bool HouseholdExists(int id)
        {
            return _context.Households.Any(e => e.ID == id);
        }

                // GET/POST: Grow/Notification/5
        public async Task<IActionResult> Notification(int? id, string Subject, string emailContent)
        {


            if (string.IsNullOrEmpty(Subject) || string.IsNullOrEmpty(emailContent))
            {
                ViewData["Message"] = "You must enter both a Subject Line and Message Content before sending out a notification.";
            }
            else
            {
                int folksCount = 0;
                try
                {
                    //Send a Notice.
                    List<EmailAddress> folks = (from p in _context.Members
                                                where p.DataConsent == true
                                                select new EmailAddress
                                                {
                                                    Name = p.FullName,
                                                    Address = p.Email
                                                }).ToList();
                    folksCount = folks.Count();
                    if (folksCount > 0)
                    {
                        var msg = new EmailMessage()
                        {
                            ToAddresses = folks,
                            Subject = Subject,
                            //possibly use this if theres a signature that wants to be sent by grow
                            Content = "<p>" + emailContent + "</p><p>Insert message from grow here</p>"

                        };
                        await _emailSender.SendToManyAsync(msg);
                        ViewData["Message"] = "Message sent to " + folksCount + " Member"
                            + ((folksCount == 1) ? "." : "s.");
                    }
                    else
                    {
                        ViewData["Message"] = "Message NOT sent!  No Members with data consent!";
                    }
                }
                catch (Exception ex)
                {
                    string errMsg = ex.GetBaseException().Message;
                    ViewData["Message"] = "Error: Could not send email message to the " + folksCount + " Member"
                        + ((folksCount == 1) ? "" : "s") ;
                }
            }
            return View();
        }
    }
}
