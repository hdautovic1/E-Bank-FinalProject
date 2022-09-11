using E_Bank_FinalProject.Data;
using E_Bank_FinalProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string Email)
        {
            var user = await _context.User.Where(u => u.Email == Email).FirstOrDefaultAsync();
            if (user == null) return Json(true);
            return Json($"Email {Email} is already in use!");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserID,UserName,FirstName,LastName,Email,Password,ConfirmedPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                UserRoles userRoles = new UserRoles();
                user.Password = PasswordManager.Encode(user.Password);
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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User u = new User();
                Role r = new Role();
                try
                {
                    u = _context.User.Where(u => model.Email == u.Email && PasswordManager.Encode(model.Password) == u.Password).First();
                    r = _context.UserRoles.Where(ur => ur.User == u).Include(ur => ur.Role).First().Role;
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                    return View(model);
                }

                if (u == null || r == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
                else
                {
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

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> MyProfile()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            User user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            return View(user);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet("ChangePassword")]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                string email = User.FindFirstValue(ClaimTypes.Email);
                User user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    RedirectToAction("Login");
                }
                if (PasswordManager.Encode(model.CurrentPassword) != user.Password)
                {
                    ModelState.AddModelError(string.Empty, "Wrong password!");
                    return View(model);
                }
                user.Password = PasswordManager.Encode(model.NewPassword);
                user.ConfirmedPassword = PasswordManager.Encode(model.ConfirmNewPassword);
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyProfile");
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return _context.User != null ?
                        View(await _context.User.ToListAsync()) :
                        Problem("Entity set 'DataContext.User'  is null.");
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'DataContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            string email = this.User.FindFirstValue(ClaimTypes.Email);
            var u2 = await _context.User.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            if (user == u2)
            {
                await HttpContext.SignOutAsync();
                return Redirect("/");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
