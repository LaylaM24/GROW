using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class Province
    {
        public Province()
        {
            this.Households = new HashSet<Household>();
            this.Volunteers = new HashSet<Volunteer>();
        }
        //hello
        public int ID { get; set; }

        [Display(Name = "Province")]
        [Required(ErrorMessage = "Province name is required.")]
        public string ProvinceName { get; set; }

        public ICollection<Household> Households { get; set; }
        public ICollection<Volunteer> Volunteers { get; set; }
    }
}
