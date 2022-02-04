using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PFD_OCBC_Group5.Models
{
    public class ValidateConfirmPassword
    {

        public SingpassModel accountInformation { get; set; }

        [Display(Name = "Confirm Password")]
        public string cfmPassword { get; set; }
         
    }
}
