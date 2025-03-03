using Library.DTOs;
using FluentValidation;

namespace Library.Validators
{
    public class AuthorUpdateValidator : AbstractValidator<AuthorUpdateDTO>
    {
        public AuthorUpdateValidator()
        {
            RuleFor(x => x.IdAuthor).NotNull().WithMessage("IdAuthor is required");
            RuleFor(x => x.NameAuthor).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.NameAuthor).Length(2, 20).WithMessage("Name must be between 2 and 20 characters");
        }
    }
}
