using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebApplication3.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication3.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<CustomIdentitiyUser> userManager;
        private readonly SignInManager<CustomIdentitiyUser> signInManager;

        public AccountController(UserManager<CustomIdentitiyUser> userManager,SignInManager<CustomIdentitiyUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        //if Email is already registered
        [AllowAnonymous]
        [AcceptVerbs("GET","POST")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user=await userManager.FindByEmailAsync(email);

            if (user== null)
            {
                return Json(true);
            }
            else
            {
                return Json($"The email {email} is already in use");
            }
        }


        //Register Action Method(Get)
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        //Register Action Method(Post)
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = new CustomIdentitiyUser
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email,
                    City=registerViewModel.City
                };
                var result=await userManager.CreateAsync(user, registerViewModel.Password);

                if(result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach(var errors in result.Errors)
                {
                    ModelState.AddModelError("", errors.Description);
                }
            }
            return View(registerViewModel);
        }

        //Login Action Method(Get)
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        //Login Action Method(Post)
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password,
                                                                     loginViewModel.RememberMe, false);

                if (result.Succeeded)
                {

                        if(!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("index", "home");
                        }
                    
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "invalid login attempt");
                }
                 
                
            }
            return View(loginViewModel);
        }

        //Logout Action Method(Post)
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
                await signInManager.SignOutAsync();
                return RedirectToAction("index", "home");
        }
    }
}
