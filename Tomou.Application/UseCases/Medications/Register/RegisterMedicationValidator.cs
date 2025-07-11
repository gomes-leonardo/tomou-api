using FluentValidation;
using Tomou.Application.UseCases.Dependent.Validators.Common;
using Tomou.Communication.Requests.Medications.Register;
using Tomou.Exception;

namespace Tomou.Application.UseCases.Medications.Register;

public class RegisterMedicationValidator : AbstractValidator<RequestRegisterMedicationsJson>
{
    public RegisterMedicationValidator()
    {
        Include(new NameValidator<RequestRegisterMedicationsJson>(x => x.Name));

        RuleFor(m => m.Dosage)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_DOSAGE);

        RuleFor(m => m.TimesToTake)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_TIME_TO_TAKE);

        RuleForEach(m => m.TimesToTake)
            .Must(BeValidTime)
            .WithMessage(ResourceErrorMessages.INVALID_TIME_FORMAT);

        RuleFor(m => m.StartDate)
            .LessThan(m => m.EndDate)
            .WithMessage(ResourceErrorMessages.START_MUST_BE_BEFORE_END);
    }

    private bool BeValidTime(string time)
    {
        return TimeOnly.TryParse(time, out _);
    }
}
