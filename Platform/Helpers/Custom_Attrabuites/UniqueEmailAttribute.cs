using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Platform.Model.Auth
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value as string;
            if (email != null)
            {
                var userManager = (UserManager<IdentityUser>)validationContext.GetService(typeof(UserManager<IdentityUser>));
                var existingUser = userManager.FindByEmailAsync(email).Result;
                if (existingUser != null)
                {
                    return new ValidationResult("Email is already taken.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
   