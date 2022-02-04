using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PFD_OCBC_Group5.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Firebase.Database;
using Firebase.Database.Query;

namespace PFD_OCBC_Group5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        { 
            HttpContext.Session.SetString("Status", "New");
            HttpContext.Session.SetString("Applicant", "First");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult Continue()
        {
            HttpContext.Session.SetString("Type", "NonSP");
            HttpContext.Session.SetString("Status", "Continue");
            return RedirectToAction("SendContinueEmail", "NonSP");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
