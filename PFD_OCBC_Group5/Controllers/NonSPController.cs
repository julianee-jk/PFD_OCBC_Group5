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

namespace PFD_OCBC_Group5.Controllers
{
    public class NonSPController : Controller
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

        // GET: NonSPControllercs
        public ActionResult PersonInfo(int currentUser, int accId, string rel)
        {
            // Set the session type of the user SP / NonSP
            HttpContext.Session.SetString("Type", "NonSP");

            if (currentUser == 2)
            {
                // Set the session state to the second user
                HttpContext.Session.SetString("Applicant", "Second");

                // Save the accId to be passed to the joint account controller
                HttpContext.Session.SetInt32("FirstUserAccID", accId);

                // Save the owner's relationship with the second applicant
                HttpContext.Session.SetString("RelationshipWithOwner", rel);
            }

            AccountFormModel account = new AccountFormModel();
            account.DOB = DateTime.Now;
            return View(account);
        }

        public ActionResult SubmitAccountInfo(AccountFormModel account)
        {
            bool flag = false;

            if (account.Occupation == null || account.PR == null || account.Gender == null || account.SelfEmployed == null || account.HomeAddress == null || account.PostalCode == null || account.Email == null || account.MobileNumber == null)
            {
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

                if (HttpContext.Session.GetString("Applicant") == "Second")
                    HttpContext.Session.SetInt32("SecondUserAccID", account.AccountID);

                HttpContext.Session.SetInt32("AccountID", account.AccountID);
                return RedirectToAction("UploadPhoto", "NSPVerification");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult SaveAccountInfo(string nric)
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult SaveAccountInfo(AccountFormModel account)
        {
            client = new FireSharp.FirebaseClient(config);
            //retrieve from accountholder in firebase
            FirebaseResponse response = client.Get("AccountHolder");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<AccountFormModel>();

            //retrieve from singpass user in firebase
            FirebaseResponse response2 = client.Get("SingpassUser");
            dynamic data2 = JsonConvert.DeserializeObject<dynamic>(response2.Body);
            var SingpassHolderList = new List<SingpassModel>();
            var accExistInSP = false;

            var flag = false;

            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<AccountFormModel>(((JProperty)item).Value.ToString()));
                }
            }

            if (data2 != null)
            {
                foreach (var item in data2)
                {
                    SingpassHolderList.Add(JsonConvert.DeserializeObject<SingpassModel>(((JProperty)item).Value.ToString()));
                }
            }

            foreach (var x in SingpassHolderList)
            {
                if (x.NRIC == account.NRIC)
                {
                    accExistInSP = true;
                    break;
                }
                else
                {
                    accExistInSP = false;
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

            if (!accExistInSP)
            {
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
            }
            else
            {
                Debug.WriteLine("NRIC exists in singpass");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
