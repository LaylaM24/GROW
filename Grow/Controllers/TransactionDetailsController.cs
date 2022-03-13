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
    public class TransactionDetailsController : Controller
    {
        private readonly GrowContext _context;

        public TransactionDetailsController(GrowContext context)
        {
            _context = context;
        }

        public IActionResult Add()
        {
            ViewData["ItemID"] = new SelectList(_context.Items, "ID", "ItemName");
            ViewData["TransactionID"] = new SelectList(_context.Transactions, "ID", "ID");
            return View();
        }

        // GET: TransactionDetails
        public async Task<IActionResult> Index()
        {
            var transactionDetail = _context.TransactionDetails
                .Include(td => td.Item)
                .Include(td => td.Transactions);

            return View(await transactionDetail.ToListAsync());
        }

        // GET: TransactionDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionDetail = await _context.TransactionDetails
                .Include(td => td.Item)
                .Include(td => td.Transactions)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (transactionDetail == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Transactions", new { id = transactionDetail.TransactionID });
        }

        // GET: TransactionDetails/Create
        public IActionResult Create()
        {
            ViewData["ItemID"] = new SelectList(_context.Items, "ID", "ItemName");
            ViewData["TransactionID"] = new SelectList(_context.Transactions, "ID", "ID");
            return View();
        }

        // POST: TransactionDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TransactionID,ItemID,Quantity,UnitCost,ExtendedCost")] TransactionDetail transactionDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transactionDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Transactions", new { id = transactionDetail.TransactionID });
            }

            ViewData["ItemID"] = new SelectList(_context.Items, "ID", "ItemName");
            ViewData["TransactionID"] = new SelectList(_context.Transactions, "ID", "ID");
            return RedirectToAction("Details", "Transactions", new { id = transactionDetail.TransactionID });
        }

        // GET: TransactionDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionDetail = await _context.TransactionDetails.FindAsync(id);
            if (transactionDetail == null)
            {
                return NotFound();
            }

            ViewData["ItemID"] = new SelectList(_context.Items, "ID", "ItemName");
            ViewData["TransactionID"] = new SelectList(_context.Transactions, "ID", "ID"); 

            return RedirectToAction("Details", "Transactions", new { id = transactionDetail.TransactionID });
        }

        // POST: TransactionDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TransactionID,ItemID,Quantity,UnitCost,ExtendedCost")] TransactionDetail transactionDetail)
        {
            if (id != transactionDetail.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transactionDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionDetailExists(transactionDetail.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Transactions", new { id = transactionDetail.TransactionID });
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ID", "ItemName");
            ViewData["TransactionID"] = new SelectList(_context.Transactions, "ID", "ID");
            return RedirectToAction("Details", "Transactions", new { id = transactionDetail.TransactionID });
        }

        // GET: TransactionDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionDetail = await _context.TransactionDetails
                .Include(t => t.Item)
                .Include(t => t.Transactions)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (transactionDetail == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Transactions", new { id = transactionDetail.TransactionID });
        }

        // POST: TransactionDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transactionDetail = await _context.TransactionDetails.FindAsync(id);
            _context.TransactionDetails.Remove(transactionDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Transactions", new { id = transactionDetail.TransactionID });
        }


        //becuase we would need to populate 2 drop down list ive used this code instead instead of calling the method.
        //im sure you could create seperate methods to call but we can now create new transaction details with this.

                        /*-----------------------------------------------------------------------------------------
                        ViewData["ItemID"] = new SelectList(_context.Items, "ID", "ItemName");
                        ViewData["TransactionID"] = new SelectList(_context.Transactions, "ID", "ID");
                        -----------------------------------------------------------------------------------------*/


                        /*
                        private void PopulateDropDownLists(TransactionDetail transactionDetail = null)
                        {
                            var iQuery = from i in _context.Items
                                         orderby i.ItemName
                                         select i;

                            ViewData["ItemID"] = new SelectList(iQuery, "ID", "ItemName", transactionDetail?.ItemID);
                        }
                        */


        private bool TransactionDetailExists(int id)
        {
            return _context.TransactionDetails.Any(e => e.ID == id);
        }
    }
}
