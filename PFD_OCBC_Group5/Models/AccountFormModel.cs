using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PFD_OCBC_Group5.Models
{
    public class AccountFormModel
    {

        [Required]
        public int AccountID { get; set; }

        [Required]
        [StringLength(9)]
        public string NRIC { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(3)]
        public string Salutation { get; set; }

        [StringLength(100)]
        public string Nationality { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [StringLength(100)]
        public string Occupation { get; set; }

        [Display(Name = "Permanent Resident")]
        public string PR { get; set; }

        public string Gender { get; set; }

        [Display(Name = "Self Employed")]
        public string SelfEmployed { get; set; }

        [StringLength(100)]
        [Display(Name = "Nature of Business")]
        public string NatureOfBusiness { get; set; }

        [StringLength(200)]
        [Display(Name = "Home Address")]
        public string HomeAddress { get; set; }

        [StringLength(6)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [StringLength(200)]
        [Display(Name = "Mailing Address")]
        public string MailingAddress { get; set; }

        [StringLength(6)]
        [Display(Name = "Mailing Postal Code")]
        public string MailingPostalCode { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [StringLength(20)]
        [Display(Name = "Home Number")]
        public string HomeNumber { get; set; }

        public string AccountCreated { get; set; }
    }
}
