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
    public class DietaryRestrictionsController : Controller
    {
        private readonly GrowContext _context;

        public DietaryRestrictionsController(GrowContext context)
        {
            _context = context;
        }

        // GET: DietaryRestrictions
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Lookups");
        }

        // GET: DietaryRestrictions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dietaryRestriction = await _context.DietaryRestrictions
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dietaryRestriction == null)
            {
                return NotFound();
            }

            return View(dietaryRestriction);
        }

        // GET: DietaryRestrictions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DietaryRestrictions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Restriction")] DietaryRestriction dietaryRestriction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dietaryRestriction);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Lookups");
            }
            return View(dietaryRestriction);
        }

        // GET: DietaryRestrictions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dietaryRestriction = await _context.DietaryRestrictions.FindAsync(id);
            if (dietaryRestriction == null)
            {
                return NotFound();
            }
            return View(dietaryRestriction);
        }

        // POST: DietaryRestrictions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Restriction")] DietaryRestriction dietaryRestriction)
        {
            if (id != dietaryRestriction.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dietaryRestriction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DietaryRestrictionExists(dietaryRestriction.ID))
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
            return View(dietaryRestriction);
        }

        // GET: DietaryRestrictions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dietaryRestriction = await _context.DietaryRestrictions
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dietaryRestriction == null)
            {
                return NotFound();
            }

            return View(dietaryRestriction);
        }

        // POST: DietaryRestrictions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dietaryRestriction = await _context.DietaryRestrictions.FindAsync(id);
            _context.DietaryRestrictions.Remove(dietaryRestriction);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Lookups");
        }

        private bool DietaryRestrictionExists(int id)
        {
            return _context.DietaryRestrictions.Any(e => e.ID == id);
        }
    }
}
