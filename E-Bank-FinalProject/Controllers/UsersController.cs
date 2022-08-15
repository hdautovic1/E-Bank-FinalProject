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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace E_Bank_FinalProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }       

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserID,UserName,FirstName,LastName,Email,Password,ConfirmedPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                UserRoles userRoles = new UserRoles();
                user.Password=PasswordManager.Encode(user.Password);
                user.ConfirmedPassword = PasswordManager.Encode(user.ConfirmedPassword);
                userRoles.User = user;
                var role = _context.Role.Where(r =>
                    r.Name == "User"
                );
                userRoles.Role = role.First();
                _context.Add(user);
                _context.UserRoles.Add(userRoles);
                await _context.SaveChangesAsync();
                return View("login");
            }
            return View(user);
        }


        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();
        }
        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(string email, string password, string returnUrl)
        {

            ViewData["ReturnUrl"] = returnUrl;
            var u = _context.User.Where(u => email == u.Email && PasswordManager.Encode(password) == u.Password).First();


            if (u == null)
            {
                return View("login");
            }
            var r = _context.UserRoles.Where(ur => ur.User == u).Include(ur => ur.Role).First().Role;

            var claims = new List<Claim>()
            {             
                new Claim(ClaimTypes.Name, u.FirstName),                
                new Claim(ClaimTypes.Surname, u.LastName),
                new Claim(ClaimTypes.Email, u.Email),
                new Claim(ClaimTypes.Role, r.Name),
                new Claim("username", u.UserName),
          

        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);

            if (returnUrl == null) return RedirectToAction("Index", "Home");

            return Redirect(returnUrl);

        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> MyProfile()
        {
            string email = this.User.FindFirstValue(ClaimTypes.Email);
            User user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet("ChangePassword")]
        public async Task<IActionResult> ChangePassword()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            User user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) { 
                return NotFound();
            }
            return View(user);
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(string oldPassword,string newPassword,string confirmedPassword)
        {
                string email = User.FindFirstValue(ClaimTypes.Email);
                User u2 = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
                if (u2.Password != PasswordManager.Encode(oldPassword) || newPassword!=confirmedPassword){
                    return RedirectToAction(nameof(ChangePassword));
                }
                u2.Password = PasswordManager.Encode(newPassword);
                u2.ConfirmedPassword = PasswordManager.Encode(confirmedPassword);
                _context.Update(u2);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyProfile));
        }




        /*
        
        public async Task<IActionResult> Index()
        {
              return _context.User != null ? 
                          View(await _context.User.ToListAsync()) :
                          Problem("Entity set 'DataContext.User'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
      
        // GET: Users/Edit/5
       
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'DataContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.UserID == id)).GetValueOrDefault();
        }
        */


    }
}
