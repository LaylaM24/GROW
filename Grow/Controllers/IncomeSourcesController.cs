using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grow.Data;
using Grow.Models;

namespace Grow.Controllers
{
    public class IncomeSourcesController : Controller
    {
        private readonly GrowContext _context;

        public IncomeSourcesController(GrowContext context)
        {
            _context = context;
        }

        // GET: IncomeSources
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Lookups");
        }

        // GET: IncomeSources/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incomeSource = await _context.IncomeSources
                .FirstOrDefaultAsync(m => m.ID == id);
            if (incomeSource == null)
            {
                return NotFound();
            }

            return View(incomeSource);
        }

        // GET: IncomeSources/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IncomeSources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Source")] IncomeSource incomeSource)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incomeSource);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Lookups");
            }
            return View(incomeSource);
        }

        // GET: IncomeSources/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incomeSource = await _context.IncomeSources.FindAsync(id);
            if (incomeSource == null)
            {
                return NotFound();
            }
            return View(incomeSource);
        }

        // POST: IncomeSources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Source")] IncomeSource incomeSource)
        {
            if (id != incomeSource.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incomeSource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncomeSourceExists(incomeSource.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Lookups");
            }
            return View(incomeSource);
        }

        // GET: IncomeSources/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incomeSource = await _context.IncomeSources
                .FirstOrDefaultAsync(m => m.ID == id);
            if (incomeSource == null)
            {
                return NotFound();
            }

            return View(incomeSource);
        }

        // POST: IncomeSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incomeSource = await _context.IncomeSources.FindAsync(id);
            _context.IncomeSources.Remove(incomeSource);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Lookups");
        }

        private bool IncomeSourceExists(int id)
        {
            return _context.IncomeSources.Any(e => e.ID == id);
        }
    }
}
