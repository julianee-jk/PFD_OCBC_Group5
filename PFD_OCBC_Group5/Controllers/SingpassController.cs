using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PFD_OCBC_Group5.Controllers
{
    public class SingpassController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public ActionResult SingpassLogin()
        {
            HttpContext.Session.SetString("Type", "Singpass");
            return View();
        }
    }
}
