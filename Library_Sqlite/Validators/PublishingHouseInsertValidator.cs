using Library.DTOs;
using FluentValidation;

namespace Library.Validators
{
    public class PublishingHouseInsertValidator : AbstractValidator<PublishingHouseInsertDTO>
    {
        public PublishingHouseInsertValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Name).Length(2, 20).WithMessage("Name must be between 2 and 20 characters");
        }
    }
}
