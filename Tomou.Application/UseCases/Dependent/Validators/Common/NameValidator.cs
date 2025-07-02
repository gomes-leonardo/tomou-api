using FluentValidation;
using Tomou.Exception;

namespace Tomou.Application.UseCases.Dependent.Validators.Common;
public class NameValidator<T> : AbstractValidator<T>
{
    public NameValidator(System.Linq.Expressions.Expression<Func<T, string>> nameSelector)
    {
        RuleFor(nameSelector)
            .NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_NAME);
    }
}
