using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class FinancialStatus
    {
        public FinancialStatus()
        {
            this.MemberStatus = new HashSet<MemberStatus>();
        }

        public int ID { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Financial Status is required.")]
        public string Status { get; set; }

        public ICollection<MemberStatus> MemberStatus { get; set; }
    }
}
