using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Models
{
    public class MemberDocument
    {
        public int ID { get; set; }

        [Display(Name = "Member")]
        [Required(ErrorMessage = "Member is required")]
        public int MemberID { get; set; }
        public Member Member { get; set; }

        [StringLength(255, ErrorMessage = "The name of the file cannot be more than 255 characters.")]
        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [ScaffoldColumn(false)]
        public byte[] Content { get; set; }

        [StringLength(255)]
        public string MimeType { get; set; }
    }
}
