using FluentValidation;
using Profiles.Application.DTOs;

namespace Profiles.Application.Validators;

public class UpdateDoctorValidator : AbstractValidator<UpdateDoctorDto>
{
    public UpdateDoctorValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Please, enter the first name");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Please, enter the last name");
        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Date of birth cannot be in the future.");
        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Please, choose the specialisation");
        RuleFor(x => x.OfficeAddress)
            .NotEmpty().WithMessage("Please, choose the office");
        RuleFor(x => x.CareerStartYear)
            .NotEmpty().WithMessage("Please, select the year")
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Career start year cannot be in the future.");
        RuleFor(x => x.Status)
            .Must(BeStatus).WithMessage("Invalid status.")
            .When(x => !string.IsNullOrEmpty(x.Status));
    }

    public bool BeStatus(string status)
    {
        var validStatuses = new[] { "At work", "On vacation", "Sick Day", "Sick Leave", "Self-isolation", "Leave without pay", "Inactive" };
        return validStatuses.Contains(status);
    }
}