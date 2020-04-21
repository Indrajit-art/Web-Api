using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepos;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepos = employeeRepository;

        }

      
        public ViewResult Index()
        {
           var model= _employeeRepos.GetEmployee();
            return View(model);
        }

       
        public ViewResult Details(int? id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepos.GetEmployee(id??1),
                PageTitle= "Employee Details"
            };
            
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                Employee newemployee = _employeeRepos.Add(employee);
                return RedirectToAction("details", new { id = newemployee.ID });
            }

            return View();

        }
    }
}
