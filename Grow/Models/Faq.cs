using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class Faq
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "The question was not provided.")]
        public string Question { get; set; }

        [Required(ErrorMessage = "The question must have an answer!")]
        public string Answer { get; set; }
    }
}
