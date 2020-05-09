using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private string _allowDomain;

        public ValidEmailDomainAttribute(string allowDomian)
        {
            this._allowDomain = allowDomian;
        }

        public override bool IsValid(object value)
        {
            string [] Email=value.ToString().Split('@');

            return Email[1].ToUpper() == _allowDomain.ToUpper();
        }
    }
}
