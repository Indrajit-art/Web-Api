using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50,ErrorMessage ="Name cannot be exceeded")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$",ErrorMessage ="Invalid Email Address")]
        public string email { get; set; }

        [Required]
        public Dept? Department { get; set; }
        

        

    }
}
