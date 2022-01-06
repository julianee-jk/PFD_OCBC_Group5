using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFD_OCBC_Group5.Models;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

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
                if (secondEmail.EmailAddr == secondEmail.ConfirmEmailAddr)
                {
                    HttpContext.Session.SetString("Applicant", "Second");

                    // Send Email here (TO:DO)
                    //var mailMsg = new MimeMessage();

                    //// Send from sending party
                    //mailMsg.From.Add(new MailboxAddress("OCBC", "pfdgrp5testacc@gmail.com"));

                    //// Send to receiving party
                    //mailMsg.To.Add(new MailboxAddress("TestName", "pfdgrp5testacc@gmail.com"));
                    //mailMsg.Subject = "Application for Joint Account";
                    //mailMsg.Body = new TextPart("html")
                    //{
                    //    Text = "<a>www.youtube.com</a>"
                    //};

                    //SmtpClient smtpClient = new SmtpClient();
                    //try
                    //{
                    //    // Connect to the gmail smtp server using port 465
                    //    smtpClient.Connect("smtp.gmail.com", 465, true);

                    //    // Email Address and Password authentication required for gmail smtp server connection
                    //    smtpClient.Authenticate("email", "password");
                    //    smtpClient.Send(mailMsg);
                    //} 
                    //catch (Exception ex)
                    //{
                    //    // If an error pops up, display msg
                    //    Console.WriteLine(ex.Message);

                    //    // Disconnect the smtp connection
                    //    smtpClient.Disconnect(true);
                    //    // and dispose of the object created for the client
                    //    smtpClient.Dispose();
                    //}

                    if (HttpContext.Session.GetString("Type") == "Singpass")
                    {
                        //Redirect user to Awaiting/Index page
                        return RedirectToAction("Index", "Awaiting");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Awaiting"); // TO:DO
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
    }
}
