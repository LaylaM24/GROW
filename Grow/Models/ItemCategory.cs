using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class ItemCategory
    {
        public ItemCategory()
        {
            this.Items = new HashSet<Item>();
        }

        public int ID { get; set; }

        [Display(Name = "Category")]
        [StringLength(50, ErrorMessage = "Category name cannot be longer than 50 characters")]
        [Required(ErrorMessage = "Category name is required.")]
        public string CategoryName { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
