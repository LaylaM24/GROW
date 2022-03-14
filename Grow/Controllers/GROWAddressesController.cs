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
    public class GROWAddressesController : Controller
    {
        private readonly GrowContext _context;

        public GROWAddressesController(GrowContext context)
        {
            _context = context;
        }

        // GET: GROWAddresses/Edit/5
        public async Task<IActionResult> Edit()
        {
            var address = _context.GROWAddresses.Include(x => x.City).FirstOrDefault();

            if (address == null)
            {
                return NotFound();
            }

            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "CityName", address.CityID);
            return View(address);
        }

        // POST: GROWAddresses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StreetNumber,StreetName,ApartmentNumber,PostalCode,CityID")] GROWAddress address)
        {
            if (id != address.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GROWAddressExists(address.ID))
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

            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "CityName", address.CityID);
            return View(address);
        }

        private bool GROWAddressExists(int id)
        {
            return _context.GROWAddresses.Any(e => e.ID == id);
        }
    }
}
