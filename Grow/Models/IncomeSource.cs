using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class IncomeSource
    {
        public IncomeSource()
        {
            this.MemberIncomes = new HashSet<MemberIncome>();
        }

        public int ID { get; set; }

        [Display(Name = "Income Source")]
        [Required(ErrorMessage = "Income Source is required.")]
        public string Source { get; set; }

        public ICollection<MemberIncome> MemberIncomes { get; set; }
    }
}
