using Profiles.Application.DTOs;
using FluentValidation;

namespace Profiles.Application.Validators;

public class CreatePatientValidator : AbstractValidator<CreatePatientDto>
{
    public CreatePatientValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Please, enter the first name");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Please, enter the last name");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Please, enter the phone number")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("You've entered an invalid phone number");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Please, select the date")
            .LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Date of birth cannot be in the future.");
    }
}


