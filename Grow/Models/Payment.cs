using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Models
{
    public class Payment
    {
        public int ID { get; set; }

        [Display(Name = "Payment Amount")]
        [Required(ErrorMessage = "Payment Amount is required.")]
        [DataType(DataType.Currency)]
        public double PaymentAmount { get; set; }

        // Foreign Keys
        [Display(Name = "Payment Method")]
        [Required(ErrorMessage = "Payment Method is required.")]
        public int PaymentMethodID { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        [Display(Name = "Transaction")]
        [Required(ErrorMessage = "Transaction is required.")]
        public int TransactionID { get; set; }
        public Transaction Transaction { get; set; }
    }
}
