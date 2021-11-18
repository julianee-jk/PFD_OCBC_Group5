using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace PFD_OCBC_Group5.Models
{
    public class NSPVerification
    {

        [Display(Name = "NRIC")]
        public string NRIC { get; set; }


        [Display(Name = "Verification Image")]
        public string VerificationImage { get; set; }


        [Display(Name = "Verification Code")]
        public string VerificationCode { get; set; }

        [Display(Name = "DateTime File Upload")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm}")]
        public DateTime VerificationDate { get; set; }


    }
}
