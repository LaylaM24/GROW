using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class Gender
    {
        public Gender()
        {
            this.Members = new HashSet<Member>();
        }

        public int ID { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender type is required.")]
        public string GenderType { get; set; }

        public ICollection<Member> Members { get; set; }
    }
}
