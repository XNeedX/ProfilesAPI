using FluentValidation;
using InnoClinic.Profiles.Api.DTOs;

namespace InnoClinic.Profiles.Api.Validators;

public class CreateDoctorRequestValidator : AbstractValidator<DoctorProfileRegistrationRequest>
{
    public CreateDoctorRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Please, enter the first name");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Please, enter the last name");
        
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Please, select the date")
            .LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Date of birth cannot be in the future.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Please, enter the email")
            .EmailAddress().WithMessage("You've entered an invalid email");

        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Please, choose the specialisation");

        RuleFor(x => x.Office)
            .NotEmpty().WithMessage("Please, choose the office");

        RuleFor(x => x.CareerStartYear)
            .NotEmpty().WithMessage("Please, select the year")
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Career start year cannot be in the future.");

        RuleFor(x => x.Status)
            .Must(BeStatus).WithMessage("Invalid status.");
    }

    public bool BeStatus(string status)
    {
        var validStatuses = new[] { "At work", "On vacation", "Sick Day", "Sick Leave", "Self-isolation", "Leave without pay", "Inactive" };
        return validStatuses.Contains(status);
    }
}