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
using Microsoft.AspNetCore.Authorization;
using Grow.ViewModels;

namespace Grow.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin, Volunteer")]
    public class TransactionsController : Controller
    {
        private readonly GrowContext _context;
        private readonly IMyEmailSender _emailSender;

        public TransactionsController(GrowContext context, IMyEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: Transactions
        public async Task<IActionResult> Index(string SearchString, int? CityID, int? page, int? pageSizeID,
            string actionButton, string sortDirection = "asc", string sortField = "Membership No.")
        {
            ViewData["Filtering"] = "";

            PopulateDropDownLists();

            string[] sortOptions = new[] { "Household", "Membership No.", "Member", "Address" };

            var members = from h in _context.Members
                             .Include(x => x.Household)
                             .ThenInclude(h => h.City)
                             .AsNoTracking()
                             select h;

            if (CityID.HasValue)
            {
                members = members.Where(h => h.Household.CityID == CityID);
                ViewData["Filtering"] = " show";
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                SearchString = SearchString.Trim();
                members = members.Where(h => (h.Household.StreetNumber + " " + h.Household.StreetName + (h.Household.ApartmentNumber != null ? " Apt. " + h.Household.ApartmentNumber : "") 
                                                + ", " + h.Household.City.CityName + " " + h.Household.PostalCode).ToUpper().Contains(SearchString.ToUpper())
                                                || h.Household.HouseholdName.ToUpper().Contains(SearchString.ToUpper()) || (h.FirstName + " " + h.LastName).ToUpper().Contains(SearchString.ToUpper()));
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
                    members = members
                        .OrderBy(h => h.Household.MembershipNumber);
                }
                else
                {
                    members = members
                        .OrderByDescending(h => h.Household.MembershipNumber);
                }
            }
            else if (sortField == "Member")
            {
                if (sortDirection == "asc")
                {
                    members = members
                        .OrderBy(h => h.LastName)
                        .ThenBy(x => x.FirstName);
                }
                else
                {
                    members = members
                        .OrderByDescending(h => h.LastName)
                        .ThenByDescending(x => x.FirstName);
                }
            }
            else if (sortField == "Address")
            {
                if (sortDirection == "asc")
                {
                    members = members
                        .OrderBy(h => h.Household.StreetName)
                        .ThenBy(h => h.Household.StreetNumber)
                        .ThenBy(h => h.Household.City.CityName)
                        .ThenBy(h => h.Household.PostalCode);
                }
                else
                {
                    members = members
                        .OrderByDescending(h => h.Household.StreetName)
                        .ThenByDescending(h => h.Household.StreetNumber)
                        .ThenByDescending(h => h.Household.City.CityName)
                        .ThenByDescending(h => h.Household.PostalCode);
                }
            }
            else
            {
                if (sortDirection == "asc")
                {
                    members = members
                        .OrderBy(h => h.Household.HouseholdName);
                }
                else
                {
                    members = members
                        .OrderByDescending(h => h.Household.HouseholdName);
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID);
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Member>.CreateAsync(members.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        public async Task<IActionResult> SalesHistory(string SearchString, int? page, int? pageSizeID,
            string actionButton, string sortDirection = "desc", string sortField = "Transaction Date")
        {
            ViewData["Filtering"] = "";

            string[] sortOptions = new[] { "Transaction Date", "Total", "Household", "Member", "Sales Person", "Paid" };

            var transaction = from t in _context.Transactions
                              .Include(t => t.Household)
                              .Include(t => t.Member)
                              .Include(t => t.Employee)
                              .AsNoTracking()
                              select t;

            if (!String.IsNullOrEmpty(SearchString))
            {
                transaction = transaction.Where(t => t.Household.HouseholdName.ToUpper().Contains(SearchString.ToUpper()) || (t.Member.FirstName + " " + t.Member.LastName).ToUpper().Contains(SearchString.ToUpper())
                                                    || (t.Employee.FirstName + " " + t.Employee.LastName).ToUpper().Contains(SearchString.ToUpper()));
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

            if (sortField == "Total")
            {
                if (sortDirection == "asc")
                {
                    transaction = transaction
                        .OrderBy(h => h.TransactionTotal);
                }
                else
                {
                    transaction = transaction
                        .OrderByDescending(h => h.TransactionTotal);
                }
            }
            else if (sortField == "Sales Person")
            {
                if (sortDirection == "asc")
                {
                    transaction = transaction
                        .OrderBy(h => h.Employee);
                }
                else
                {
                    transaction = transaction
                        .OrderByDescending(h => h.Employee);
                }
            }
            else if (sortField == "Household")
            {
                if (sortDirection == "asc")
                {
                    transaction = transaction
                        .OrderBy(h => h.Household.HouseholdName);
                }
                else
                {
                    transaction = transaction
                        .OrderByDescending(h => h.Household.HouseholdName);
                }
            }
            else if (sortField == "Member")
            {
                if (sortDirection == "asc")
                {
                    transaction = transaction
                        .OrderBy(h => h.Member.LastName)
                        .ThenBy(x => x.Member.FirstName);
                }
                else
                {
                    transaction = transaction
                        .OrderByDescending(h => h.Member.LastName)
                        .ThenByDescending(x => x.Member.FirstName);
                }
            }
            else if (sortField == "Paid")
            {
                if (sortDirection == "asc")
                {
                    transaction = transaction
                        .OrderBy(h => h.Paid);
                }
                else
                {
                    transaction = transaction
                        .OrderByDescending(h => h.Paid);
                }
            }
            else
            {
                if (sortDirection == "asc")
                {
                    transaction = transaction
                        .OrderBy(h => h.TransactionDate);
                }
                else
                {
                    transaction = transaction
                        .OrderByDescending(h => h.TransactionDate);
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID);
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Transaction>.CreateAsync(transaction.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Household)
                .ThenInclude(x => x.City)
                .Include(x => x.Member)
                .Include(t => t.Employee)
                .Include(t => t.TransactionDetails)
                .ThenInclude(td => td.Item)
                .Include(x => x.Payments)
                .ThenInclude(x => x.PaymentMethod)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create(int id)
        {
            // Get member
            var member = _context.Members.FirstOrDefault(x => x.ID == id);

            if (member == null)
            {
                return RedirectToAction("Index", "Transactions");
            }

            // Get employee info
            Employee employee = new Employee();
            try
            {
                employee = _context.Employees.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            }
            catch { }

            // Create a new transaction immediately
            Transaction newTrans = new Transaction()
            {
                HouseholdID = member.HouseholdID,
                MemberID = member.ID,
                TransactionDate = DateTime.Today,
                TransactionTotal = 0,
                Paid = false,
                // Change to actual volunteer later
                EmployeeID = employee.ID
            };

            try
            {
                _context.Transactions.Add(newTrans);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Transactions");
            }

            Transaction trans = _context.Transactions
                .Include(x => x.Household)
                .ThenInclude(x => x.City)
                .Include(x => x.Member)
                .Include(x => x.TransactionDetails)
                .Include(x => x.Employee)
                .Include(x => x.Payments)
                .ThenInclude(x => x.PaymentMethod)
                .FirstOrDefault(x => x.ID == newTrans.ID);

            return View(trans);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Household)
                .ThenInclude(x => x.City)
                .Include(t => t.Employee)
                .Include(x => x.Member)
                .Include(t => t.TransactionDetails)
                .ThenInclude(t => t.Item)
                .Include(x => x.Payments)
                .ThenInclude(x => x.PaymentMethod)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (transaction == null)
            {
                return NotFound();
            }

            PopulateDropDownLists();
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Household)
                .ThenInclude(x => x.City)
                .Include(t => t.Employee)
                .Include(x => x.Member)
                .Include(t => t.TransactionDetails)
                .ThenInclude(t => t.Item)
                .Include(x => x.Payments)
                .ThenInclude(x => x.PaymentMethod)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SalesHistory));
        }

        public PartialViewResult ItemList(int id)
        {
            var transaction = _context.Transactions
                .Include(x => x.TransactionDetails)
                .ThenInclude(x => x.Item)
                .Where(x => x.ID == id)
                .FirstOrDefault();

            return PartialView("_ItemList", transaction);
        }

        private void PopulateDropDownLists(Transaction transaction = null)
        {
            var hQuery = from h in _context.Households
                         orderby h.HouseholdName
                         select h;

            var vQuery = from v in _context.Employees
                         orderby v.LastName, v.FirstName
                         select v;

            var cQuery = from c in _context.Cities
                         orderby c.CityName
                         select c;

            ViewData["HouseholdID"] = new SelectList(hQuery, "ID", "HouseholdName", transaction?.HouseholdID);
            ViewData["VolunteerID"] = new SelectList(vQuery, "ID", "FullName", transaction?.EmployeeID);
            ViewData["CityID"] = new SelectList(cQuery, "ID", "CityName");
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.ID == id);
        }

        public IActionResult SendReceiptEmail(int transID)
        {
            try
            {
                // Get Transaction and Member
                Transaction trans = _context.Transactions.Where(x => x.ID == transID)
                    .Include(x => x.Member)
                    .Include(x => x.Employee)
                    .Include(x => x.TransactionDetails)
                    .ThenInclude(x => x.Item)
                    .Include(x => x.Payments)
                    .FirstOrDefault();
                Member member = _context.Members.Where(x => x.ID == trans.MemberID).FirstOrDefault();

                if(member == null || trans == null)
                {
                    return Json(new { success = false });
                }

                EmailAddress email = new EmailAddress() { Name = member.FullName, Address = member.Email };

                // Get message contents
                string messageContent = $"<h3>Sale Details</h3><p>Member: &emsp; {trans.Member.FullName}</p><p>Transaction Date: &emsp; {trans.TransactionDate.ToShortDateString()}</p>"
                    + $"<p>Sales Person: &emsp; {trans.Employee.FullName}</p><br/><h3>Items</h3>";

                foreach(TransactionDetail td in trans.TransactionDetails)
                {
                    messageContent += $"<p>{td.Item.ItemName} &emsp; Cost: {td.UnitCost.ToString("C")} &emsp; Qty: {td.Quantity} &emsp; Ext. Cost: {td.ExtendedCost.ToString("C")}</p>";
                }

                messageContent += $"<br/><p>Transaction Total: {trans.TransactionTotal.ToString("C")}</p><p>Amount Paid: {trans.Payments.Sum(x => x.PaymentAmount).ToString("C")}</p>" +
                    "<br/><p>Thanks for shopping at GROW Community Food Literacy Centre!</p>";

                var msg = new EmailMessage()
                {
                    ToAddresses = new List<EmailAddress>() { email },
                    Subject = $"GROW Sale Receipt {DateTime.Today.ToShortDateString()}",
                    Content = messageContent
                };

                _emailSender.SendToManyAsync(msg);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
    }
}
