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
    public class PaymentMethodsController : Controller
    {
        private readonly GrowContext _context;

        public PaymentMethodsController(GrowContext context)
        {
            _context = context;
        }

        // GET: PaymentMethods
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Lookups");
        }

        // GET: PaymentMethods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var method = await _context.PaymentMethods
                .FirstOrDefaultAsync(m => m.ID == id);

            if (method == null)
            {
                return NotFound();
            }

            return View(method);
        }

        // GET: PaymentMethods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PaymentMethods/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Method")] PaymentMethod method)
        {
            if (ModelState.IsValid)
            {
                _context.Add(method);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Lookups");
            }
            return View(method);
        }

        // GET: PaymentMethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var method = await _context.PaymentMethods.FindAsync(id);

            if (method == null)
            {
                return NotFound();
            }

            return View(method);
        }

        // POST: PaymentMethods/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Method")] PaymentMethod method)
        {
            if (id != method.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(method);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentMethodExists(method.ID))
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
            return View(method);
        }

        // GET: PaymentMethods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var method = await _context.PaymentMethods
                .FirstOrDefaultAsync(m => m.ID == id);

            if (method == null)
            {
                return NotFound();
            }

            return View(method);
        }

        // POST: PaymentMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var method = await _context.PaymentMethods.FindAsync(id);
            _context.PaymentMethods.Remove(method);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Lookups");
        }

        private bool PaymentMethodExists(int id)
        {
            return _context.PaymentMethods.Any(e => e.ID == id);
        }
    }
}
