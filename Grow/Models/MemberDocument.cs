using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Models
{
    public class MemberDocument : UploadedFile
    {
        [Display(Name = "Member")]
        public int MemberID { get; set; }

        public Member Member { get; set; }
    }
}
