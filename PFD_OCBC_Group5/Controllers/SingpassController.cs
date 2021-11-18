using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PFD_OCBC_Group5.Models;
using PFD_OCBC_Group5.DAL;

namespace PFD_OCBC_Group5.Controllers
{
    public class SingpassController : Controller
    {
        private AccountDAL AccountContext = new AccountDAL();

        public IActionResult Index()
        {
            return View();
        }


        public ActionResult SingpassLogin()
        {
            HttpContext.Session.SetString("Type", "Singpass");
            return View();
        }
        [HttpPost]
        public ActionResult SingpassLogin(string nric)
        {
            if (AccountContext.AccountExists(nric))
            {
                AccountFormModel account = AccountContext.GetApplicantInfo(nric);

                return View(account);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
