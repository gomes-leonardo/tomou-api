using FluentValidation;
using Tomou.Application.UseCases.Dependent.Validators.Common;
using Tomou.Application.UseCases.Medications.Validators.Common;
using Tomou.Communication.Requests.Medications.Register;
using Tomou.Exception;

namespace Tomou.Application.UseCases.Medications.Register;

public class RegisterMedicationValidator : AbstractValidator<RequestRegisterMedicationsJson>
{
    public RegisterMedicationValidator()
    {
       Include(new RegisterOrUpdateMedicationValidator<RequestRegisterMedicationsJson>(
            x => x.Name,
            x => x.Dosage,
            x => x.TimesToTake,
            x => x.DaysOfWeek,
            x => x.StartDate.ToDateTime(new TimeOnly()),
            x => x.EndDate.ToDateTime(new TimeOnly())
        ));
    }
}
