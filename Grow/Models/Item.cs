using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class Item
    {
        public Item()
        {
            this.TransactionDetails = new HashSet<TransactionDetail>();
        }

        public int ID { get; set; }

        [Display(Name = "Item No.")]
        [Required(ErrorMessage = "Item number is required.")]
        public int ItemNo { get; set; }

        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "Item name cannot be longer than 50 characters")]
        [Required(ErrorMessage = "Item name is required.")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category is required.")]
        public int ItemCategoryID { get; set; }

        [Display(Name = "Category")]
        public ItemCategory ItemCategory { get; set; }

        public ICollection<TransactionDetail> TransactionDetails { get; set; }
    }
}
