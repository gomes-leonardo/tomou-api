using FluentValidation;
using System.Linq.Expressions;
using Tomou.Exception;

namespace Tomou.Application.UseCases.Medications.Validators.Common;

public class RegisterOrUpdateMedicationValidator<T> : AbstractValidator<T>
{
    public RegisterOrUpdateMedicationValidator(
        Expression<Func<T, string>> nameSelector,
        Expression<Func<T, string>> dosageSelector,
        Expression<Func<T, IEnumerable<string>>> timesToTakeSelector,
        Expression<Func<T, IEnumerable<string>>> daysOfWeek,
        Expression<Func<T, DateTime>> startDateSelector,
        Expression<Func<T, DateTime>> endDateSelector)
    {
        RuleFor(nameSelector)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_NAME);

        RuleFor(dosageSelector)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_DOSAGE);

        RuleFor(timesToTakeSelector)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_TIME_TO_TAKE);

        RuleForEach<string>(timesToTakeSelector)
            .Must(BeValidTime)
            .WithMessage(ResourceErrorMessages.INVALID_TIME_FORMAT);

        RuleForEach<string>(daysOfWeek)
            .Must(BeValidDayOfWeek)
            .WithMessage(ResourceErrorMessages.INVALID_DAY_OF_WEEK);

        RuleFor(startDateSelector)
            .LessThanOrEqualTo(endDateSelector)
            .WithMessage(ResourceErrorMessages.START_MUST_BE_BEFORE_END);
    }
    private bool BeValidTime(string time)
    {
        return TimeOnly.TryParse(time, out _);
    }

    private bool BeValidDayOfWeek (string day)
    {
        return Enum.TryParse<DayOfWeek>(day, ignoreCase: true, out _);
    }
}
