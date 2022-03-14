using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Models
{
    public class GROWAddress
    {
        public int ID { get; set; }

        public string Address
        {
            get
            {
                return StreetNumber + " " + StreetName + (ApartmentNumber != null ? $" Apt. {ApartmentNumber}" : "") + (City != null ? ", " + City.CityName : "") + " " + PostalCode?.ToUpper();
            }
        }

        [Display(Name = "Street No.")]
        [Required(ErrorMessage = "Street Number is a required field.")]
        [StringLength(10, ErrorMessage = "Street number cannot exceed 10 characters.")]
        public string StreetNumber { get; set; }

        [Display(Name = "Street Name")]
        [StringLength(100, ErrorMessage = "Street name cannot exceed 100 characters.")]
        [Required(ErrorMessage = "Street Name is a required field.")]
        public string StreetName { get; set; }

        [Display(Name = "Apt No.")]
        [StringLength(10, ErrorMessage = "Apartment number cannot exceed 10 characters.")]
        public string ApartmentNumber { get; set; }

        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "Postal Code is a required field.")]
        [RegularExpression("^[A-Za-z]\\d[A-Za-z](| )\\d[A-Za-z]\\d$", ErrorMessage = "Please enter a valid Postal Code.")]
        public string PostalCode { get; set; }

        // Foreign Keys
        [Display(Name = "City")]
        [Required(ErrorMessage = "You must select a City.")]
        public int CityID { get; set; }
        public City City { get; set; }
    }
}
