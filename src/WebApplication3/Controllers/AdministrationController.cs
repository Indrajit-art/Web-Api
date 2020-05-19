using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<CustomIdentitiyUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,UserManager<CustomIdentitiyUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRoleViewModel)
        {
            if(ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = createRoleViewModel.Role
                };

                IdentityResult identityResult=  await roleManager.CreateAsync(identityRole);

                if(identityResult.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach(IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(createRoleViewModel);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var Role = roleManager.Roles;

            return View(Role);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var result=  await roleManager.FindByIdAsync(id);

            var model = new EditRolesViewModel

            {
                RoleName = result.Name,
                RoleId = result.Id
            };

            foreach(var users in userManager.Users)
            {
                if(await userManager.IsInRoleAsync(users, result.Name))
                {
                    model.Users.Add(users.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRolesViewModel editRolesViewModel)
        {
            var Role = await roleManager.FindByIdAsync(editRolesViewModel.RoleId);

            Role.Name = editRolesViewModel.RoleName;

            var result=await roleManager.UpdateAsync(Role);
            
            if(result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }
            else
            {
                foreach(var errors in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, errors.Description);
                }
                return View(editRolesViewModel);
            }
            
        }
    }
}
