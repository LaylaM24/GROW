using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class LowIncomeCutOff
    {
        public int ID { get; set; }

        [Display(Name = "Number of Members")]
        [Required(ErrorMessage = "Number of Members is required.")]
        public byte NumberOfMembers { get; set; }

        [Display(Name = "Yearly Income")]
        [Required(ErrorMessage = "Yearly Income is required.")]
        public double YearlyIncome { get; set; }
    }
}
