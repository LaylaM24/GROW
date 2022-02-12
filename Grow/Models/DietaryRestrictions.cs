using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class DietaryRestrictions
    {
        public DietaryRestrictions()
        {
            this.MemberRestrictions = new HashSet<MemberRestrictions>();
        }

        public int ID { get; set; }

        [Display(Name = "Restriction")]
        [Required(ErrorMessage = "Restriction description is a required field.")]
        public string Restriction { get; set; }

        [Display(Name = "Member Restrictions")]
        public ICollection<MemberRestrictions> MemberRestrictions { get; set; }
    }
}
