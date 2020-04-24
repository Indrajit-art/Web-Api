using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;

        public SQLEmployeeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Employee Add(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee delete(int Id)
        {
            Employee employee= context.Employees.Find(Id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }

        public Employee GetEmployee(int Id)
        {
          return context.Employees.Find(Id);  
        }

        public IEnumerable<Employee> GetEmployee()
        {
            return context.Employees;
        }

        public Employee update(Employee employeenew)
        {
            var employee  =context.Employees.Attach(employeenew);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeenew;
        }
    }
}
