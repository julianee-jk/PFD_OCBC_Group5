using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFD_OCBC_Group5.Models;
using System.Diagnostics;
using PFD_OCBC_Group5.DAL;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mail;

namespace PFD_OCBC_Group5.Controllers
{
    public class JointAccountController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            BasePath = "https://pfd-group-5-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;

        public ActionResult Index()
        {
            JointAccountModel ja = new JointAccountModel();
            JointAccountView jaView = new JointAccountView();

            // Retrieve account holder
            AccountFormModel firstUser = GetAccountHolder(HttpContext.Session.GetInt32("FirstUserAccID").Value);
            AccountFormModel secondUser = GetAccountHolder(HttpContext.Session.GetInt32("SecondUserAccID").Value);

            // Update the status of the account holder's to Account Created = "Y"
            UpdateAccountHolderInFirebase(firstUser);
            UpdateAccountHolderInFirebase(secondUser);

            // Add the newly created Joint Account to Firebase
            ja.AccountNumber = GetAccountNumber();
            ja.OwnerNRIC = firstUser.NRIC;
            ja.JointNRIC = secondUser.NRIC;
            ja.RelationshipToOwner = HttpContext.Session.GetString("RelationshipWithOwner");
            AddJointAccountToFirebase(ja);

            // Return JointAccountView model and the model data back to the view
            jaView.ja = ja;
            jaView.firstName = firstUser.Name;
            jaView.secondName = secondUser.Name;

            // Retrieve accId and relationship
            string accId = HttpContext.Session.GetInt32("AccountID").ToString();

            // Email Section
            // Email body text
            string messageBodyOwner = @"Dear Applicant," + "\n" +
                                      "Congragulations! Your OCBC Joint-Account with " + secondUser.Name + " has been successfully created!" + "\n\n"
                                    + "Your bank account details are as shown below:" + "\n"
                                    + "Joint-account Account Number: " + ja.AccountNumber + "\n"
                                    + "Joint-account Owner: " + firstUser.Name + "\n"
                                    + "Joint-account Second Owner: " + secondUser.Name + "\n"
                                    + "Relationship of Owner with Second Owner: " + ja.RelationshipToOwner + "\n\n"
                                    + "Information in this email is confidential. It is intended solely for the person to whom it is addressed to. If you are not the intended recipient, you are not to disseminate, distribute or copy this communication. Please notify the sender and delete the message and any other record of it from your system immediately.";

            string messageBodyJoint = @"Dear Applicant," + "\n" +
                                      "Congragulations! Your OCBC Joint-Account with " + firstUser.Name + " has been successfully created!" + "\n\n"
                                    + "Your bank account details are as shown below:" + "\n"
                                    + "Joint-account Account Number: " + ja.AccountNumber + "\n"
                                    + "Joint-account Owner: " + firstUser.Name + "\n"
                                    + "Joint-account Second Owner: " + secondUser.Name + "\n"
                                    + "Relationship of Owner with Second Owner: " + ja.RelationshipToOwner + "\n\n"
                                    + "Information in this email is confidential. It is intended solely for the person to whom it is addressed to. If you are not the intended recipient, you are not to disseminate, distribute or copy this communication. Please notify the sender and delete the message and any other record of it from your system immediately.";

            // Send Email to the two account holders that have just created the account
            SendEmail("OCBC Joint-Account - Application Successful", messageBodyOwner, firstUser.Email);
            SendEmail("OCBC Joint-Account - Application Successful", messageBodyJoint, secondUser.Email);

            return View(jaView);
        }

        private void AddJointAccountToFirebase(JointAccountModel jointAcc)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = jointAcc;

            // Create Joint Account
            PushResponse response = client.Push("JointAccount/", data);

            // Set UID of Firebase
            data.UniqueID = response.Result.name;

            // Update the newly created joint account's UID with the previously set UID
            SetResponse setResponse = client.Set("JointAccount/" + data.UniqueID, data);
        }

        private void UpdateAccountHolderInFirebase(AccountFormModel accForm)
        {
            List<AccountFormModel> selAccountList = new List<AccountFormModel>();
            String uid = "";

            client = new FireSharp.FirebaseClient(config);
            var data = accForm;

            // Set the account created status to Yes
            data.AccountCreated = "Y";

            // Find the account holder
            FirebaseResponse response = client.Get("AccountHolder");
            dynamic selData = JsonConvert.DeserializeObject<dynamic>(response.Body);

            // Deserialize and append data to selDataList
            var selDataList = new List<AccountFormModel>();
            foreach (var item in selData)
            {
                selDataList.Add(JsonConvert.DeserializeObject<AccountFormModel>(((JProperty)item).Value.ToString()));
            }

            // Loops through the list to locate the given unique id of the account holder
            foreach (AccountFormModel i in selDataList)
            {
                if (i.AccountCreated == "N" && i.NRIC == accForm.NRIC)
                {
                    uid = i.UniqueID;
                    break;
                }
            }

            // Update the account holder's status to Yes
            SetResponse setResponse = client.Set("AccountHolder/" + uid, data);
        }

        private AccountFormModel GetAccountHolder(int accId)
        {
            AccountFormModel selAccForm = new AccountFormModel();

            client = new FireSharp.FirebaseClient(config);

            // Find the account holder
            FirebaseResponse response = client.Get("AccountHolder");
            dynamic selData = JsonConvert.DeserializeObject<dynamic>(response.Body);

            // Deserialize and append data to selDataList
            var selDataList = new List<AccountFormModel>();
            foreach (var item in selData)
            {
                selDataList.Add(JsonConvert.DeserializeObject<AccountFormModel>(((JProperty)item).Value.ToString()));
            }

            // Loops through the list to locate the given unique id of the account holder
            foreach (AccountFormModel i in selDataList)
            {
                if (i.AccountCreated == "N" && i.AccountID == accId)
                {
                    selAccForm.AccountCreated = i.AccountCreated;
                    selAccForm.AccountID = i.AccountID;
                    selAccForm.DOB = i.DOB;
                    selAccForm.Email = i.Email;
                    selAccForm.Gender = i.Gender;
                    selAccForm.HomeAddress = i.HomeAddress;
                    selAccForm.MobileNumber = i.MobileNumber;
                    selAccForm.NRIC = i.NRIC;
                    selAccForm.Name = i.Name;
                    selAccForm.Nationality = i.Nationality;
                    selAccForm.Occupation = i.Occupation;
                    selAccForm.PR = i.PR;
                    selAccForm.PostalCode = i.PostalCode;
                    selAccForm.Salutation = i.Salutation;
                    selAccForm.SelfEmployed = i.SelfEmployed;
                    selAccForm.UniqueID = i.UniqueID;
                    break;
                }
            }

            return selAccForm;
        }

        private string GetAccountNumber()
        {
            Random randomNum = new Random();
            
            bool isAccNumberIsValid = false;
            string baseNum = null;

            client = new FireSharp.FirebaseClient(config);

            // Find the account holder
            FirebaseResponse response = client.Get("JointAccount");
            dynamic selData = JsonConvert.DeserializeObject<dynamic>(response.Body);

            // Deserialize and append data to selDataList
            var selDataList = new List<JointAccountModel>();
            if (selData != null)
            {
                foreach (var item in selData)
                {
                    selDataList.Add(JsonConvert.DeserializeObject<JointAccountModel>(((JProperty)item).Value.ToString()));
                }
            }

            if (selDataList.Count != 0)
            {
                // Loops through the list to locate the given unique id of the account holder
                while (!isAccNumberIsValid)
                {
                    string randFirst = randomNum.Next(100000, 999999).ToString();
                    string randSec = randomNum.Next(100, 999).ToString();
                    baseNum = "565-" + randFirst + "-" + randSec;

                    foreach (JointAccountModel j in selDataList)
                    {
                        if (j.AccountNumber != baseNum)
                        {
                            isAccNumberIsValid = true;
                        }
                        else
                        {
                            isAccNumberIsValid = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                string randFirst = randomNum.Next(100000, 999999).ToString();
                string randSec = randomNum.Next(100, 999).ToString();
                baseNum = "565-" + randFirst + "-" + randSec;
            }
            
            // Return the generated acc number
            return baseNum;
        }

        public static void SendEmail(string messageBody, string messageContent, string email)
        {
            MailAddress from = new MailAddress("ocbcpfdgroup5noreply@gmail.com");
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);
            message.Subject = messageBody;
            message.Body = messageContent;
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
        }
    }
}