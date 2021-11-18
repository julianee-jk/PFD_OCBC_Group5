using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFD_OCBC_Group5.DAL;
using PFD_OCBC_Group5.Models;
using System.IO;

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
        public async Task<IActionResult> UploadPhoto(NSPVerification verification)
        {
            if (verification.FileUpload != null &&
            verification.FileUpload.Length > 0)
            {
                try
                {
                    // Find the filename extension of the file to be uploaded.
                    string fileExt = Path.GetExtension(

                    verification.FileUpload.FileName);
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
                        await verification.FileUpload.CopyToAsync(fileSteam);
                    }
                    verification.VerificationImage = uploadedFile;
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
            return RedirectToAction("Index", "Awaiting");
        }
    }
}
