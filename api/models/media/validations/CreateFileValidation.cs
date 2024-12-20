using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.media.dtos;
using FluentValidation;

namespace api.models.media.validations
{
    public class CreateFileValidation : AbstractValidator<CreateFileDto>
    {
        public CreateFileValidation()
        {
            RuleFor(x => x.file).NotEmpty().WithMessage("File is required");
            RuleFor(x => x.type).NotEmpty().WithMessage("Type is required");
        }
    }
}