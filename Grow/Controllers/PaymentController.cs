using Grow.Data;
using Grow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin, Volunteer")]
    public class PaymentController : Controller
    {
        private readonly GrowContext _context;

        public PaymentController(GrowContext context)
        {
            _context = context;
        }

        // GET: PaymentController
        public IActionResult Index(int id)
        {
            var transaction = _context.Transactions
                .Include(x => x.Payments)
                .ThenInclude(x => x.PaymentMethod)
                .Include(x => x.Household)
                .ThenInclude(x => x.City)
                .Include(x => x.Member)
                .Include(x => x.TransactionDetails)
                .Include(x => x.Volunteer)
                .Where(x => x.ID == id)
                .FirstOrDefault();

            if (transaction == null)
            {
                return RedirectToAction("Index", "Transactions");
            }

            return View(transaction);
        }

        public PartialViewResult AddPayment(int ID)
        {
            // Save trans ID for form
            ViewData["TransactionID"] = ID;

            PopulateDropDownLists();

            return PartialView("_AddPayment");
        }

        // POST: PaymentController/AddPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPayment([Bind("ID, PaymentAmount, PaymentMethodID, TransactionID")] Payment payment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Payments.Add(payment);
                    _context.SaveChanges();

                    UpdateTransactionPaidStatus(payment.TransactionID);

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
            return View(payment);
        }


        public PartialViewResult EditPayment(int ID)
        {
            var payment = _context.Payments
                .FirstOrDefault(x => x.ID == ID);

            return PartialView("_EditPayment", payment);
        }

        // POST: PaymentController/EditPayment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPayment([Bind("ID, PaymentAmount, PaymentMethodID, TransactionID")] Payment payment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Payment p = _context.Payments.FirstOrDefault(x => x.ID == payment.ID);

                    // Update fields
                    p.PaymentAmount = payment.PaymentAmount;
                    p.PaymentMethod = payment.PaymentMethod;

                    _context.SaveChanges();

                    UpdateTransactionPaidStatus(p.TransactionID);

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

            return View(payment);
        }


        public PartialViewResult DeletePayment(int ID)
        {
            // Get the payment to delete
            Payment p = _context.Payments
                .FirstOrDefault(x => x.ID == ID);

            return PartialView("_DeletePayment", p);
        }

        // POST: PaymentController/DeletePayment/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ID)
        {
            var p = _context.Payments
                .FirstOrDefault(x => x.ID == ID);

            int transID = p.TransactionID;

            if (p != null)
            {
                _context.Payments.Remove(p);
                await _context.SaveChangesAsync();

                UpdateTransactionPaidStatus(transID);

                return Json(new { success = true });
            }

            return View(p);
        }


        public PartialViewResult PaymentList(int id)
        {
            var transaction = _context.Transactions
                .Include(x => x.Payments)
                .ThenInclude(x => x.PaymentMethod)
                .Include(x => x.Household)
                .ThenInclude(x => x.City)
                .Include(x => x.Member)
                .Include(x => x.TransactionDetails)
                .Include(x => x.Volunteer)
                .Where(x => x.ID == id)
                .FirstOrDefault();

            return PartialView("_PaymentList", transaction);
        }

        private void UpdateTransactionPaidStatus(int transID)
        {
            Transaction t = _context.Transactions
                .Include(x => x.Payments)
                .FirstOrDefault(x => x.ID == transID);

            if (t != null)
            {
                var paidAmount = t.Payments.Sum(x => x.PaymentAmount);

                if(paidAmount >= t.TransactionTotal)
                {
                    t.Paid = true;
                }
                else
                {
                    t.Paid = false;
                }

                _context.SaveChanges();
            }
        }

        private void PopulateDropDownLists()
        {
            ViewBag.PaymentMethods = new SelectList(_context.PaymentMethods.OrderBy(x => x.Method), "ID", "Method");
        }
    }
}
