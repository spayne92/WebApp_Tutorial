using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private object _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
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
    }
}
