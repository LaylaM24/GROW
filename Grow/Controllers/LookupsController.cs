using Grow.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Controllers
{
    public class LookupsController : Controller
    {
        private readonly GrowContext _context;

        public LookupsController(GrowContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.GrowAddress = _context.GROWAddresses.Include(x => x.City).FirstOrDefault().Address;
            ViewData["HealthConcernsID"] = new SelectList(_context.HealthConcerns.OrderBy(a => a.Concern), "ID", "Concern");
            ViewData["DietaryRestrictionsID"] = new SelectList(_context.DietaryRestrictions.OrderBy(a => a.Restriction), "ID", "Restriction");
            ViewData["IncomeSourcesID"] = new SelectList(_context.IncomeSources.OrderBy(a => a.Source), "ID", "Source");
            ViewData["CitiesID"] = new SelectList(_context.Cities.OrderBy(a => a.CityName), "ID", "CityName");
            ViewData["GendersID"] = new SelectList(_context.Genders.OrderBy(a => a.GenderType), "ID", "GenderType");
            ViewData["ItemCategoriesID"] = new SelectList(_context.ItemCategories.OrderBy(a => a.CategoryName), "ID", "CategoryName");
            return View();
        }
    }
}
