using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.utilities;

namespace WebApplication3.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", ErrorMessage = "Invalid Email Address")]
        [Remote(action: "IsEmailInUse",controller:"account")]
        [ValidEmailDomain(allowDomian:"gmail.com",ErrorMessage ="Email must be gmail.com")]
        public string Email{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [Compare("Password",ErrorMessage ="Password and Confirm password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
