using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PFD_OCBC_Group5.Models
{
    public class SecondMobile
    {
        [Display(Name = "Mobile Number")]
        [Range(1000, 999999999999999, ErrorMessage = "Mobile numbers can only be between 4 & 15 digits.")]
        [Required(ErrorMessage = "Please enter a mobile number!")]
        public int MobileNo { get; set; }

        
        [Display(Name = "Confirm Mobile Number")]
        [Range(1000, 999999999999999, ErrorMessage = "Mobile numbers can only be between 4 & 15 digits.")]
        [Required(ErrorMessage = "Please enter a mobile number!")]
        public int ConfirmMobileNo { get; set; }
    }
}
