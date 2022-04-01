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

namespace Grow.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly GrowContext _context;

        public TransactionsController(GrowContext context)
        {
            _context = context;
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
                members = members.Where(h => h.Household.StreetNumber.ToUpper().Contains(SearchString.ToUpper())
                                                || h.Household.StreetName.ToUpper().Contains(SearchString.ToUpper()));
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

            string[] sortOptions = new[] { "Transaction Date", "Total", "Household", "Member", "Sales Person" };

            var transaction = from t in _context.Transactions
                              .Include(t => t.Household)
                              .Include(t => t.Member)
                              .Include(t => t.Volunteer)
                              .AsNoTracking()
                              select t;

            if (!String.IsNullOrEmpty(SearchString))
            {
                transaction = transaction.Where(t => t.Household.HouseholdName.ToUpper().Contains(SearchString.ToUpper()));
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
                        .OrderBy(h => h.Volunteer);
                }
                else
                {
                    transaction = transaction
                        .OrderByDescending(h => h.Volunteer);
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
                .Include(t => t.Volunteer)
                .Include(t => t.TransactionDetails)
                    .ThenInclude(td => td.Item)
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

            // Create a new transaction immediately
            Transaction newTrans = new Transaction()
            {
                HouseholdID = member.HouseholdID,
                MemberID = member.ID,
                TransactionDate = DateTime.Today,
                TransactionTotal = 0,
                // Change to actual volunteer later
                VolunteerID = 1
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
                .Include(x => x.Volunteer)
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
                .Include(t => t.Volunteer)
                .Include(x => x.Member)
                .Include(t => t.TransactionDetails)
                .ThenInclude(t => t.Item)
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
                .Include(t => t.Volunteer)
                .Include(x => x.Member)
                .Include(t => t.TransactionDetails)
                .ThenInclude(t => t.Item)
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

        public async void DeleteNewTransaction(int id)
        {
            var transaction = _context.Transactions.Find(id);
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
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

            var vQuery = from v in _context.Volunteers
                         orderby v.LastName, v.FirstName
                         select v;

            var cQuery = from c in _context.Cities
                         orderby c.CityName
                         select c;

            ViewData["HouseholdID"] = new SelectList(hQuery, "ID", "HouseholdName", transaction?.HouseholdID);
            ViewData["VolunteerID"] = new SelectList(vQuery, "ID", "FormalName", transaction?.VolunteerID);
            ViewData["CityID"] = new SelectList(cQuery, "ID", "CityName");
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.ID == id);
        }
    }
}
