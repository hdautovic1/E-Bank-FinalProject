using E_Bank_FinalProject.Data;
using E_Bank_FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Bank_FinalProject.Controllers
{
    public class AccountsController : Controller
    {
        private readonly DataContext _context;

        public AccountsController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Account.Include(a => a.User)
                .Where(u => u.User.FirstName == User.Identity.Name);

            return View(await dataContext.ToListAsync());
        }
        public async Task<IActionResult> AddAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<List<object>> getData()
        {
            List<object> data = new List<object>();
            List<string> labels = new List<string>();

            var dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            for (; dt <= DateTime.Today; dt = dt.AddDays(1))
            {
                labels.Add(dt.ToString());
            }
            data.Add(labels);

            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            double available = 0, income = 0, outcome = 0, start = 0;
            var accounts = _context.Account.Where(a => a.UserID == user.UserID);
            foreach (var item in accounts)
            {
                available += item.Limit + item.Balance;
                start += item.Balance;
            }

            var transactions = _context.Transaction.Include(a => a.Account)
            .Where(u => u.Account.UserID == user.UserID);

            foreach (var item in transactions)
            {
                if (item.TransactionType.Equals("Income"))
                {
                    income += item.Amount;
                }
                else if (item.TransactionType.Equals("Outcome"))

                {
                    outcome += item.Amount;
                }
            }
            return data;
        }
        public async Task<IActionResult> DashBoard()

        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            double available = 0, income = 0, outcome = 0, start = 0;
            var accounts = _context.Account.Where(a => a.UserID == user.UserID);
            foreach (var item in accounts)
            {
                available += item.Limit + item.Balance;
                start += item.Balance;
            }

            var transactions = _context.Transaction.Include(a => a.Account)
            .Where(u => u.Account.UserID == user.UserID);

            foreach (var item in transactions)
            {
                if (item.TransactionType.Equals("Income"))
                {
                    income += item.Amount;
                }
                else if (item.TransactionType.Equals("Outcome"))

                {
                    outcome += item.Amount;
                }
            }
            ViewBag.Available = available;
            ViewBag.Income = income;
            ViewBag.Outcome = outcome;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAccount([Bind("AccountName,AccountNumber,AccountDescription")] Account account)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            User user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);

            account.User = user;
            account.UserID = user.UserID;
            account.DateCreated = DateTime.Today;
            account.Balance = 0;
            account.Limit = 1000;
            _context.Add(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AccountID == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Account == null)
            {
                return Problem("Entity set 'DataContext.Account'  is null.");
            }
            var account = await _context.Account.FindAsync(id);
            if (account != null)
            {
                _context.Account.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
