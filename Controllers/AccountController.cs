using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Clinic.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Clinic.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Clinic.Controllers
{
    public class AccountController : Controller
    {
        private ClinicDbContext _context;
        public AccountController(ClinicDbContext context)
        {
            _context = context;
        }
        [HttpGet]
    [AnonymousOnly]

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
    [AnonymousOnly]

        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Проверяем, существует ли уже пользователь с такой почтой
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    // Если пользователь существует, добавляем ошибку в ModelState и возвращаем представление снова
                    ModelState.AddModelError("", "Пользователь с таким номером телефона уже существует");
                    return View(model);
                }

                // Если пользователя с такой почтой не существует, продолжаем процесс регистрации
                int maxId = await _context.Users.MaxAsync(u => (int?)u.Id) ?? 0;
                User user = new User 
                { 
                    Id = maxId + 1,
                    Email = model.Email, 
                    Password = model.Password 
                };

                Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                if (userRole != null)
                    user.Role = userRole;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await Authenticate(user);

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }


        [HttpGet]
    [AnonymousOnly]

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
    [AnonymousOnly]

        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user); // аутентификация
 
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные телефон и(или) пароль");
            }
            return View(model);
        }
        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Вызываем метод SignOutAsync для удаления аутентификационных куки пользователя
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Перенаправляем пользователя на страницу входа
            return RedirectToAction("Login", "Account");
        }
    }
}