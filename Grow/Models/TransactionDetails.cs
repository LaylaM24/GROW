using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class TransactionDetails
    {
        public int ID { get; set; }

        [Display(Name = "Transaction")]
        [Required(ErrorMessage = "Transaction is required.")]
        public int TransactionID { get; set; }
        public Transactions Transactions { get; set; }

        [Required(ErrorMessage = "Item is required.")]
        public string Item { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }

        [Display(Name = "Unit Cost")]
        [Required(ErrorMessage = "Unit Cost is required.")]
        public double UnitCost { get; set; }

        [Display(Name = "Extended Cost")]
        [Required(ErrorMessage = "Extended Cost is required.")]
        public double ExtendedCost { get; set; }
    }
}
