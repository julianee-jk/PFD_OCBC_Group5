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

        [Display(Name = "Main Account Holder's NRIC")]
        public string OwnerNRIC { get; set; }

        [Display(Name = "Joint Account Holder's NRIC")]
        public string JointNRIC { get; set; }

        public string RelationshipToOwner { get; set; }
    }
}
