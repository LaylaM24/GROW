using System.ComponentModel.DataAnnotations;

namespace Grow.Models
{
    public class Faq
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "a question is required.")]
        public string Question { get; set; }

        [Required(ErrorMessage = "an question must have an answer!")]
        public string Answer { get; set; }


    }
}
