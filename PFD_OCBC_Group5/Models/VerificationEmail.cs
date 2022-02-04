using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PFD_OCBC_Group5.Models
{
    public class VerificationEmail
    {
        [Display(Name = "Email")]
        [EmailAddress]
        [Required(ErrorMessage = "Please enter an email address!")]
        public string EmailAddr { get; set; }

        public string VerificationCode { get; set; }

        public string InputCode { get; set; }


    }
}
