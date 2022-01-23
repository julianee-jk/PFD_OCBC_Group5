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


        [StringLength(200)]
        public string Name { get; set; }


        [StringLength(100)]
        public string Nationality { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        public string Gender { get; set; }

        [StringLength(200)]
        [Display(Name = "Home Address")]
        public string HomeAddress { get; set; }

        [StringLength(6)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

    }
}
