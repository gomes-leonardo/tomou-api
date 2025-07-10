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
        RuleFor(m => m.Dosage).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_DOSAGE);
        RuleFor(m => m.TimeToTake).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_TIME_TO_TAKE)
            .Must(BeValidTime).WithMessage(ResourceErrorMessages.INVALID_TIME_FORMAT);



    }
    private bool BeValidTime(string time)
    {
        return TimeSpan.TryParse(time, out _);
    }

}
