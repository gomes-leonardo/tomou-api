using FluentValidation;
using Tomou.Communication.Requests.MedicationsLog.MedicationLogQuery;
using Tomou.Exception;

namespace Tomou.Application.UseCases.MedicationsLog.Get.Validators;

public class MedicationLogQueryValidator : AbstractValidator<MedicationLogQuery>
{
    public MedicationLogQueryValidator()
    {
        RuleFor(q => q.Page).GreaterThanOrEqualTo(1).WithMessage(ResourceErrorMessages.INVALID_PAGE);
        RuleFor(q => q.PageSize).InclusiveBetween(1, 100).WithMessage(ResourceErrorMessages.INVALID_PAGE_SIZE);
        RuleFor(q => q.Order)
            .Must(o => string.IsNullOrWhiteSpace(o) || o.Equals("asc", StringComparison.OrdinalIgnoreCase) || o.Equals("desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage(ResourceErrorMessages.INVALID_ORDER);

        RuleFor(q => q.ScheduledTo)
            .Must((q, to) => to is null || q.ScheduledFrom is null || q.ScheduledFrom <= to)
            .WithMessage(ResourceErrorMessages.INVALID_DATE_RANGE);

        RuleFor(q => q.TakenTo)
            .Must((q, to) => to is null || q.TakenFrom is null || q.TakenFrom <= to)
            .WithMessage(ResourceErrorMessages.INVALID_DATE_RANGE);
    }
}
