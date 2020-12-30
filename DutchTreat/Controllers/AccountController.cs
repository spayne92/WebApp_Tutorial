using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<StoreUser> _signInManager;

        public AccountController(ILogger<AccountController> logger, SignInManager<StoreUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            // Checks if user is already logged in.
            if (this.User.Identity.IsAuthenticated)
            {
                // Redirects to home page if already logged in.
                return RedirectToAction("Index", "App");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Allows logging in without retreiving User entity.  
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, 
                    model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        Redirect(Request.Query["ReturnUrl"].First());
                    }
                    return RedirectToAction("Shop", "App");
                }
            }

            // First parameter blank, because issue with entire model, not specific prop.
            ModelState.AddModelError("", "Failed to login.");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }
    }
}
