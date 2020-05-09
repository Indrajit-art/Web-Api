using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{

    
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepos;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment)
        {
            _employeeRepos = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _employeeRepos.GetEmployee();
            return View(model);
        }

        [HttpGet]
        public ViewResult Details(int? id)
        {
            Employee employee = _employeeRepos.GetEmployee(id.Value);

            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }


        
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string UniqueFilename = ProcessUploadFile(model);
                Employee newemployee = new Employee
                {
                    Name = model.Name,
                    email = model.email,
                    Department = model.Department,
                    PhotoPath = UniqueFilename
                };

                _employeeRepos.Add(newemployee);
                return RedirectToAction("details", new { id = newemployee.ID });
            }

            return View();
        }

        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee = _employeeRepos.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.ID,
                Name = employee.Name,
                Department = employee.Department,
                email = employee.email,
                ExsistingPhoto = employee.PhotoPath
            };
            return View(employeeEditViewModel);

        }

        
        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepos.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.email = model.email;
                employee.Department = model.Department;

                if (model.Photos != null)
                {
                    if (model.ExsistingPhoto != null)
                    {
                        string FilePath = Path.Combine(hostingEnvironment.WebRootPath, "image", model.ExsistingPhoto);
                        System.IO.File.Delete(FilePath);
                    }
                    employee.PhotoPath = ProcessUploadFile(model);
                }

                _employeeRepos.update(employee);
                return RedirectToAction("details", new { id = employee.ID });
            }
            return View();
        }

            [HttpGet]
            public IActionResult delete(int Id)
            {
                _employeeRepos.delete(Id);
                return RedirectToAction("index");
            }
    

        private string ProcessUploadFile(EmployeeCreateViewModel model)
        {
            string UniqueFilename = null;
            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (IFormFile photo in model.Photos)
                {
                    string UploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "image");
                    UniqueFilename = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filpath = Path.Combine(UploadFolder, UniqueFilename);
                    using (var fileStream = new FileStream(filpath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                        
                }
            }
            return UniqueFilename;
        }
    }
}
    
