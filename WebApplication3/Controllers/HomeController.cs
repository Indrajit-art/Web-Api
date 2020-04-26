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

        public HomeController(IEmployeeRepository employeeRepository,IHostingEnvironment hostingEnvironment)
        {
            _employeeRepos = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
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
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string UniqueFilename = null;
                if (model.Photos != null && model.Photos.Count > 0)
                {
                    foreach (IFormFile photo in model.Photos)
                    {
                    string UploadFolder=Path.Combine(hostingEnvironment.WebRootPath, "image");
                    UniqueFilename= Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filpath=Path.Combine(UploadFolder, UniqueFilename);
                    photo.CopyTo(new FileStream(filpath, FileMode.Create));
                    }
                }
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

        //[HttpGet]
        //public IActionResult delete(int Id)
        //{
        //    _employeeRepos.delete(Id);
        //    return RedirectToAction("details");
        //}
    }
}
