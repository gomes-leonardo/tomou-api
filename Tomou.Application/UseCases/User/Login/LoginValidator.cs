using FluentValidation;
using Tomou.Communication.Requests.User.Login;
using Tomou.Exception;

namespace Tomou.Application.UseCases.User.Login;
public class LoginValidator : AbstractValidator<RequestLoginUserJson>
{
    public LoginValidator()
    {
        RuleFor(user => user.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_EMAIL)
            .EmailAddress()
            .WithMessage(ResourceErrorMessages.INVALID_EMAIL);
        RuleFor(user => user.Password).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_PASSWORD);
    }
}
