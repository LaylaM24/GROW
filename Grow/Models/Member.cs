using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class Member : IValidatableObject
    {
        public Member()
        {
            this.MemberDocuments = new HashSet<MemberDocument>();
            this.MemberRestrictions = new HashSet<MemberRestriction>();
            this.MemberConcerns = new HashSet<MemberConcern>();
            this.MemberIncomes = new HashSet<MemberIncome>();
        }

        [Display(Name = "Member")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Display(Name = "Member")]
        public string FormalName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int a = today.Year - DOB.Year - ((today.Month < DOB.Month || (today.Month == DOB.Month && today.Day < DOB.Day) ? 1 : 0));
                return a;
            }
        }

        [Display(Name = "Phone")]
        public string PhoneFormatted
        {
            get
            {
                return Phone.Substring(0, 3) + "-" + Phone.Substring(3, 3) + "-" + Phone[6..];
            }
        }

        public int ID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is a requried field.")]
        [StringLength(75, ErrorMessage = "First name cannot be more than 75 characters long.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is a required field.")]
        [StringLength(75, ErrorMessage = "Last name cannot exceed 75 characters.")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date of Birth is a required field.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number is a required field.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number (no spaces).")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(255, ErrorMessage = "Email Address cannot exceed 255 characters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Income Verified")]
        [Required(ErrorMessage = "Income Verified is a required field.")]
        public bool IncomeVerified { get; set; }

        [Display(Name = "Annual Income")]
        [Required(ErrorMessage = "Annual Income is a required field.")]
        [DataType(DataType.Currency)]
        public double IncomeAmount { get; set; }

        [Display(Name = "Income Verified By")]
        public string IncomeVerifiedBy { get; set; }

        [Display(Name = "Data Consent")]
        [Required(ErrorMessage = "Data Consent is a required field.")]
        public bool DataConsent { get; set; }

        [Display(Name = "Notes")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string Notes { get; set; }

        //Foreign Keys
        [Display(Name = "Household")]
        [Required(ErrorMessage = "Household is a required field.")]
        public int HouseholdID { get; set; }
        public Household Household { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender is a required field.")]
        public int GenderID { get; set; }
        public Gender Gender { get; set; }

        [Display(Name = "Income Sources")]
        public ICollection<MemberIncome> MemberIncomes { get; set; }

        [Display(Name = "Dietary Restrictions")]
        public ICollection<MemberRestriction> MemberRestrictions { get; set; }

        [Display(Name = "Health Concerns")]
        public ICollection<MemberConcern> MemberConcerns { get; set; }

        [Display(Name = "Documents")]
        public ICollection<MemberDocument> MemberDocuments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DOB > DateTime.Today)
            {
                yield return new ValidationResult("Date of Birth cannot be set in the future.", new[] { "DOB" });
            }
        }
    }
}
