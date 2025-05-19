using FluentValidation;
using Tomou.Exception;

namespace Tomou.Application.UseCases.User.Validators.Common;
public class PasswordValidator<T> : AbstractValidator<T>
{

    private const string PasswordRegex = @"^(?=.*[A-Z])(?=.*[!@#$%^&*()\-_=+{}[\]|\\:;""'<>,.?/~`]).{8,}$";
    public PasswordValidator(System.Linq.Expressions.Expression<Func<T, string>> passwordSelector)
    {
        RuleFor(passwordSelector)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_PASSWORD)
            .Matches(PasswordRegex).WithMessage(ResourceErrorMessages.INVALID_PASSWORD);
    }
}
