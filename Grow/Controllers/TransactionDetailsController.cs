using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grow.Data;
using Grow.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Grow.Controllers
{
    public class TransactionDetailsController : Controller
    {
        private readonly GrowContext _context;

        public TransactionDetailsController(GrowContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Transactions");
        }

        public PartialViewResult AddItem(int ID)
        {
            // Save trans ID for form
            ViewData["TransactionID"] = ID;

            PopulateDropDownLists();

            return PartialView("_AddItem");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem([Bind("TransactionID, ItemID, Quantity, UnitCost, ExtendedCost")] TransactionDetail transDetail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Get unit cost and extended cost
                    var item = _context.Items.FirstOrDefault(x => x.ID == transDetail.ItemID);

                    transDetail.UnitCost = item.Price;
                    transDetail.ExtendedCost = item.Price * transDetail.Quantity;

                    _context.TransactionDetails.Add(transDetail);
                    _context.SaveChanges();

                    UpdateTransactionTotal(transDetail.TransactionID);

                    return Json(new { success = true });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            PopulateDropDownLists();
            return View(transDetail);
        }


        public PartialViewResult EditItem(int ID)
        {
            var transactionDetail = _context.TransactionDetails
                .Include(x => x.Item)
                .FirstOrDefault(x => x.ID == ID);

            PopulateDropDownLists(transactionDetail.Item);

            return PartialView("_EditItem", transactionDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem([Bind("ID, TransactionID, ItemID, Quantity, UnitCost, ExtendedCost")] TransactionDetail transDetail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TransactionDetail td = _context.TransactionDetails.FirstOrDefault(x => x.ID == transDetail.ID);

                    // Update fields
                    td.ItemID = transDetail.ItemID;
                    td.Quantity = transDetail.Quantity;

                    // Get unit cost and extended cost
                    var item = _context.Items.FirstOrDefault(x => x.ID == transDetail.ItemID);

                    td.UnitCost = item.Price;
                    td.ExtendedCost = item.Price * transDetail.Quantity;

                    _context.SaveChanges();

                    UpdateTransactionTotal(td.TransactionID);

                    return Json(new { success = true });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            PopulateDropDownLists();
            return View(transDetail);
        }


        public PartialViewResult DeleteItem(int ID)
        {
            // Get the TD to delete
            TransactionDetail td = _context.TransactionDetails
                .Include(x => x.Item)
                .FirstOrDefault(x => x.ID == ID);

            return PartialView("_DeleteItem", td);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ID)
        {
            var td = _context.TransactionDetails
                .Include(x => x.Item)
                .FirstOrDefault(x => x.ID == ID);

            int transID = td.TransactionID;

            if (td != null)
            {
                _context.TransactionDetails.Remove(td);
                await _context.SaveChangesAsync();

                UpdateTransactionTotal(transID);

                return Json(new { success = true });
            }

            return View(td);
        }


        private void UpdateTransactionTotal(int id)
        {
            Transaction t = _context.Transactions
                .Include(x => x.TransactionDetails)
                .FirstOrDefault(x => x.ID == id);

            if(t != null)
            {
                t.TransactionTotal = 0;

                foreach(var td in t.TransactionDetails)
                {
                    t.TransactionTotal += td.ExtendedCost;
                }

                _context.SaveChanges();
            }
        }

        private void PopulateDropDownLists(Item item = null)
        {
            ViewBag.ItemID = new SelectList(_context.Items, "ID", "ItemName", item?.ID);
        }

        private bool TransactionDetailExists(int id)
        {
            return _context.TransactionDetails.Any(e => e.ID == id);
        }
    }
}
