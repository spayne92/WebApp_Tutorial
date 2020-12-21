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

            throw new InvalidOperationException("Bad things happen.");

            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About Us";

            return View();
        }
    }
}
