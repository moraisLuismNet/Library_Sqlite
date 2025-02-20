using System.ComponentModel.DataAnnotations;

namespace Library.Validators
{
    public class NonNegativePagesValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int paginas && paginas < 0)
            {
                return new ValidationResult("The number of pages cannot be negative");
            }

            return ValidationResult.Success;
        }
    }
}
