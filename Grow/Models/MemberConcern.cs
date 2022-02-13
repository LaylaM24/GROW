using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class MemberConcern
    {
        public int ID { get; set; }

        [Display(Name = "Health Concern")]
        [Required(ErrorMessage = "Health Concern is a required field.")]
        public int HealthConcernID { get; set; }
        public HealthConcern HealthConcern { get; set; }

        [Display(Name = "Member")]
        [Required(ErrorMessage = "Member is a required field.")]
        public int MemberID { get; set; }
        public Member Member { get; set; }

        [Display(Name = "Notes")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string Notes { get; set; }
    }
}
