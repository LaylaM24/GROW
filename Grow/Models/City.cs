using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class City
    {
        public City()
        {
            this.Households = new HashSet<Household>();
            this.Volunteers = new HashSet<Volunteer>();
        }

        public int ID { get; set; }

        [Display(Name = "City")]
        //longest city in canada is 31 characters long
        //room for any future city name changes that are longer but shouldnt be more than 50..
        [StringLength(50, ErrorMessage = "City name cannot be longer than 50 characters")]
        [Required(ErrorMessage = "City name is required.")]
        public string CityName { get; set; }

        public ICollection<Household> Households { get; set; }
        public ICollection<Volunteer> Volunteers { get; set; }
    }
}
