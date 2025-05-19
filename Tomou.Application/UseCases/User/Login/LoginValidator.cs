using FluentValidation;
using Tomou.Application.UseCases.User.Validators.Common;
using Tomou.Communication.Requests.User.Login;
using Tomou.Communication.Requests.User.Register;
using Tomou.Exception;

namespace Tomou.Application.UseCases.User.Login;
public class LoginValidator : AbstractValidator<RequestLoginUserJson>
{
    public LoginValidator()
    {
        Include(new EmailFieldValidator<RequestLoginUserJson>(x => x.Email));
        RuleFor(user => user.Password).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_PASSWORD);
    }
}
