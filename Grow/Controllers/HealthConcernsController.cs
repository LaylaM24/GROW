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
    public class HealthConcernsController : Controller
    {
        private readonly GrowContext _context;

        public HealthConcernsController(GrowContext context)
        {
            _context = context;
        }

        // GET: HealthConcerns
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Lookups");
        }

        // GET: HealthConcerns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthConcern = await _context.HealthConcerns
                .FirstOrDefaultAsync(m => m.ID == id);
            if (healthConcern == null)
            {
                return NotFound();
            }

            return View(healthConcern);
        }

        // GET: HealthConcerns/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HealthConcerns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Concern")] HealthConcern healthConcern)
        {
            if (ModelState.IsValid)
            {
                _context.Add(healthConcern);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Lookups");
            }
            return View(healthConcern);
        }

        // GET: HealthConcerns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthConcern = await _context.HealthConcerns.FindAsync(id);
            if (healthConcern == null)
            {
                return NotFound();
            }
            return View(healthConcern);
        }

        // POST: HealthConcerns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Concern")] HealthConcern healthConcern)
        {
            if (id != healthConcern.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(healthConcern);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HealthConcernExists(healthConcern.ID))
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
            return View(healthConcern);
        }

        // GET: HealthConcerns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthConcern = await _context.HealthConcerns
                .FirstOrDefaultAsync(m => m.ID == id);
            if (healthConcern == null)
            {
                return NotFound();
            }

            return View(healthConcern);
        }

        // POST: HealthConcerns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var healthConcern = await _context.HealthConcerns.FindAsync(id);
            _context.HealthConcerns.Remove(healthConcern);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Lookups");
        }

        private bool HealthConcernExists(int id)
        {
            return _context.HealthConcerns.Any(e => e.ID == id);
        }
    }
}
