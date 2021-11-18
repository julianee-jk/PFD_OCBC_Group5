using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PFD_OCBC_Group5.Models
{
    public class NSPVerification
    {
        public string NRIC { get; set; }

        public string MobileNo { get; set; }

        public string VerificationImage { get; set; }

        public string VerificationCode { get; set; }

        public IFormFile File { get; set; }
    }
}
