using FluentValidation;
using InnoClinic.Profiles.Api.DTOs;

namespace InnoClinic.Profiles.Api.Validators;

public class UpdateReceptionistRequestValidator : AbstractValidator<ReceptionistUpdateRequest>
{
    public UpdateReceptionistRequestValidator()
    {
       RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Please, enter the first name");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Please, enter the last name");
        RuleFor(x => x.Office)
            .NotEmpty().WithMessage("Please, choose the office");
    }
}

