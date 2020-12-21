using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Changes routing URL to be from 'root' instead of '/app/'.
        [HttpGet("contact")] 
        public IActionResult Contact()
        {
            return View();
        }

        // HttpPost maps up with HTML 'method=post' and View name.
        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            return View();
        }

        public IActionResult About()
        {
            // Set in Controller as opposed to in CSHTML.
            ViewBag.Title = "About Us";

            return View();
        }
    }
}
