using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.authors.dtos;
using FluentValidation;

namespace api.models.authors.validations
{
    public class UpdateAuthorValidation : AbstractValidator<UpdateAuthorDto>
    {
        public UpdateAuthorValidation()
        {
            RuleFor(au => au.First_name)
                .NotEmpty()
                .WithMessage("First name is required")
                .MaximumLength(50)
                .WithMessage("First name must not exceed 50 characters");
            RuleFor(au => au.Last_name)
                .NotEmpty()
                .WithMessage("Last name is required")
                .MaximumLength(50)
                .WithMessage("Last name must not exceed 50 characters");
            RuleFor(au => au.Date_of_birth)
                .NotNull()
                .NotEmpty()
                .WithMessage("Date of birth is required");
            RuleFor(au => au.Biography)
                .MaximumLength(500)
                .WithMessage("Biography must not exceed 500 characters");
            RuleFor(au => au.Country)
                .MaximumLength(50)
                .WithMessage("Country must not exceed 50 characters");
        }
    }
}