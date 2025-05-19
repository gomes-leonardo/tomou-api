using FluentValidation;
using Tomou.Exception;

namespace Tomou.Application.UseCases.User;
public class EmailValidator<T> : AbstractValidator<T>
{

    public EmailValidator(System.Linq.Expressions.Expression<Func<T, string>> emailSelector)
    {
        RuleFor(emailSelector)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_EMAIL)
            .EmailAddress()
            .WithMessage(ResourceErrorMessages.INVALID_EMAIL);
    }
}
