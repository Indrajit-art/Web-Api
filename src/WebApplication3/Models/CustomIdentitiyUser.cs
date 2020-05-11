using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class CustomIdentitiyUser : IdentityUser
    {
        public string City { get; set; }
    }
}
