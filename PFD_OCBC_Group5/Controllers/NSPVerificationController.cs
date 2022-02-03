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
using System.Drawing;
using OfficeOpenXml;

namespace PFD_OCBC_Group5.Controllers
{
    public class NSPVerificationController : Controller
    {
        private NSPVerificationDAL NSPVerificationContext = new NSPVerificationDAL();
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // GET: NSPVerificationController
        public ActionResult UploadPhoto()
        {
            string generatedCode = RandomString(8);
            TempData["generatedCode"] = generatedCode;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhoto(NSPVerification verification, IFormFile photo)
        {

            verification.NRIC = HttpContext.Session.GetString("FirstNRIC");
            verification.VerificationDate = DateTime.Now;

            HttpContext.Session.SetString("Status", "New");

            CreateExcel(verification.VerificationCode);

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


            return RedirectToAction("Validate", "SecondEmail");
        }

        public ActionResult CreateExcel(string code)
        {
            string filepath = "Verification Code Generated.xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage())
            {
                //Add Worksheets in Excel file
                excel.Workbook.Worksheets.Add("Verification Code Generated");

                //Create Excel file in Uploads folder of your project
                FileInfo excelFile = new FileInfo(filepath);

                //Add header row columns name in string list array
                var headerRow = new List<string[]>()
                  {
                    new string[] { "Verification Code Generated" }
                  };

                // Get the header range
                string Range = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                // get the workSheet in which you want to create header
                var worksheet = excel.Workbook.Worksheets["Verification Code Generated"];

                // Populate & style header row data
                worksheet.Cells[Range].Style.Font.Bold = true;
                worksheet.Cells[Range].LoadFromArrays(headerRow);

                //lock all cells except inputs
                worksheet.Protection.IsProtected = true;
                worksheet.Cells["A2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A2"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                //auto size all columns
                worksheet.Cells["A1:K20"].AutoFitColumns();

                //2 is rowNumber 1 is column number
                worksheet.Cells[2, 1].Value = code;

                //Save Excel file
                excel.SaveAs(excelFile);
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            string fileName = "Verification Code Generated.xlsx";

            //return RedirectToAction("Index");
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

       
    }
}
