﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class MockEmployeeRepos : IEmployeeRepository
    {
        private List<Employee> _employeeList;


        public MockEmployeeRepos()
        {
            _employeeList = new List<Employee>()
                {
                    new Employee(){ ID=1,Name="Indrajit",Department=Dept.TCS,email="abc@gmail.com"},
                    new Employee(){ ID=2,Name="Abhi",Department=Dept.PWC,email="efg@gmail.com"}
                };
        }

        public Employee Add(Employee employee)
        {
             employee.ID= _employeeList.Max(e => e.ID) + 1;
            _employeeList.Add(employee);
             return employee;
        }

        public Employee GetEmployee(int Id)
        {
            //return _employeeList.FirstOrDefault(e => e.ID == Id);
            return _employeeList.Where(e => e.ID == Id).FirstOrDefault();
        }

        public IEnumerable<Employee> GetEmployee()
        {
           
            return _employeeList;
        }
    }
}

