using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFD_OCBC_Group5.Models;

namespace PFD_OCBC_Group5.Controllers
{
    public class JointAccountController : Controller
    {
        // GET: JointAccountController
        // Auto-create account in DAL and retrieve auto-incremented PK as Account Number
        // Retrieve OwnerNRIC, JointNRIC and Relationship through TempData passing

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index(JointAccountModel jointAccount)
        {
            return View(jointAccount);
        }
    }
}