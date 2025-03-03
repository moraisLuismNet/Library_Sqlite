using Library.DTOs;
using FluentValidation;

namespace Library.Validators
{
    public class BookUpdateValidator : AbstractValidator<BookUpdateDTO>
    {
        public BookUpdateValidator()
        {
            RuleFor(x => x.IdBook).NotEmpty().WithMessage("IdBook is required");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Title).Length(2, 20).WithMessage("Title must be between 2 and 20 characters");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(x => x.AuthorId).NotEmpty().WithMessage("AuthorId is required");
            RuleFor(x => x.PublishingHouseId).NotEmpty().WithMessage("PublishingHouseId is required");
        }
    }
}
