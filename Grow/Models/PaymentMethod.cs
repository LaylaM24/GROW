using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Models
{
    public class PaymentMethod
    {
        public PaymentMethod()
        {
            this.Payments = new HashSet<Payment>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "Payment Method is required.")]
        [StringLength(50, ErrorMessage = "Payment Method cannot exceed 50 characters.")]
        public string Method { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}
