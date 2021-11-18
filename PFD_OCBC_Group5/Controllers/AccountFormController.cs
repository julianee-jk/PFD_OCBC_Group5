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
        private JointAccountDAL JointAccountContext = new JointAccountDAL();

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult PersonInfo()
        {
            AccountFormModel account = new AccountFormModel();
            account.DOB = DateTime.Now;
            return View(account);
        }

        [HttpPost]
        public ActionResult PersonInfo(string nric)
        {
            if (AccountContext.AccountExists(nric))
            {
                AccountFormModel account = AccountContext.GetApplicantInfo(nric);
                if (account.AccountCreated == "Y")
                {
                    return RedirectToAction("SingpassLogin", "Singpass");
                }
                else
                {
                    return View(account);
                }
                
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
                if (AccountContext.AccountExists(account.NRIC))
                {
                    AccountContext.Update(account);
                   
                }
                else
                {
                    account.AccountCreated = "N";
                    AccountContext.Add(account);
                }

                if (HttpContext.Session.GetString("Type") == "Singpass")
                {
                    HttpContext.Session.SetString("FirstNRIC", account.NRIC);
                    return RedirectToAction("Validate", "SecondMobile");
                }
                else
                {
                    HttpContext.Session.SetString("FirstNRIC", account.NRIC);
                    return RedirectToAction("UploadPhoto", "NSPVerification");
                }

                
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Savemesecond(AccountFormModel account, string testing)
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
            temp.Append(testing);

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
                if (AccountContext.AccountExists(account.NRIC))
                {
                    AccountContext.Update(account);
                }
                else
                {
                    //need to change
                    account.AccountCreated = "N";
                    AccountContext.Add(account);    
                }

                JointAccountModel ja = new JointAccountModel();
                ja.OwnerNRIC = HttpContext.Session.GetString("FirstNRIC");
                ja.JointNRIC = account.NRIC;
                ja.RelationshipToOwner = testing;
                ja.AccountNumber = JointAccountContext.Add(ja);
                return RedirectToAction("Index", "JointAccount", new { jointAccount = ja});

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
