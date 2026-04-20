using FluentValidation;
using Profiles.Application.DTOs;

namespace Profiles.Application.Validators;

public class UpdateReceptionistValidator : AbstractValidator<UpdateReceptionistDto>
{
    public UpdateReceptionistValidator()
    {
       RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Please, enter the first name");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Please, enter the last name");
        RuleFor(x => x.OfficeAddress)
            .NotEmpty().WithMessage("Please, choose the office");
    }
}

