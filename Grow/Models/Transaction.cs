using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class Transaction : IValidatableObject
    {
        public Transaction()
        {
            this.TransactionDetails = new HashSet<TransactionDetail>();
            this.Payments = new HashSet<Payment>();
        }

        public int ID { get; set; }

        [Display(Name = "Transaction Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Transaction date is required.")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "Transaction Total")]
        [Required(ErrorMessage = "Transaction total is required.")]
        [DataType(DataType.Currency)]
        public double TransactionTotal { get; set; }

        [Required(ErrorMessage = "Paid is a required field.")]
        public bool Paid { get; set; }

        // Foreign keys
        [Display(Name = "Household")]
        [Required(ErrorMessage = "Household is required.")]
        public int HouseholdID { get; set; }
        public Household Household { get; set; }

        [Display(Name = "Member")]
        [Required(ErrorMessage = "Member is required.")]
        public int MemberID { get; set; }
        public Member Member { get; set; }

        [Display(Name = "Volunteer")]
        [Required(ErrorMessage = "Volunteer is required.")]
        public int VolunteerID { get; set; }
        public Volunteer Volunteer { get; set; }

        [Display(Name = "Items")]
        public ICollection<TransactionDetail> TransactionDetails { get; set; }

        [Display(Name = "Payments")]
        public ICollection<Payment> Payments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TransactionDate < DateTime.Today)
            {
                yield return new ValidationResult("Transaction Data cannot be set in the past.", new[] { "TransactionDate" });
            }
        }
    }
}
