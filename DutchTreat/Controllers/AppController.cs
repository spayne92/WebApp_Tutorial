using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly DutchContext _context;

        public AppController(IMailService mailService, DutchContext context)
        {
            _mailService = mailService;
            _context = context;
        }

        public IActionResult Index()
        {
            // Set in Controller as opposed to in CSHTML.
            ViewBag.Title = "Home";

            // Testing database seeding by querying Products table.
            _context.Products.ToList();

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
            // Checks the ViewModel defined validation attributes. 
            if (ModelState.IsValid)
            {
                // Send the email.
                _mailService.SendMessage("spayne@mst.edu", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");
                
                // Communicate back to view/form. 
                ViewBag.UserMessage = "Mail Sent";

                // Clear model, so form is cleared on refresh.
                ModelState.Clear();
            }
            else
            {
                // Show the errors.
                // Shouldn't be needed if Pages section worked, but it doesn't.
            }

            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About Us";

            return View();
        }

        public IActionResult Shop()
        {
            ViewBag.Title = "Shop";

            var result = _context.Products
                .OrderBy(p => p.Category)
                .ToList();

            return View(result);
        }
    }
}
