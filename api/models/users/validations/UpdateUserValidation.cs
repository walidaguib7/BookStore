
using api.models.users.dtos;
using FluentValidation;

namespace api.models.users.validations
{
    public class UpdateUserValidation : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidation()
        {
            RuleFor(u => u.Username)
                .NotNull().NotEmpty().Length(8, 20).WithMessage("username must have between 8 and 20 letters");

            RuleFor(u => u.First_name).NotNull().NotEmpty();

            RuleFor(u => u.Last_Name).NotNull().NotEmpty();

            RuleFor(u => u.Bio).NotEmpty().NotNull();

            RuleFor(u => u.Gender).NotEmpty().NotNull();

            RuleFor(u => u.Age).GreaterThan(10).NotNull();
        }
    }
}