using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PFD_OCBC_Group5.Models;
using PFD_OCBC_Group5.DAL;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PFD_OCBC_Group5.Controllers
{
    public class AccountFormController : Controller
    {
        private AccountDAL AccountContext = new AccountDAL();

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult PersonInfo()
        {
            return View();

        }

        [HttpPost]
        public ActionResult PersonInfo(string nric)
        {



            if (AccountContext.AccountExists(nric))
            {
                AccountFormModel account = AccountContext.GetApplicantInfo(nric);

                return View(account);
            }
            else
            {
                AccountFormModel account = new AccountFormModel();
                account.NRIC = nric;
                account.DOB = DateTime.Now;
                return View(account);
            }
        }

        [HttpPost]
        public ActionResult Saveme(AccountFormModel account)
        {
            bool flag = false;

            string[] temp = new string[] { };

            temp.Append(account.Occupation);
            temp.Append(account.PR);
            temp.Append(account.Gender);
            temp.Append(account.SelfEmployed);
            temp.Append(account.HomeAddress);
            temp.Append(account.PostalCode);
            temp.Append(account.Email);
            temp.Append(account.MobileNumber);

            foreach (string x in temp)
            {
                if (x == null)
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                return RedirectToAction("Validate", "SecondMobile");
            }

            return RedirectToAction("Index", "Home");



        }


        public ActionResult save(string nric)
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult save(AccountFormModel account)
        {

            account.AccountCreated = "N";

            if (AccountContext.AccountExists(account.NRIC))
            {
                AccountContext.Update(account);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                AccountContext.Add(account);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
