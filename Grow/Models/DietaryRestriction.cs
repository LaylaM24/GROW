using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class DietaryRestriction
    {
        public DietaryRestriction()
        {
            this.MemberRestrictions = new HashSet<MemberRestriction>();
        }

        public int ID { get; set; }

        [Display(Name = "Restriction")]
        [Required(ErrorMessage = "Restriction description is a required field.")]
        public string Restriction { get; set; }

        [Display(Name = "Member Restrictions")]
        public ICollection<MemberRestriction> MemberRestrictions { get; set; }
    }
}
