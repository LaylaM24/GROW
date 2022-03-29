using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class Item : IValidatableObject
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ItemCategoryID == 1 && ItemNo < 100 || ItemCategoryID == 1 && ItemNo > 199)
            {
                yield return new ValidationResult("Please follow the guidelines above.", new[] { "ItemNo" });
            }
            else if (ItemCategoryID == 2 && ItemNo < 200 || ItemCategoryID == 2 && ItemNo > 299)
            {
                yield return new ValidationResult("Please follow the guidelines above.", new[] { "ItemNo" });
            }
            else if (ItemCategoryID == 3 && ItemNo < 300 || ItemCategoryID == 3 && ItemNo > 399)
            {
                yield return new ValidationResult("Please follow the guidelines above.", new[] { "ItemNo" });
            }
            else if (ItemCategoryID == 4 && ItemNo < 400 || ItemCategoryID == 4 && ItemNo > 499)
            {
                yield return new ValidationResult("Please follow the guidelines above.", new[] { "ItemNo" });
            }
            else if (ItemCategoryID == 5 && ItemNo < 600 || ItemCategoryID == 5 && ItemNo > 699)
            {
                yield return new ValidationResult("Please follow the guidelines above.", new[] { "ItemNo" });
            }
        }
    }
}
