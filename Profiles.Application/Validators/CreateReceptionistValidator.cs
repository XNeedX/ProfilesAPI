using FluentValidation;
using Profiles.Application.DTOs;

namespace Profiles.Application.Validators;

public class CreateReceptionistValidator : AbstractValidator<CreateReceptionistRequest>
{
    public CreateReceptionistValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Please, enter the first name");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Please, enter the last name");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Please, enter the email")
            .EmailAddress().WithMessage("You've entered an invalid email");

        RuleFor(x => x.OfficeAddress)
            .NotEmpty().WithMessage("Please, choose the office");
    }
}