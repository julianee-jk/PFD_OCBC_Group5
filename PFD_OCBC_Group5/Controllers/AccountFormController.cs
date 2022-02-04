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

        public ActionResult SubmitOrSave()
        {
            AccountFormModel account = TempData.Get<AccountFormModel>("firstUserAcc");
            account.DOB = DateTime.Now;
            return View(account);
        }

        [HttpPost]
        public ActionResult SubmitOrSave(AccountFormModel account, int ID)
        {
            if (ID==1)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("AccountHolder");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

                var list = new List<AccountFormModel>();
                var flag = false;

                bool isNumber1 = true, isNumber2 = true, isNumber3 = true, isNumber4 = true;
                int numericValue;
                if (account.PostalCode != null)
                {
                    isNumber1 = int.TryParse(account.PostalCode, out numericValue);
                }
                if (account.MobileNumber != null)
                {
                    isNumber2 = int.TryParse(account.MobileNumber, out numericValue);
                }
                if (account.HomeNumber != null)
                {
                    isNumber3 = int.TryParse(account.HomeNumber, out numericValue);
                }
                if (account.MailingPostalCode != null)
                {
                    isNumber4 = int.TryParse(account.MailingPostalCode, out numericValue);
                }
                if (!isNumber1 || !isNumber2 || !isNumber3 || !isNumber4)
                {
                    if (!isNumber1)
                    {
                        TempData["ValidationPostalCode"] = "Please input a number";
                    }
                    if (!isNumber2)
                    {
                        TempData["ValidationMobileNumber"] = "Please input a number";
                    }
                    if (!isNumber3)
                    {
                        TempData["ValidationHomeNumber"] = "Please input a number";
                    }
                    if (!isNumber4)
                    {
                        TempData["ValidationMailingPostalCode"] = "Please input a number";
                    }
                    return View(account);
                }

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
                            flag = true;
                            account.UniqueID = x.UniqueID;
                            account.AccountID = x.AccountID;
                            account.AccountCreated = x.AccountCreated;
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
            else
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

                bool isNumber1 = true, isNumber2 = true, isNumber3 = true, isNumber4 = true;
                int numericValue;
                if (account.PostalCode != null)
                {
                    isNumber1 = int.TryParse(account.PostalCode, out numericValue);
                }
                if (account.MobileNumber != null)
                {
                    isNumber2 = int.TryParse(account.MobileNumber, out numericValue);
                }
                if (account.HomeNumber != null)
                {
                    isNumber3 = int.TryParse(account.HomeNumber, out numericValue);
                }
                if (account.MailingPostalCode != null)
                {
                    isNumber4 = int.TryParse(account.MailingPostalCode, out numericValue);
                }
                if (!isNumber1 || !isNumber2 || !isNumber3 || !isNumber4)
                {
                    if (!isNumber1)
                    {
                        TempData["ValidationPostalCode"] = "Please input a number";
                    }
                    if (!isNumber2)
                    {
                        TempData["ValidationMobileNumber"] = "Please input a number";
                    }
                    if (!isNumber3)
                    {
                        TempData["ValidationHomeNumber"] = "Please input a number";
                    }
                    if (!isNumber4)
                    {
                        TempData["ValidationMailingPostalCode"] = "Please input a number";
                    }
                    return View(account);
                }

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
                            account.AccountCreated = x.AccountCreated;
                            break;
                        }
                    }
                }

                if (!flag)
                {
                    if (accexist)
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
        }
    }
}
