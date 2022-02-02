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

        public ActionResult Index()
        {
            Debug.WriteLine("First: " + HttpContext.Session.GetInt32("FirstUserAccID"));
            Debug.WriteLine("Second: " + HttpContext.Session.GetInt32("SecondUserAccID"));

            //JointAccountModel ja = new JointAccountModel();
            //ja.OwnerNRIC = HttpContext.Session.GetString("FirstNRIC");
            //ja.JointNRIC = HttpContext.Session.GetString("SecondNRIC");
            //ja.RelationshipToOwner = HttpContext.Session.GetString("SPRelationship");
            //ja.AccountNumber = (int)HttpContext.Session.GetInt32("AccountNum");

            //JointAccountView jaView = new JointAccountView();
            //jaView.firstName = AccountContext.GetApplicantInfo(ja.OwnerNRIC).Name;
            //jaView.secondName = AccountContext.GetApplicantInfo(ja.JointNRIC).Name;

            //jaView.ja = ja;

            return View();
        }
    }
}