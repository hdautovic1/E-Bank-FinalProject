using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Bank_FinalProject.Data;
using E_Bank_FinalProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace E_Bank_FinalProject.Controllers
{
    public class CreditCardsController : Controller
    {
        private readonly DataContext _context;

        public CreditCardsController(DataContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.CreditCard.Include(c => c.Account).Include(c=>c.Account)
                .Where(u => u.Account.User.FirstName == User.Identity.Name); ;
            return View(await dataContext.ToListAsync());
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult AddCreditCard(int? id)
        {
            if (id == null) {
                var selectListItems = _context.Account.Where(a => a.User.FirstName == User.Identity.Name);
                ViewData["AccountID"] = new SelectList(selectListItems, "AccountID", "AccountName", id);
            }
            else
            {
                var selectListItems = _context.Account.Where(a => a.User.FirstName == User.Identity.Name)   
                                                      .Where(a=> a.AccountID==id);
                ViewData["AccountID"] = new SelectList(selectListItems, "AccountID", "AccountName", id);

            }
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCreditCard([Bind("CreditCardID,CreditCardName,CreditCardNumber,AccountID")] CreditCard creditCard)
        {
            var account = _context.Account.Where(a=>a.AccountID==creditCard.AccountID).FirstOrDefault();
            creditCard.Account = account;
        
                _context.Add(creditCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));           
           
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CreditCard == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCard
                .Include(c => c.Account)
                .FirstOrDefaultAsync(m => m.CreditCardID == id);
            if (creditCard == null)
            {
                return NotFound();
            }

            return View(creditCard);
        }


        [Authorize(Roles = "Admin,User")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CreditCard == null)
            {
                return Problem("Entity set 'DataContext.CreditCard'  is null.");
            }
            var creditCard = await _context.CreditCard.FindAsync(id);
            if (creditCard != null)
            {
                _context.CreditCard.Remove(creditCard);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreditCardExists(int id)
        {
          return (_context.CreditCard?.Any(e => e.CreditCardID == id)).GetValueOrDefault();
        }
    }
}
