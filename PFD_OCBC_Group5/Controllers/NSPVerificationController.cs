using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFD_OCBC_Group5.DAL;
using PFD_OCBC_Group5.Models;

namespace PFD_OCBC_Group5.Controllers
{
    public class NSPVerificationController : Controller
    {
        private NSPVerificationDAL NSPVerificationContext = new NSPVerificationDAL();

        // GET: NSPVerificationController
        public ActionResult Index()
        {
            return View();
        }

        // GET: NSPVerificationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NSPVerificationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NSPVerificationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //public ActionResult UploadFile(int id)
        //{
        //    if ((HttpContext.Session.GetString("Role") == null) ||
        //       (HttpContext.Session.GetString("Role") != "Competitor"))
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    int competitorid = (int)HttpContext.Session.GetInt32("CompetitorID");

        //    //NSPVerification verification = NSPVerificationContext.(TODO: Whatever the function name is;


        //    return View(verification);


        //}
    }
}
