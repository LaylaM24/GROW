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
            string actionButton, string sortDirection = "asc", string sortField = "Household")
        {
            ViewData["Filtering"] = "";

            PopulateDropDownLists();

            string[] sortOptions = new[] { "Household", "Membership No.", "Address" };

            var households = from h in _context.Households
                             .Include(h => h.City)
                             .Include(h => h.Members)
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
                        .ThenBy(h => h.StreetNumber)
                        .ThenBy(h => h.City.CityName)
                        .ThenBy(h => h.PostalCode);
                }
                else
                {
                    households = households
                        .OrderByDescending(h => h.StreetName)
                        .ThenByDescending(h => h.StreetNumber)
                        .ThenByDescending(h => h.City.CityName)
                        .ThenByDescending(h => h.PostalCode);
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

        public async Task<IActionResult> SalesHistory(string SearchString, int? page, int? pageSizeID,
            string actionButton, string sortDirection = "desc", string sortField = "Transaction Date")
        {
            ViewData["Filtering"] = "";

            string[] sortOptions = new[] { "Transaction Date", "Total", "Household", "Sales Person" };

            var transaction = from t in _context.Transactions
                              .Include(t => t.Household)
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
            if(id < 0)
            {
                return RedirectToAction("Index", "Transactions");
            }

            // Create a new transaction immediately
            Transaction newTrans = new Transaction()
            {
                HouseholdID = id,
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
                .Include(x => x.TransactionDetails)
                .Include(x => x.Volunteer)
                .FirstOrDefault(x => x.ID == newTrans.ID);

            return View(trans);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TransactionDate,TransactionTotal,HouseholdID,VolunteerID")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TransDate = DateTime.Today.ToShortDateString();
            PopulateDropDownLists();
            return View(transaction);
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

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,TransactionDate,TransactionTotal,HouseholdID,VolunteerID")] Transaction transaction)
        //{
        //    if (id != transaction.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(transaction);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TransactionExists(transaction.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //    }
        //    PopulateDropDownLists();
        //    return View(transaction);
        //}

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
