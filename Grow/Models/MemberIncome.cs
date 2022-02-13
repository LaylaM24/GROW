using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Models
{
    public class MemberIncome
    {
        public int ID { get; set; }

        [Display(Name = "Income Source")]
        [Required(ErrorMessage = "Income Source is a required field.")]
        public int IncomeSourceID { get; set; }
        public IncomeSource IncomeSource { get; set; }

        [Display(Name = "Member")]
        [Required(ErrorMessage = "Member is a required field.")]
        public int MemberID { get; set; }
        public Member Member { get; set; }

        [Display(Name = "Income Amount")]
        public double IncomeAmount { get; set; }
    }
}
