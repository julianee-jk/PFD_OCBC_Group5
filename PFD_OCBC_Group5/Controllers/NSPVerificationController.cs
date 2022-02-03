using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFD_OCBC_Group5.DAL;
using PFD_OCBC_Group5.Models;
using System.IO;
using System.Diagnostics;

namespace PFD_OCBC_Group5.Controllers
{
    public class NSPVerificationController : Controller
    {
        private NSPVerificationDAL NSPVerificationContext = new NSPVerificationDAL();

        // GET: NSPVerificationController
        public ActionResult UploadPhoto()
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhoto(NSPVerification verification, IFormFile photo )
        {
            verification.NRIC = HttpContext.Session.GetString("FirstNRIC");
            verification.VerificationDate = DateTime.Now;

            HttpContext.Session.SetString("Status", "New");
            

            if (photo != null &&
            photo.Length > 0)
            {
                try
                {
                    // Find the filename extension of the file to be uploaded.
                    string fileExt = Path.GetExtension(

                    photo.FileName);
                    // Rename the uploaded file with the user's NRIC.
                    string uploadedFile = "NSPVerification_" + verification.NRIC + fileExt;

                    // Get the complete path to the images folder in server
                    string savePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/nsp_files", uploadedFile);
                    // Upload the file to server
                    using (var fileSteam = new FileStream(
                    savePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(fileSteam);
                    }
                    verification.VerificationImage = uploadedFile;
                    


                    NSPVerificationContext.Add(verification);

                    ViewData["VerificationMessage"] = "File uploaded successfully.";
                }
                catch (IOException)
                {
                    //File IO error, could be due to access rights denied
                    ViewData["VerificationMessage"] = "File uploading fail!";
                }
                catch (Exception ex) //Other type of error
                {
                    ViewData["VerificationMessage"] = ex.Message;
                }
            }

            // Return redirect based on the current user
            if (HttpContext.Session.GetString("Applicant") == "Second")
            {
                return RedirectToAction("Index", "JointAccount");
            }
            else
            {
                return RedirectToAction("Validate", "SecondEmail");
            }
        }
    }
}
