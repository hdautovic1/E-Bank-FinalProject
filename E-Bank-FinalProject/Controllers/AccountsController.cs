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
    public class AccountsController : Controller
    {
        private readonly DataContext _context;

        public AccountsController(DataContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Account.Include(a => a.User)
                .Where(u => u.User.FirstName == User.Identity.Name);
      
            return View(await dataContext.ToListAsync());
        }

        

        // GET: Accounts/Create
        public async Task<IActionResult> Create()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            User user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);

            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", user.UserID);
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountID,AccountName,DateCreated,AccountDescription,UserID")] Account account)
        {
           
            var u = _context.User.Where(u => account.UserID == u.UserID).First();
            account.User = u;
            _context.Add(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", account.UserID);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountID,AccountName,DateCreated,AccountDescription,UserID")] Account account)
        {
            if (id != account.AccountID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountID))
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
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", account.UserID);
            return View(account);
        }

        // GET: Accounts/Delete/5
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

        // POST: Accounts/Delete/5
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

        private bool AccountExists(int id)
        {
          return (_context.Account?.Any(e => e.AccountID == id)).GetValueOrDefault();
        }
    }
}
