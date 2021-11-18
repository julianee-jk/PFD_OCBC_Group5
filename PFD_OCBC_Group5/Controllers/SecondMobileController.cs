using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFD_OCBC_Group5.Models;

namespace PFD_OCBC_Group5.Controllers
{
    public class SecondMobileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Validate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Validate(SecondMobile secondmobile)
        {
            if (ModelState.IsValid)
            {
                if (secondmobile.MobileNo == secondmobile.ConfirmMobileNo)
                {
                    // Send SMS here (TO:DO)
                    //Redirect user to Awaiting/Index page
                    return RedirectToAction("Index", "Awaiting");
                }
                else
                {
                    // Send error message not the same confirm

                    return View(secondmobile);
                }
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(secondmobile);
            }
        }
    }
}
