using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PFD_OCBC_Group5.Models;
using PFD_OCBC_Group5.DAL;
using PFD_OCBC_Group5.Extensions;
using Firebase.Database;
using Firebase.Database.Query;
using System.Diagnostics;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PFD_OCBC_Group5.Controllers
{
    public class SingpassController : Controller
    {
        private AccountDAL AccountContext = new AccountDAL();
        IFirebaseConfig config = new FirebaseConfig
        {
            BasePath = "https://pfd-group-5-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;

        private void AddSingpassUser(SingpassModel account)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = account;
            PushResponse response = client.Push("SingpassUser/", data);
            data.SingpassID = response.Result.name;
            Debug.WriteLine(data.SingpassID);
            SetResponse setResponse = client.Set("SingpassUser/" + data.SingpassID, data);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SingpassLogin(int currentUser, int accId, string rel)
        {
            if (currentUser == 2) 
            {
                // Set the session state to the second user
                HttpContext.Session.SetString("Applicant", "Second");

                // Save the accId to be passed to the joint account controller
                HttpContext.Session.SetInt32("FirstUserAccID", accId);

                // Save the owner's relationship with the second applicant
                HttpContext.Session.SetString("RelationshipWithOwner", rel);
            }

            var accountHolderList = new List<AccountFormModel>();
            var singpassUserlist = new List<SingpassModel>();

            DateTime test;
            test = new DateTime(1990, 11, 17);
            var firebaseClient = new FirebaseClient("https://pfd-group-5-default-rtdb.firebaseio.com/");

            /*
            var singpassUser = new SingpassModel() { NRIC = "S2929292H", Password = "Password1.", DOB = test, Email = "Rad@gmail.com", Gender = "F", HomeAddress = "SK Road 54", PostalCode = "123123", MobileNumber = "91212351", Name = "Rad", Nationality = "Singaporean" };
            var currentAccountHolder = new AccountFormModel() { AccountID = 1, NRIC = "S1234567C", Name = "Duby" , Salutation = "Miss", Nationality = "Singaporean", DOB = test, Occupation = "Restaurant Owner", PR = "N", Gender = "F", SelfEmployed = "Y", NatureOfBusiness = "Manages a restaurant", HomeAddress = "Clementi Road 54", PostalCode = "583957", MailingAddress = "31 Lifebouy Road" , MailingPostalCode = "561292", Email = "Duby@gmail.com", MobileNumber = "98708394", HomeNumber = "False", AccountCreated = "N" };
            
            var result = await firebaseClient
              .Child("AccountHolder")
              .PostAsync(currentAccountHolder);

            var result2 = await firebaseClient
              .Child("SingpassUser")
              .PostAsync(singpassUser);
            AddStudentToFirebase(singpassUser);
             */

            var singpassAccounts = await firebaseClient
              .Child("SingpassUser")
              .OnceAsync<SingpassModel>();
           
            foreach (var y in singpassAccounts)
            {
                var singpass = y.Object;
                SingpassModel tempSingpass = new SingpassModel();
                tempSingpass.DOB = singpass.DOB;
                tempSingpass.Email = singpass.Email;
                tempSingpass.Gender = singpass.Gender;
                tempSingpass.HomeAddress = singpass.HomeAddress;
                tempSingpass.MobileNumber = singpass.MobileNumber;
                tempSingpass.NRIC = singpass.NRIC;
                tempSingpass.Name = singpass.Name;
                tempSingpass.Nationality = singpass.Nationality;
                tempSingpass.Password = singpass.Password;
                tempSingpass.PostalCode = singpass.PostalCode;

                singpassUserlist.Add(tempSingpass);
            }

            var accountHolders = await firebaseClient
              .Child("AccountHolder")
              .OnceAsync<AccountFormModel>();

            foreach (var x in accountHolders)
            {
                var account = x.Object;
                AccountFormModel temp = new AccountFormModel();

                temp.AccountID = account.AccountID;

                temp.NRIC = account.NRIC == "False" ?  "" : account.NRIC;

                temp.Name = account.Name == "False" ? "" : account.Name;

                temp.Salutation = account.Salutation == "False" ? "" : account.Salutation;

                temp.Nationality = account.Nationality == "False" ? "" : account.Nationality;

                string g = account.DOB.ToString();
                temp.DOB = g == "False" ? DateTime.Parse("") : account.DOB;

                temp.Occupation = account.Occupation == "False" ? "" : account.Occupation;

                temp.PR = account.PR == "False" ? "" : account.PR;

                temp.Gender = account.Gender == "False" ? "" : account.Gender;

                temp.SelfEmployed = account.SelfEmployed == "False" ? "" : account.SelfEmployed;

                temp.NatureOfBusiness = account.NatureOfBusiness == "False" ? "" : account.NatureOfBusiness;

                temp.HomeAddress = account.HomeAddress == "False" ? "" : account.HomeAddress;

                temp.PostalCode = account.PostalCode == "False" ? "" : account.PostalCode;

                temp.MailingAddress = account.MailingAddress == "False" ? "" : account.MailingAddress;

                temp.MailingPostalCode = account.MailingPostalCode == "False" ? "" : account.MailingPostalCode;

                temp.Email = account.Email == "False" ? "" : account.Email;

                temp.MobileNumber = account.MobileNumber == "False" ? "" : account.MobileNumber;

                temp.HomeNumber = account.HomeNumber == "False" ? "" : account.HomeNumber;

                temp.AccountCreated = account.AccountCreated == "False" ? "" : account.AccountCreated;

                accountHolderList.Add(temp);
            }

            TempData.Put("accountHolderList", accountHolderList);
            TempData.Put("singpassUserList", singpassUserlist);

            HttpContext.Session.SetString("Type", "Singpass");
            return View();
        }

        public ActionResult SingpassRegister()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SingpassRegister(ValidateConfirmPassword account)
        {

            if (account.cfmPassword == account.accountInformation.Password)
            {

                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("SingpassUser");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var singpassAccountList = new List<AccountFormModel>();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        singpassAccountList.Add(JsonConvert.DeserializeObject<AccountFormModel>(((JProperty)item).Value.ToString()));
                    }
                }

                var exists = false;
                foreach(var dbAccount in singpassAccountList)
                {
                    if(dbAccount.NRIC == account.accountInformation.NRIC)
                    {
                        exists = true;
                        break;
                    }
                }

                if(!exists)
                {
                    //insert into firebase
                    Debug.WriteLine("Inserted into firebase");


                    AddSingpassUser(account.accountInformation);


                }
                else
                {
                    Debug.WriteLine("Singpass account already exist");



                }
                


                return RedirectToAction("Index", "Home");
            }
            else
            {
                //passwords were not the same
                return View(account);
            }

            
        }


        [HttpPost]
        public ActionResult SingpassLogin(string nric, string password)
        {

            List<AccountFormModel> accountHolderList = TempData.Get<List<AccountFormModel>>("accountHolderList");
            List<SingpassModel> singpassUserList = TempData.Get<List<SingpassModel>>("singpassUserlist");

            SingpassModel singpassInfo = new SingpassModel();

            var userExists = false;
            var passwordCorrect = false;

            foreach(var singpassUser in singpassUserList)
            {
                if(singpassUser.NRIC == nric)
                {
                    //checking if user exists
                    userExists = true;
                    
                    Debug.WriteLine(singpassUser.Password);
                    if (singpassUser.Password == password)
                    {
                        singpassInfo = singpassUser;
                        //checking if password is correct
                        passwordCorrect = true;
                    }
                    break;
                }
            }

            var existingForm = false;
            if (passwordCorrect)
            {
                AccountFormModel account = new AccountFormModel();
                
                foreach (var accounts in accountHolderList)
                {
                    if (accounts.NRIC == nric)
                    {
                        if(accounts.AccountCreated == "N")
                        {
                            account = accounts;
                            //checking if the nric has an existing form that was saved.
                            existingForm = true;
                            break;
                        }
                    }
                }

                if(existingForm)
                {
                    TempData.Put("firstUserAcc", account);

                    // Redirect to form if Singpass account exists
                    return RedirectToAction("PersonInfo", "AccountForm");
                }
                else
                {
                    AccountFormModel newAccount = new AccountFormModel();
                    newAccount.DOB = singpassInfo.DOB;
                    newAccount.Email = singpassInfo.Email;
                    newAccount.Gender = singpassInfo.Gender;
                    newAccount.HomeAddress = singpassInfo.HomeAddress;
                    newAccount.MobileNumber = singpassInfo.MobileNumber;
                    newAccount.NRIC = singpassInfo.NRIC;
                    newAccount.Name = singpassInfo.Name;
                    newAccount.Nationality = singpassInfo.Nationality;
                    newAccount.PostalCode = singpassInfo.PostalCode;

                    TempData.Put("firstUserAcc", newAccount);
                    return RedirectToAction("PersonInfo", "AccountForm");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            

        }





    }
}
