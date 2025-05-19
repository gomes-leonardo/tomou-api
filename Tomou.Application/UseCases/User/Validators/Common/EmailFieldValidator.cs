using FluentValidation;
using Tomou.Exception;

namespace Tomou.Application.UseCases.User.Validators.Common;
public class EmailFieldValidator<T> : AbstractValidator<T>
{

    public EmailFieldValidator(System.Linq.Expressions.Expression<Func<T, string>> emailSelector)
    {
        RuleFor(emailSelector)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_EMAIL)
            .EmailAddress()
            .WithMessage(ResourceErrorMessages.INVALID_EMAIL);
    }
}
