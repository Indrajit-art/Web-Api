using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<CustomIdentitiyUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<CustomIdentitiyUser> userManager)
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
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = createRoleViewModel.Role
                };

                IdentityResult identityResult = await roleManager.CreateAsync(identityRole);

                if (identityResult.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in identityResult.Errors)
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
            var result = await roleManager.FindByIdAsync(id);

            var model = new EditRolesViewModel

            {
                RoleName = result.Name,
                RoleId = result.Id
            };

            foreach (var users in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(users, result.Name))
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

            var result = await roleManager.UpdateAsync(Role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }
            else
            {
                foreach (var errors in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, errors.Description);
                }
                return View(editRolesViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string roleId)
        {
            ViewBag.roldeId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.message = $"role with id {roleId} is not found";
                return View("NotFound");
            }

            var model = new List<EditUserViewModel>();

            foreach (var user in userManager.Users)
            {
                var EditUserViewModel = new EditUserViewModel
                {
                    UserID = user.Id,
                    Username = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    EditUserViewModel.IsSelected = true;
                }
                else
                {
                    EditUserViewModel.IsSelected = false;
                }
                model.Add(EditUserViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(List<EditUserViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.message = $"role with id {roleId} is not found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserID);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { id = roleId });
        }

        [HttpGet]
        public IActionResult ListUser()
        {
            var User = userManager.Users;

            return View(User);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.message = $"User with id {id} is not found";
                return View("NotFound");
            }
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new UpdateUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                City = user.City,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.message = $"User with the id {model.Id} is not found";
                return View("NotFound");
            }

            user.Email = model.Email;
            user.UserName = model.UserName;
            user.City = model.City;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("ListUser");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.message = $"User with the id {id} is not found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUser");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View("ListUser");
            }

        }

        [HttpGet]
        public async Task<IActionResult> DeleteRoles(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.message = $"User with the id {id} is not found";
                return View("NotFound");
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View("ListRoles");
            }
        }
    }
}
