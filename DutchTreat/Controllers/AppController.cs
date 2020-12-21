using Microsoft.AspNetCore.Mvc;
using System;

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
            // Set in Controller as opposed to in CSHTML.
            ViewBag.Title = "Contact Us";

            return View();
        }

        // HttpPost maps up with HTML 'method=post' and View name.
        [HttpPost("contact")]
        public IActionResult Contact(object model)
        {
            ViewBag.Title = "Contact Us";

            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About Us";

            return View();
        }
    }
}
