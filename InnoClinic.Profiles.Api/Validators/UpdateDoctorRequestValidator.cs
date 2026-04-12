using FluentValidation;
using InnoClinic.Profiles.Api.DTOs;

namespace InnoClinic.Profiles.Api.Validators;

public class UpdateDoctorRequestValidator : AbstractValidator<DoctorUpdateRequest>
{
    public UpdateDoctorRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Please, enter the first name");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Please, enter the last name");
        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Date of birth cannot be in the future.");
        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Please, choose the specialisation");
        RuleFor(x => x.Office)
            .NotEmpty().WithMessage("Please, choose the office");
        RuleFor(x => x.CareerStartYear)
            .NotEmpty().WithMessage("Please, select the year")
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Career start year cannot be in the future.");
        RuleFor(x => x.Status)
            .Must(BeStatus).WithMessage("Invalid status.")
            .When(x => x.Status != null);
    }

    public bool BeStatus(string status)
    {
        var validStatuses = new[] { "At work", "On vacation", "Sick Day", "Sick Leave", "Self-isolation", "Leave without pay", "Inactive" };
        return validStatuses.Contains(status);
    }
}