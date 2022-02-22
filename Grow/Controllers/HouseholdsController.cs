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
    public class HouseholdsController : Controller
    {
        private readonly GrowContext _context;

        public HouseholdsController(GrowContext context)
        {
            _context = context;
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
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "CityName");

            PopulateDropDownLists();
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MembershipNumber,NumOfMembers,CreatedDate,LICOVerified,LICOVerifiedDate,IncomeTotal,RenewalDate,StreetNumber,StreetName,ApartmentNumber,PostalCode,ProvinceID,CityID")] Household household)
        {
            if (ModelState.IsValid)
            {
                _context.Add(household);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "CityName", household.CityID);

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

            var household = await _context.Households.FindAsync(id);
            if (household == null)
            {
                return NotFound();
            }
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "CityName", household.CityID);

            PopulateDropDownLists(household);
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MembershipNumber,NumOfMembers,CreatedDate,LICOVerified,LICOVerifiedDate,IncomeTotal,RenewalDate,StreetNumber,StreetName,ApartmentNumber,PostalCode,ProvinceID,CityID")] Household household)
        {
            if (id != household.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(household);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HouseholdExists(household.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "CityName", household.CityID);

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
            var household = await _context.Households.FindAsync(id);
            _context.Households.Remove(household);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
    }
}
