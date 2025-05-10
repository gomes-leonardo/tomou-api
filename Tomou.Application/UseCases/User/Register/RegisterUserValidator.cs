using FluentValidation;
using Tomou.Communication.Requests.User;
using Tomou.Exception;

namespace Tomou.Application.UseCases.User.Register;
public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(u => u.Name).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_NAME);
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_EMAIL)
            .EmailAddress().WithMessage(ResourceErrorMessages.INVALID_EMAIL);

        Include(new PasswordValidator<RequestRegisterUserJson>(x => x.Password));
    }
}
