using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class HealthConcern
    {
        public HealthConcern()
        {
            this.MemberConcerns = new HashSet<MemberConcern>();
        }

        public int ID { get; set; }

        [Display(Name = "Health Concern")]
        [Required(ErrorMessage = "Health concern is a required field.")]
        public string Concern { get; set; }

        [Display(Name = "Member Concerns")]
        public ICollection<MemberConcern> MemberConcerns { get; set; }
    }
}
