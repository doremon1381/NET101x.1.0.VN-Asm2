using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Asm2.Attributes
{
    public class ValidateDateFormatAttribute : ValidationAttribute
    {
        public string Format { get; set; } = "yyyy-MM-ddTHH:mm"; 
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string && DateTime.TryParseExact(value as string, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                return ValidationResult.Success;

            return new ValidationResult($"Invalid dattime. Expected format is: {Format}");
        }
    }
}
