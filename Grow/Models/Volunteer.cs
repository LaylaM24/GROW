using System;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class Volunteer
    {
        [Display(Name = "Volunteer")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string ShortName
        {
            get
            {
                return FirstName + " " + ((char?)LastName[0] + ". ").ToUpper();
            }
        }

        public string FormalName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public int ID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is a required field.")]
        [StringLength(75, ErrorMessage = "First name cannot be more than 75 characters long.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is a required field.")]
        [StringLength(75, ErrorMessage = "Last name cannot be more than 75 characters long.")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number is a required field.")]
        [StringLength(15, ErrorMessage = "First name cannot be more than 75 characters long.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number (no spaces).")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(255, ErrorMessage = "Email Address cannot exceed 255 characters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Voulunteer Hours")]
        public double VolunteerHours { get; set; }

        [Display(Name = "Street Number")]
        [Required(ErrorMessage = "Street Number is a required field.")]
        [StringLength(10, ErrorMessage = "Street number cannot exceed 10 characters.")]
        public string StreetNum { get; set; }

        [Display(Name = "Street Name")]
        [StringLength(100, ErrorMessage = "Street name cannot exceed 100 characters.")]
        [Required(ErrorMessage = "Street Name is a required field.")]
        public string StreetName { get; set; }

        [Display(Name = "Apartment Number")]
        [StringLength(10, ErrorMessage = "Apartment number cannot exceed 10 characters.")]
        public string ApartmentNum { get; set; }

        [Display(Name = "Postal Code")]
        [RegularExpression("^[A-Za-z]\\d[A-Za-z](| |-)\\d[A-Za-z]\\d$", ErrorMessage = "Please enter a valid Postal Code.")]
        public string PostalCode { get; set; }

        //Foreign Keys
        [Display(Name = "City")]
        [Required(ErrorMessage = "City is a required field.")]
        public int CityID { get; set; }
        public City City { get; set; }

    }
}
