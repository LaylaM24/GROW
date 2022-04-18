using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.ViewModels
{
    public class EmployeeAdminVM : EmployeeVM
    {
        public EmployeeAdminVM()
        {
            UserRoles = new List<string>();
        }

        [Display(Name = "Roles")]
        public List<string> UserRoles { get; set; }
    }
}
