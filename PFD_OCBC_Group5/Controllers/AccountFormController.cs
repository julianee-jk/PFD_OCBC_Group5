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
        public ActionResult PersonInfo(AccountFormModel account)
        {
            account.AccountCreated = "N";

            if (AccountContext.AccountExists(account.NRIC))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                AccountContext.Add(account);
                return RedirectToAction("Home/Index");
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

            foreach(string x in temp)
            {
                if(x == null)
                {
                    flag = true;
                    break;
                }
            }

            if(flag)
            {

            }

            return RedirectToAction("Index");
           
        }
    }
}
