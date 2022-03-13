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
    public class ItemsController : Controller
    {
        private readonly GrowContext _context;

        public ItemsController(GrowContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index(string SearchString, int? ItemCategoryID, int? page, int? pageSizeID,
            string actionButton, string sortDirection = "asc", string sortField = "Item No.")
        {
            CookieHelper.CookieSet(HttpContext, ControllerName() + "URL", "", -1);

            ViewData["Filtering"] = "";
            
            PopulateDropDownLists();

            string[] sortOptions = new[] { "Item No.", "Name", "Price", "Category" };

            var items = from p in _context.Items
                        .Include(p => p.ItemCategory)
                        .AsNoTracking()
                        select p;

            if (ItemCategoryID.HasValue)
            {
                items = items.Where(p => p.ItemCategoryID == ItemCategoryID);
                ViewData["Filtering"] = " show";
            }
            
            if (!String.IsNullOrEmpty(SearchString))
            {
                items = items.Where(p => p.ItemName.ToUpper().Contains(SearchString.ToUpper()));
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

            if (sortField == "Item No.")
            {
                if (sortDirection == "asc")
                {
                    items = items
                        .OrderBy(i => i.ItemNo);
                }
                else
                {
                    items = items
                        .OrderByDescending(i => i.ItemNo);
                }
            }
            else if (sortField == "Name")
            {
                if (sortDirection == "asc")
                {
                    items = items
                        .OrderBy(i => i.ItemName);
                }
                else
                {
                    items = items
                        .OrderByDescending(i => i.ItemName);
                }
            }
            else if (sortField == "Price")
            {
                if (sortDirection == "asc")
                {
                    items = items
                        .OrderBy(i => i.Price);
                }
                else
                {
                    items = items
                        .OrderByDescending(i => i.Price);
                }
            }
            else
            {
                if (sortDirection == "asc")
                {
                    items = items
                        .OrderBy(i => i.ItemCategory.CategoryName);
                }
                else
                {
                    items = items
                        .OrderByDescending(i => i.ItemCategory.CategoryName);
                }
            }
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID);
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Item>.CreateAsync(items.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewDataReturnURL();

            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemCategory)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewDataReturnURL();

            ViewData["ItemCategoryID"] = new SelectList(_context.ItemCategories, "ID", "CategoryName");
            PopulateDropDownLists();

            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ItemNo,ItemName,Price,ItemCategoryID")] Item item)
        {
            ViewDataReturnURL();

            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDownLists();
            return RedirectToAction("Details", new { item.ID });
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewDataReturnURL();

            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            
            ViewData["ItemCategoryID"] = new SelectList(_context.ItemCategories, "ID", "CategoryName", item.ItemCategoryID);
            PopulateDropDownLists();

            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ItemNo,ItemName,Price,ItemCategoryID")] Item item)
        {
            ViewDataReturnURL();

            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ID))
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
            PopulateDropDownLists();
            return RedirectToAction("Details", new { item.ID });
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewDataReturnURL();

            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemCategory)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewDataReturnURL();

            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);

            await _context.SaveChangesAsync();
            return Redirect(ViewData["returnURL"].ToString());
        }

        private void PopulateDropDownLists(Item item = null)
        {
            var cQuery = from c in _context.ItemCategories
                         orderby c.CategoryName
                         select c;

            ViewData["ItemCategoryID"] = new SelectList(cQuery, "ID", "CategoryName", item?.ItemCategoryID);
        }

        private string ControllerName()
        {
            return this.ControllerContext.RouteData.Values["controller"].ToString();
        }

        private void ViewDataReturnURL()
        {
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, ControllerName());
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ID == id);
        }
    }
}
