using Grow.Data;
using Grow.Models;
using Grow.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin, Volunteer")]
    public class FaqsController : Controller
    {
        private readonly GrowContext _context;

        public FaqsController(GrowContext context)
        {
            _context = context;
        }

        // GET: FaqController
        public async Task<IActionResult> Index(string SearchString)
        {
            //Toggle the Open/Closed state of the collapse depending on if we are filtering
            ViewData["Filtering"] = ""; //Asume not filtering
            //Then in each "test" for filtering, add ViewData["Filtering"] = " show" if true;

            var questions = from q in _context.Faqs
                .AsNoTracking()
            select q;



            if (!String.IsNullOrEmpty(SearchString))
            {
                questions = questions.Where(p => p.Question.ToUpper().Contains(SearchString.ToUpper()));
                                       
            }
            return View(await questions.ToListAsync());
        }

        // GET: FaqController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewDataReturnURL();
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.Faqs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (faq == null)
            { 
                return NotFound();
            }

            return View(faq);
        }

        

        // GET: FaqController/Create
        public IActionResult Create()
        {
            ViewDataReturnURL();
            return View();
        }

        // POST: FaqController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("ID,Question,Answer")] Faq faq)
        {
            //URL with the last filter, sort and page parameters for this controller
            ViewDataReturnURL();

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(faq);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { faq.ID });
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(faq);
        }

        // GET: FaqController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //URL with the last filter, sort and page parameters for this controller
            ViewDataReturnURL();

            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.Faqs.FindAsync(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        // POST: FaqController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            //URL with the last filter, sort and page parameters for this controller
            ViewDataReturnURL();

            //Go get the Coach to update
            var faqToUpdate = await _context.Faqs.SingleOrDefaultAsync(p => p.ID == id);

            //Check that you got it or exit with a not found error
            if (faqToUpdate == null)
            {
                return NotFound();
            }

            //Try updating it with the values posted
            if (await TryUpdateModelAsync<Faq>(faqToUpdate, "",
                d => d.Question, d => d.Answer))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { faqToUpdate.ID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaqExists(faqToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }

            }
            return View(faqToUpdate);
        }

        // GET: FaqController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewDataReturnURL();
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.Faqs
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        // POST: FaqController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewDataReturnURL();
            var faq = await _context.Faqs
                .FirstOrDefaultAsync(m => m.ID == id);

            _context.Faqs.Remove(faq);
            await _context.SaveChangesAsync();
           // return View(faq);
            return Redirect(ViewData["returnURL"].ToString());
        }
        private bool FaqExists(int id)
        {
            return _context.Faqs.Any(e => e.ID == id);
        }
        private string ControllerName()
        {
            return this.ControllerContext.RouteData.Values["controller"].ToString();
        }
        private void ViewDataReturnURL()
        {
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, ControllerName());
        }
    }
}
