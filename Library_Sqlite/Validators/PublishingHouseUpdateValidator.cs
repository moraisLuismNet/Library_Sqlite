﻿using Library.DTOs;
using FluentValidation;

namespace Library.Validators
{
    public class PublishingHouseUpdateValidator : AbstractValidator<PublishingHouseUpdateDTO>
    {
        public PublishingHouseUpdateValidator()
        {
            RuleFor(x => x.IdPublishingHouse).NotNull().WithMessage("IdPublishingHouse is required");
            RuleFor(x => x.NamePublishingHouse).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.NamePublishingHouse).Length(2, 20).WithMessage("Name must be between 2 and 20 characters");
        }
    }
}