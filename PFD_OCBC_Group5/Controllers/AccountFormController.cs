using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PFD_OCBC_Group5.Models;
using PFD_OCBC_Group5.DAL;
using System.Diagnostics;
using PFD_OCBC_Group5.Extensions;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PFD_OCBC_Group5.Controllers
{
    public class AccountFormController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            BasePath = "https://pfd-group-5-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;

        private AccountDAL AccountContext = new AccountDAL();
        private JointAccountDAL JointAccountContext = new JointAccountDAL();

        private void AddStudentToFirebase(AccountFormModel account)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = account;
            PushResponse response = client.Push("AccountHolder/", data);
            data.UniqueID = response.Result.name;
            Debug.WriteLine(data.UniqueID);
            SetResponse setResponse = client.Set("AccountHolder/" + data.UniqueID, data);
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult PersonInfo()
        {
            AccountFormModel account = TempData.Get<AccountFormModel>("firstUserAcc");
            account.DOB = DateTime.Now;
            return View(account);
        }

        [HttpPost]
        public ActionResult Saveme(AccountFormModel account)
        {
            bool flag = false;

            if (account.Occupation == null || account.PR == null || account.Gender == null || account.SelfEmployed == null || account.HomeAddress == null || account.PostalCode == null || account.Email == null || account.MobileNumber == null)
            {
                Debug.WriteLine("null");
                flag = true;
            }   

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("AccountHolder");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            
            var list = new List<AccountFormModel>();
            var accexist = false;
            
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<AccountFormModel>(((JProperty)item).Value.ToString()));
                }
            }

            foreach (var x in list)
            {
                if (x.NRIC == account.NRIC)
                {
                    if (x.AccountCreated == "N")
                    {
                        accexist = true;
                        account.UniqueID = x.UniqueID;
                        account.AccountID = x.AccountID;
                        break;
                    }
                }
            }

            if (!flag)
            {
                if(accexist)
                {
                    client = new FireSharp.FirebaseClient(config);
                    SetResponse setResponse = client.Set("AccountHolder/" + account.UniqueID, account);
                }
                else
                {
                    account.AccountID = list.Count + 1;
                    account.AccountCreated = "N";
                    AddStudentToFirebase(account);
                }

                /*
                if (AccountContext.AccountExists(account.NRIC))
                {
                    //update account
                    account.AccountCreated = "Y";
                    AccountContext.Update(account);
                }
                else
                {
                    //create new account
                    account.AccountCreated = "N";
                    AccountContext.Add(account);
                }*/

                if (HttpContext.Session.GetString("Type") == "Singpass" && HttpContext.Session.GetString("Applicant") == "First")
                {
                    HttpContext.Session.SetInt32("AccountID", account.AccountID);
                    return RedirectToAction("Validate", "SecondEmail");
                }
                else if (HttpContext.Session.GetString("Type") == "Singpass" && HttpContext.Session.GetString("Applicant") == "Second")
                {
                    HttpContext.Session.SetInt32("SecondUserAccID", account.AccountID);
                    return RedirectToAction("Index", "JointAccount");
                }
                else
                {
                    HttpContext.Session.SetInt32("AccountID", account.AccountID);
                    return RedirectToAction("UploadPhoto", "NSPVerification");
                }
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
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("AccountHolder");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            
            var list = new List<AccountFormModel>();
            var flag = false;

            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<AccountFormModel>(((JProperty)item).Value.ToString()));
                }
            }
            
            foreach (var x in list)
            {
                if (x.NRIC == account.NRIC)
                {
                    if(x.AccountCreated == "N")
                    {
                        flag = true;
                        account.UniqueID = x.UniqueID;
                        account.AccountID = x.AccountID;
                        break;
                    }
                }
            }
            if (flag)
            {
                client = new FireSharp.FirebaseClient(config);
                SetResponse setResponse = client.Set("AccountHolder/" + account.UniqueID, account);
            }
            else
            {
                account.AccountID = list.Count + 1;
                account.AccountCreated = "N";
                AddStudentToFirebase(account);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
