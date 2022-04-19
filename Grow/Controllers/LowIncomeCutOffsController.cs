using Grow.Data;
using Grow.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Controllers
{
    public class LowIncomeCutOffsController : Controller
    {
        private readonly GrowContext _context;

        public LowIncomeCutOffsController(GrowContext context)
        {
            _context = context;
        }

        // GET: LowIncomeCutOffs
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Lookups");
        }

        // GET: LowIncomeCutOffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cutOff = await _context.LowIncomeCutOffs
                .FirstOrDefaultAsync(m => m.ID == id);

            if (cutOff == null)
            {
                return NotFound();
            }

            return View(cutOff);
        }

        // GET: LowIncomeCutOffs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LowIncomeCutOffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,NumberOfMembers,YearlyIncome")] LowIncomeCutOff cutOff)
        {
            if (ModelState.IsValid)
            {
                // Check if cutoff already exists for specified number of members
                LowIncomeCutOff c = _context.LowIncomeCutOffs.Where(x => x.NumberOfMembers == cutOff.NumberOfMembers).FirstOrDefault();

                if(c != null)
                {
                    ModelState.AddModelError("NumberOfMembers", "Low Income Cut Off for " + c.NumberOfMembers + " member(s) already exists.");
                    return View(cutOff);
                }

                _context.Add(cutOff);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Lookups");
            }
            return View(cutOff);
        }

        // GET: LowIncomeCutOffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cutOff = await _context.LowIncomeCutOffs.FindAsync(id);

            if (cutOff == null)
            {
                return NotFound();
            }

            return View(cutOff);
        }

        // POST: LowIncomeCutOffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,NumberOfMembers,YearlyIncome")] LowIncomeCutOff cutOff)
        {
            if (id != cutOff.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cutOff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LowIncomeCutOffExists(cutOff.ID))
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
            return View(cutOff);
        }

        // GET: LowIncomeCutOffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cutOff = await _context.LowIncomeCutOffs
                .FirstOrDefaultAsync(m => m.ID == id);

            if (cutOff == null)
            {
                return NotFound();
            }

            return View(cutOff);
        }

        // POST: LowIncomeCutOffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cutOff = await _context.LowIncomeCutOffs.FindAsync(id);
            _context.LowIncomeCutOffs.Remove(cutOff);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Lookups");
        }

        private bool LowIncomeCutOffExists(int id)
        {
            return _context.LowIncomeCutOffs.Any(e => e.ID == id);
        }
    }
}
