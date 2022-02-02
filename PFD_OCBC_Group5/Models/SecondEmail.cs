using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PFD_OCBC_Group5.Models
{
    public class SecondEmail
    {
        [Display(Name = "Email")]
        [EmailAddress]
        [Required(ErrorMessage = "Please enter an email address!")]
        public String EmailAddr { get; set; }
        
        [Display(Name = "Confirm Email")]
        [EmailAddress]
        [Required(ErrorMessage = "Please enter a an email address!")]
        public String ConfirmEmailAddr { get; set; }

        [Display(Name = "Relationship To Applicant")]
        [Required(ErrorMessage = "Please enter your relationship with the applicant!")]
        public String RelationShipToApplicant { get; set; }
    }
}
