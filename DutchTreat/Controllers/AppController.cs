using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private IMailService _mailService;

        public AppController(IMailService mailService)
        {
            _mailService = mailService;
        }

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
                // Show the erros.
                // Shouldn't be needed if Pages section worked, but it doesn't.
            }

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
