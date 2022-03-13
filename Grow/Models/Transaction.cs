using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class Transaction
    {
        public Transaction()
        {
            this.TransactionDetails = new HashSet<TransactionDetail>();
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

        // Foreign keys
        [Display(Name = "Household")]
        [Required(ErrorMessage = "Household is required.")]
        public int HouseholdID { get; set; }

        public Household Household { get; set; }

        [Display(Name = "Volunteer")]
        [Required(ErrorMessage = "Volunteer is required.")]
        public int VolunteerID { get; set; }

        public Volunteer Volunteer { get; set; }

        [Display(Name = "Transaction Details")]
        public ICollection<TransactionDetail> TransactionDetails { get; set; }
    }
}
