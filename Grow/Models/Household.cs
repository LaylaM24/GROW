using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class Household : IValidatableObject
    {
        public Household()
        {
            this.Members = new HashSet<Member>();
            this.Transactions = new HashSet<Transactions>();
            this.MembershipChanges = new HashSet<MembershipChanges>();
        }

        public string Address
        {
            get
            {
                return StreetNumber + " " + StreetName;
            }
        }

        public int ID { get; set; }

        [Display(Name = "Household No.")]
        [Required(ErrorMessage = "Household number required to create a membership.")]
        public int MembershipNumber { get; set; }

        [Display(Name = "Number of Members")]
        [Required(ErrorMessage = "Number of members is required.")]
        public byte? NumOfMembers { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Creation Date is a required field.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "LICO Verified")]
        [Required(ErrorMessage = "LICO Verified must be true or false.")]
        public Boolean LICOVerified { get; set; }

        [Display(Name = "LICO Verification Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "LICO Verification Date is a required field.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LICOVerifiedDate { get; set; }

        [Display(Name = "Total Income")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Total Income is a required field.")]
        public double? IncomeTotal { get; set; }

        [Display(Name = "Renewal Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Renewal Date is a required field.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? RenewalDate { get; set; }

        [Display(Name = "Street No.")]
        [Required(ErrorMessage = "Street Number is a required field.")]
        [StringLength(10, ErrorMessage = "Street number cannot exceed 10 characters.")]
        public string StreetNumber { get; set; }

        [Display(Name = "Street Name")]
        [StringLength(100, ErrorMessage = "Street name cannot exceed 100 characters.")]
        [Required(ErrorMessage = "Street Name is a required field.")]
        public string StreetName { get; set; }

        [Display(Name = "Apartment No.")]
        [StringLength(10, ErrorMessage = "Apartment number cannot exceed 10 characters.")]
        public string ApartmentNumber { get; set; }

        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "Postal Code is a required field.")]
        [RegularExpression("^[A-Za-z]\\d[A-Za-z](| |-)\\d[A-Za-z]\\d$", ErrorMessage = "Please enter a valid Postal Code.")]
        public string PostalCode { get; set; }

        // Foreign Keys
        [Display(Name = "Province")]
        [Required(ErrorMessage = "You must select a Province.")]
        public int? ProvinceID { get; set; }
        public Province Province { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "You must select a City.")]
        public int? CityID { get; set; }
        public City City { get; set; }

        public ICollection<Member> Members { get; set; }
        public ICollection<Transactions> Transactions { get; set; }
        public ICollection<MembershipChanges> MembershipChanges { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CreatedDate.GetValueOrDefault() > DateTime.Today)
            {
                yield return new ValidationResult("Creation Date cannot be set in the future.", new[] { "CreatedDate" });
            }

            if (LICOVerifiedDate.GetValueOrDefault() > DateTime.Today)
            {
                yield return new ValidationResult("LICO Verification Date cannot be set in the future.", new[] { "LICOVerifiedDate" });
            }

            if (RenewalDate.GetValueOrDefault() > DateTime.Today)
            {
                yield return new ValidationResult("Renewal Date cannot be set in the future.", new[] { "RenewalDate" });
            }
        }
    }
}
