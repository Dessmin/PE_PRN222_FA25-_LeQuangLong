using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsLendDB.BLL.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SportsLendDB_LeQuangLong.Pages.Authentication
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            return CheckLogin();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var loginCheck = CheckLogin();
            if (loginCheck is RedirectToPageResult)
            {
                return loginCheck;
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _authService.LoginAsync(Email, Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or Password!");
                TempData["Message"] = "Invalid Email or Password!";

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync
                (CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            Response.Cookies.Append("UserName", user.FullName);

            // redirect to Equipment list per requirements
            return RedirectToPage("/EquipmentPage/Index");
        }

        private IActionResult CheckLogin()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToPage("/EquipmentPage/Index");
            }
            return Page();
        }
    }
}
