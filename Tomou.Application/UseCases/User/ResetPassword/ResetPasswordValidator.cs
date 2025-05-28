using FluentValidation;
using Tomou.Application.UseCases.User.Validators.Common;
using Tomou.Communication.Requests.User.ForgotPassword;
using Tomou.Exception;

namespace Tomou.Application.UseCases.User.ResetPassword;
public class ResetPasswordValidator : AbstractValidator<RequestResetPasswordJson>
{
    public ResetPasswordValidator()
    {
        RuleFor(u => u.Token)
            .NotEmpty()
            .MinimumLength(5)
            .WithMessage(ResourceErrorMessages.INVALID_TOKEN);
        Include(new PasswordValidator<RequestResetPasswordJson>(x => x.NewPassword));
    }
}

