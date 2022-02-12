using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Models
{
    public class MemberStatus
    {
        [Display(Name = "Financial Status")]
        [Required(ErrorMessage = "Financial Status is a required field.")]
        public int FinancialStatusID { get; set; }
        public FinancialStatus FinancialStatus { get; set; }

        [Display(Name = "Member")]
        [Required(ErrorMessage = "Member is a required field.")]
        public int MemberID { get; set; }
        public Member Member { get; set; }

        [Display(Name = "Documents")]
        public int? MemberDocumentsID { get; set; }
        public MemberDocument MemberDocuments { get; set; }

        [Display(Name = "Income Amount")]
        public double IncomeAmount { get; set; }

        [Display(Name = "Notes")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string Notes { get; set; }
    }
}
