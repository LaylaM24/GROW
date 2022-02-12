using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class MemberRestrictions
    {
        [Display(Name = "Dietary Restrictions")]
        [Required(ErrorMessage = "Dietary Restriction is a required field.")]
        public int DietaryRestrictionsID { get; set; }
        public DietaryRestrictions DietaryRestrictions { get; set; }

        [Display(Name = "Member")]
        [Required(ErrorMessage = "Member is a required field.")]
        public int MemberID { get; set; }
        public Member Member { get; set; }

        [Display(Name = "Notes")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string Notes { get; set; }
    }
}
