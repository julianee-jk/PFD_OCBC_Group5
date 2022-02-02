using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFD_OCBC_Group5.Models;
using System.Net.Mail;
using System.Diagnostics;

namespace PFD_OCBC_Group5.Controllers
{
    public class SecondEmailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Validate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Validate(SecondEmail secondEmail)
        {
            if (ModelState.IsValid)
            {
                // Confirm email
                if (secondEmail.EmailAddr == secondEmail.ConfirmEmailAddr)
                {
                    HttpContext.Session.SetString("Applicant", "Second");

                    string accId = HttpContext.Session.GetInt32("AccountID").ToString();

                    // Link to send to second person via Email - TO:DO
                    // Need to check if is singpass user or not
                    string secondPersonLink = "https://localhost:44382/Singpass/SingpassLogin?currentUser=2&accId=" + accId;

                    // Email body text
                    string messageBody = @"Dear user," + "\n" +
                                              "You have been invited to an OCBC Joint-Account as the second applicant." + "\n"
                                            + "Please click on the link below to continue the process." + "\n\n"
                                            + "OCBC Joint-Account Application Link: " + secondPersonLink;

                    // Send Email here
                    SendEmail("OCBC Joint-Account Creation - 2nd Applicant", messageBody, secondEmail.ConfirmEmailAddr);

                    if (HttpContext.Session.GetString("Type") == "Singpass")
                    {
                        // Redirect the first applicant to the Awaiting/Index page
                        return RedirectToAction("Index", "Awaiting");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home"); // TO:DO
                    }
                }
                else
                {
                    // Send error message not the same confirm
                    ViewBag.MismatchNum = "The confirmation email does not matched the previously entered email. Please check and enter again.";
                    return View(secondEmail);
                }
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(secondEmail);
            }
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
