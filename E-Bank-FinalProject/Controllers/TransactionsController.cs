using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Bank_FinalProject.Data;
using E_Bank_FinalProject.Models;
using System.Security.Claims;

namespace E_Bank_FinalProject.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly DataContext _context;

        public TransactionsController(DataContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {

            string email = User.FindFirstValue(ClaimTypes.Email);
            User user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);

            var dataContext = _context.Transaction.Include(a => a.Account)
              .Where(u => u.Account.UserID == user.UserID);

            return View(await dataContext.ToListAsync());
        }

        public IActionResult AddTransaction(int id)
        {
            var selectListItems = _context.Account.Where(a => a.AccountID != id);
            ViewData["AccountID"] = new SelectList(selectListItems, "AccountID", "AccountName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTransaction(int id, [Bind("TransactionID,Amount,AccountID")] Transaction transaction)
        {
            Account acc1, acc2;
            try
            {
                acc1 = await _context.Account.FirstOrDefaultAsync(a => a.AccountID == id);
                acc2 = await _context.Account.FirstOrDefaultAsync(a => a.AccountID == transaction.AccountID);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Invalid transaction");
                var selectListItems1 = _context.Account.Where(a => a.AccountID != id);
                ViewData["AccountID"] = new SelectList(selectListItems1, "AccountID", "AccountName");
                return View(transaction);

            }
            if (acc1.Balance + acc1.Limit >= transaction.Amount && transaction.Amount > 0)
            {
                transaction.Account = acc1;
                transaction.TransactionDate = DateTime.Today;
                transaction.TransactionType = "Outcome";
                _context.Add(transaction);
                acc1.Balance -= transaction.Amount;
                acc2.Balance += transaction.Amount;
                Transaction t = new Transaction();
                t.Account = acc2;
                t.AccountID = acc2.AccountID;
                t.Amount = -transaction.Amount;
                t.TransactionType = "Income";
                t.TransactionDate = DateTime.Today;
                _context.Add(t);

                _context.Update(acc1);
                _context.Update(acc2);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid transaction");
                var selectListItems = _context.Account.Where(a => a.AccountID != id);
                ViewData["AccountID"] = new SelectList(selectListItems, "AccountID", "AccountName");
                return View(transaction);
            }

        }

    }
}
