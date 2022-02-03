using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PFD_OCBC_Group5.Models
{
    public class JointAccountModel
    {
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Display(Name = "First Applicant's NRIC")]
        public string OwnerNRIC { get; set; }

        [Display(Name = "Second Applicant's NRIC")]
        public string JointNRIC { get; set; }

        [Display(Name = "Relationship of Second Applicant with Owner")]
        public string RelationshipToOwner { get; set; }

        public string UniqueID { get; set; }
    }
}
