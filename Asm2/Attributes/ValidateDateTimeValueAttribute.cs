using System;
using System.ComponentModel.DataAnnotations;

namespace Asm2.Attributes
{
    public class ValidateDateTimeValueAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // check if the appointment date is belong to the past
            var date = DateTime.Parse(value.ToString());
            if (date < DateTime.Now)
                return new ValidationResult("Date value is not valid!");

            // TODO: other validation

            return ValidationResult.Success;
        }
    }
}
