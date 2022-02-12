using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class Transactions
    {
        public Transactions()
        {
            this.TransactionDetails = new HashSet<TransactionDetails>();
        }

        public int ID { get; set; }

        [Display(Name = "Transaction Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Transaction date is required.")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "Transaction Total")]
        [Required(ErrorMessage = "Transaction total is required.")]
        public double TransactionTotal { get; set; }

        // Foreign keys
        [Display(Name = "Household")]
        public int HouseholdID { get; set; }
        public Household Household { get; set; }

        [Display(Name = "Transaction Details")]
        public ICollection<TransactionDetails> TransactionDetails { get; set; }
    }
}
