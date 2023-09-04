using AutomixMVC.Models;
using Microsoft.AspNetCore.Mvc;
using AutomixMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AutomixMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            if (ModelState.IsValid)
            {
                var result = await _userService.Authenticate(model.Username ?? "", model.Password ?? "");
                if (result != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, result.Username),
                        new Claim(ClaimTypes.Role, result.Role ?? "DefaultRole")
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    if (returnUrl != null)
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
