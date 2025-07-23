using FluentValidation;
using Tomou.Application.UseCases.Medications.Validators.Common;
using Tomou.Communication.Requests.Medications.Update;

namespace Tomou.Application.UseCases.Medications.Update;
public class UpdateMedicationValidator : AbstractValidator<RequestUpdateMedicationJson>
{
    public UpdateMedicationValidator()
    {
        Include(new RegisterOrUpdateMedicationValidator<RequestUpdateMedicationJson>(
             x => x.Name,
             x => x.Dosage,
             x => x.TimesToTake,
             x => x.DaysOfWeek,
             x => x.StartDate.ToDateTime(new TimeOnly()),
             x => x.EndDate.ToDateTime(new TimeOnly())
         ));
    }
}

