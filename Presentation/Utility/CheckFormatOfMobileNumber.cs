using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Utility
{
    public class CheckFormatOfMobileNumber : ValidationAttribute, IClientModelValidator
    {
        public  void AddValidation(ClientModelValidationContext context)
        {
           
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-mobilenumber", GetErrorMessage());
        }
        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }
        public string GetErrorMessage() =>
        $"Enter Valid Phone Number";

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var str = Convert.ToString(value);
         str = str.Replace("-", "");
        if(str.Length==10 && str.All(Char.IsDigit))
        {
               return  ValidationResult.Success;
            }
           return new ValidationResult(GetErrorMessage());

        }
    }
}
