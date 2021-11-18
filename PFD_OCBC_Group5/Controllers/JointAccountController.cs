using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFD_OCBC_Group5.Models;
using System.Diagnostics;
using PFD_OCBC_Group5.DAL;

namespace PFD_OCBC_Group5.Controllers
{
    public class JointAccountController : Controller
    {
        // GET: JointAccountController
        // Auto-create account in DAL and retrieve auto-incremented PK as Account Number
        // Retrieve OwnerNRIC, JointNRIC and Relationship through TempData passing
        private AccountDAL AccountContext = new AccountDAL();
        private JointAccountDAL JointAccountContext = new JointAccountDAL();
        public ActionResult Index(JointAccountModel jointAccount)
        {
            return View(jointAccount);
        }

        [HttpPost]
        public ActionResult Index(AccountFormModel account, string testing)
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

                JointAccountView jaView = new JointAccountView();
                jaView.firstName = AccountContext.GetApplicantInfo(ja.OwnerNRIC).Name;
                jaView.secondName = AccountContext.GetApplicantInfo(ja.JointNRIC).Name;

             

                jaView.ja = ja;

                return View(jaView);

            }

            return RedirectToAction("Index", "Home");
        }
    }
}