using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int Id);
        //to retrieve all employee in a list
        IEnumerable<Employee> GetEmployee();
    }
}
