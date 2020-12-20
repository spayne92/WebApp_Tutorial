using Microsoft.AspNetCore.Mvc;
using System;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            // Testing new developer exception page.
            throw new InvalidOperationException();
            //return View();
        }
    }
}
