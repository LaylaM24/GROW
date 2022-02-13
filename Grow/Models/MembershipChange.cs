using System;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class MembershipChange
    {
        public int ID { get; set; }

        [Display(Name = "Household")]
        [Required(ErrorMessage = "Household is required.")]
        public int HouseholdID { get; set; }
        public Household Household { get; set; }

        [Display(Name = "Change Type")]
        [Required(ErrorMessage = "Change Type is required.")]
        public string ChangeType { get; set; }

        [Display(Name = "Change Description")]
        public string ChangeDescription { get; set; }

        [Display(Name = "Change Date")]
        [Required(ErrorMessage = "Change Date is required.")]
        public DateTime ChangeDate { get; set; }
    }
}
