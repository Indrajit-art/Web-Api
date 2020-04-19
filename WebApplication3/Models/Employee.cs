using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class Employee
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string email { get; set; }

        public Dept Department { get; set; }

        

    }
}
