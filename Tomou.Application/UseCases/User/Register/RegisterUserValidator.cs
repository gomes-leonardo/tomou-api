using FluentValidation;
using Tomou.Communication.Requests.User.Register;
using Tomou.Exception;

namespace Tomou.Application.UseCases.User.Register;
public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(u => u.Name).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_NAME);


        Include(new EmailValidator<RequestRegisterUserJson>(x => x.Email));
        Include(new PasswordValidator<RequestRegisterUserJson>(x => x.Password));
    }
}
