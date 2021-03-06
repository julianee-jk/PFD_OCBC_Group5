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
using System.Net.Mail;
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
        private static Random random = new Random();

        IFirebaseConfig config = new FirebaseConfig
        {
            BasePath = "https://pfd-group-5-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;

        private AccountDAL AccountContext = new AccountDAL();

        private void AddAccountHolderToFirebase(AccountFormModel account)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = account;
            PushResponse response = client.Push("AccountHolder/", data);
            data.UniqueID = response.Result.name;
            Debug.WriteLine(data.UniqueID);
            SetResponse setResponse = client.Set("AccountHolder/" + data.UniqueID, data);
        }

        // GET: NonSPControllercs
        public ActionResult SubmitAccountInfo(int currentUser, int accId, string rel)
        {
            // Set the session type of the user SP / NonSP
            HttpContext.Session.SetString("Type", "NonSP");
            int accID = 0;
            if (TempData["accId"] != null)
            {
                accID = (int)TempData["accId"];
            }

            if (currentUser == 2)
            {
                // Set the session state to the second user
                HttpContext.Session.SetString("Applicant", "Second");

                // Save the accId to be passed to the joint account controller
                HttpContext.Session.SetInt32("FirstUserAccID", accId);

                // Save the owner's relationship with the second applicant
                HttpContext.Session.SetString("RelationshipWithOwner", rel);

                AccountFormModel account = new AccountFormModel();
                account.DOB = DateTime.Now;
                return View(account);
            }
            else if (HttpContext.Session.GetString("Status") == "Continue")
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("AccountHolder");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

                var list = new List<AccountFormModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<AccountFormModel>(((JProperty)item).Value.ToString()));
                    }
                }
                AccountFormModel tempAcc = new AccountFormModel();
                foreach (var x in list)
                {
                    if (x.AccountID == accID)
                    {
                        tempAcc.AccountID = x.AccountID;
                        tempAcc.AccountCreated = x.AccountCreated;
                        tempAcc.DOB = x.DOB;
                        tempAcc.Email = x.Email;
                        tempAcc.Gender = x.Gender;
                        tempAcc.HomeAddress = x.HomeAddress;
                        tempAcc.HomeNumber = x.HomeNumber;
                        tempAcc.MailingAddress = x.MailingAddress;
                        tempAcc.MailingPostalCode = x.MailingPostalCode;
                        tempAcc.MobileNumber = x.MobileNumber;
                        tempAcc.Name = x.Name;
                        tempAcc.Nationality = x.Nationality;
                        tempAcc.NatureOfBusiness = x.NatureOfBusiness;
                        tempAcc.NRIC = x.NRIC;
                        tempAcc.Occupation = x.Occupation;
                        tempAcc.PostalCode = x.PostalCode;
                        tempAcc.PR = x.PR;
                        tempAcc.Salutation = x.Salutation;
                        tempAcc.SelfEmployed = x.SelfEmployed;
                        tempAcc.UniqueID = x.UniqueID;
                        break;
                    }
                }
                return View(tempAcc);
            }
            else
            {
                AccountFormModel account = new AccountFormModel();
                account.DOB = DateTime.Now;
                return View(account);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitAccountInfo(AccountFormModel account, int ID)
        {
            if (ID == 1)
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
                        AddAccountHolderToFirebase(account);
                    }
                }
                else
                {
                    TempData["ValidationNRIC"] = "NRIC Exists in Singpass";
                    return View(account);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["InvalidNRIC"] = "";

                bool flag = false;

                if (account.Occupation == null || account.PR == null || account.Gender == null || account.SelfEmployed == null || account.HomeAddress == null || account.PostalCode == null || account.Email == null || account.MobileNumber == null)
                {
                    flag = true;
                }

                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("AccountHolder");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

                //retrieve from singpass user in firebase
                FirebaseResponse response2 = client.Get("SingpassUser");
                dynamic data2 = JsonConvert.DeserializeObject<dynamic>(response2.Body);
                var SingpassHolderList = new List<SingpassModel>();

                var accExistInSP = false;
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

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<AccountFormModel>(((JProperty)item).Value.ToString()));
                    }
                }

                AccountFormModel accFormVerify = new AccountFormModel();

                // only run this code if it is during the second applicant's applicant process
                if (HttpContext.Session.GetString("Applicant") == "Second")
                {
                    foreach (var accountForm in list)
                    {
                        // find the first applicant's nric
                        if (accountForm.AccountID == HttpContext.Session.GetInt32("FirstUserAccID"))
                        {
                            // set the selected accountform object to the accformverify variable
                            accFormVerify = accountForm;
                            break;
                        }
                    }

                    // verify that the second applicant's nric is not the same as the first applicant's. else display error message.
                    if (accFormVerify.NRIC == account.NRIC)
                    {
                        // send error message not the same confirm
                        TempData["InvalidNRIC"] = "The NRIC entered cannot be the same as the NRIC of the first applicant.";
                        return View(account);
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
                    if (!accExistInSP)
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
                            AddAccountHolderToFirebase(account);
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
                    else
                    {
                        TempData["ValidationNRIC"] = "NRIC Exists in Singpass";
                        return View(account);
                    }

                }

                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult SendContinueEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendContinueEmail(SecondEmail email)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response1 = client.Get("AccountHolder");

            dynamic data1 = JsonConvert.DeserializeObject<dynamic>(response1.Body);
            var accountFormList = new List<AccountFormModel>();

            bool emailExists = false;

            if (data1 != null)
            {
                foreach(var item in data1)
                {
                    accountFormList.Add(JsonConvert.DeserializeObject<AccountFormModel>(((JProperty)item).Value.ToString()));
                }
            }

            foreach (var i in accountFormList)
            {
                if (i.Email == email.EmailAddr)
                {
                    emailExists = true;
                    break;
                }
            }

            // If email does not exist
            if (emailExists == false)
            {
                TempData["IsEmailExist"] = "This email does not exist. Please try another email.";
                return View(email);
            }

            //retrieve from singpass user in firebase
            FirebaseResponse response2 = client.Get("SingpassUser");
            
            dynamic data2 = JsonConvert.DeserializeObject<dynamic>(response2.Body);
            var SingpassHolderList = new List<SingpassModel>();

            var accExistInSP = false;

            if (data2 != null)
            {
                foreach (var item in data2)
                {
                    SingpassHolderList.Add(JsonConvert.DeserializeObject<SingpassModel>(((JProperty)item).Value.ToString()));
                }
            }

            foreach (var x in SingpassHolderList)
            {
                if (x.Email == email.EmailAddr)
                {
                    accExistInSP = true;
                    break;
                }
                else
                {
                    accExistInSP = false;
                }
            }

            if(!accExistInSP)
            {
                SendEmail(email.EmailAddr);
                return RedirectToAction("VerifyCode", "NonSP");
            }
            else
            {
                TempData["IsEmailExist"] = "Email Exist for Singpass";
                return View();
            }
        }

        public ActionResult VerifyCode()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerifyCode(VerificationEmail vEmail)
        {
            if (vEmail.VerificationCode == vEmail.InputCode)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("AccountHolder");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

                var list = new List<AccountFormModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<AccountFormModel>(((JProperty)item).Value.ToString()));
                    }
                }
                foreach (var x in list)
                {
                    if (x.Email == vEmail.EmailAddr)
                    {
                        if (x.AccountCreated == "N")
                        {
                            TempData["accId"] = x.AccountID;
                        }
                    }
                }
                return RedirectToAction("SubmitAccountInfo", "NonSP");
            }
            else
            {
                TempData["CodeErrorMessage"] = "Wrong Verification Code";
                TempData["VerificationCode"] = vEmail.VerificationCode;
                TempData["EmailAddr"] = vEmail.EmailAddr;
                return RedirectToAction("VerifyCode", "NonSP");
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void SendEmail(string email)
        {
            bool emailExists = true;

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("AccountHolder");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var AccHolderList = new List<AccountFormModel>();

            if (data != null)
            {
                foreach (var item in data)
                {
                    AccHolderList.Add(JsonConvert.DeserializeObject<AccountFormModel>(((JProperty)item).Value.ToString()));
                }
            }
            else
            {
                TempData["EmailErrorMessage"] = "Invalid Email!";
            }
            foreach (var x in AccHolderList)
            {
                if (x.Email == email)
                {
                    if (x.AccountCreated == "N")
                    {
                        string verificationCode = RandomString(5);

                        string messageBody = @"Dear user," + "\n" +
                                                          "A request has been made to retrieve your information to continue a Joint-Account Application." + "\n"
                                                        + "Please click on the link below to continue the process." + "\n\n"
                                                        + "OCBC Joint-Account Verification Code: " + verificationCode;

                        MailAddress from = new MailAddress("ocbcpfdgroup5noreply@gmail.com");
                        MailAddress to = new MailAddress(email);
                        MailMessage message = new MailMessage(from, to);
                        message.Subject = "OCBC Joint-Account Creation - Retrieval of Data";
                        message.Body = messageBody;
                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                        client.Credentials = new System.Net.NetworkCredential("ocbcpfdgroup5noreply@gmail.com", "pfdgrp5123.");
                        client.EnableSsl = true;

                        try
                        {
                            client.Send(message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception caught in sending email(): {0}",
                                ex.ToString());
                        }

                        TempData["EmailAddr"] = email;
                        TempData["VerificationCode"] = verificationCode;
                        break;
                    }
                }
            }
        }
    }
}
