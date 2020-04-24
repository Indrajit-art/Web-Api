using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public static class ModelBuilderExtensionscs
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(

               new Employee
               {
                   ID = 1,
                   Name = "Indra",
                   Department = Dept.TCS,
                   email = "indra@gmail.com"
               },
                new Employee
                {
                    ID = 2,
                    Name = "Abhi",
                    Department = Dept.PWC,
                    email = "Abhi@gmail.com"
                }
               );
        }
    }
}
