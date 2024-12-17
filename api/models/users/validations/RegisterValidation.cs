using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.users.dtos;
using FluentValidation;

namespace api.models.users.validations
{
    public class RegisterValidation : AbstractValidator<RegisterDto>
    {
        public RegisterValidation()
        {
            RuleFor(u => u.email)
                .EmailAddress().NotEmpty().NotNull().WithMessage("please write a valid email");
            RuleFor(u => u.username)
                .NotNull().NotEmpty().Length(8, 20)
                .WithMessage("username must have between 8 and 20 letters");
            RuleFor(u => u.password)
                .NotNull().NotEmpty().MinimumLength(12)
                .WithMessage("password must have 12 letters");
            RuleFor(u => u.role)
                .NotNull().IsInEnum();
        }
    }
}