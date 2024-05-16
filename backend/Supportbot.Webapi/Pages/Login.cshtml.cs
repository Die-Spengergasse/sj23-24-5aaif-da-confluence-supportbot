using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Supportbot.Application.Infrastructure;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Supportbot.Webapi.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SupportbotContext _db;

        public LoginModel(SupportbotContext db)
        {
            _db = db;
        }

        public List<string> Users => _db.Employees
            .Select(e => e.Username)
            .OrderBy(u => u)
            .ToList();
        [BindProperty]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Ung�ltiger Username")]
        public string Username { get; set; } = default!;
        [BindProperty]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Ung�ltiges Passwort")]
        public string Password { get; set; } = default!;
        [FromQuery]
        public string? ReturnUrl { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();
            var user = await _db.Employees.FirstOrDefaultAsync(e => e.Username == Username);
            if (user is null)
            {
                ModelState.AddModelError("", "Invalid username.");
                return Page();
            }
            var claims = new List<Claim>
            {
                new Claim("Guid", user.Guid.ToString()),
                new Claim(ClaimTypes.Name, Username),
                new Claim(ClaimTypes.Role, "admin")
            };
            var claimsIdentity = new ClaimsIdentity(
                claims,
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3),
            };

            await HttpContext.SignInAsync(
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return Redirect(string.IsNullOrEmpty(ReturnUrl) ? "/" : ReturnUrl);
        }
    }
}
