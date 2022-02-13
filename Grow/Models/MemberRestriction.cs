using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class MemberRestriction
    {
        public int ID { get; set; }

        [Display(Name = "Dietary Restriction")]
        [Required(ErrorMessage = "Dietary Restriction is a required field.")]
        public int DietaryRestrictionID { get; set; }
        public DietaryRestriction DietaryRestriction { get; set; }

        [Display(Name = "Member")]
        [Required(ErrorMessage = "Member is a required field.")]
        public int MemberID { get; set; }
        public Member Member { get; set; }

        [Display(Name = "Notes")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string Notes { get; set; }
    }
}
