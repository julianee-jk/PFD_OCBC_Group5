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
        [Range(0, 999999999999999)]
        [Required]
        public int MobileNo { get; set; }

        
        [Display(Name = "Confirm Mobile Number")]
        [Range(0, 999999999999999)]
        [Required(ErrorMessage = "Mismatched mobile numbers. Please check and enter again.")]
        public int ConfirmMobileNo { get; set; }
    }
}
