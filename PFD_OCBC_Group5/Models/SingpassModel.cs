using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PFD_OCBC_Group5.Models
{
    public class SingpassModel
    {
        [Display(Name = "NRIC")]
        [Required(ErrorMessage = "Please enter a valid NRIC number.")]
        [MaxLength(9)]
        [MinLength(9)]
        public string NRIC { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter your Singpass password.")]
        public string Password { get; set; }
    }
}
