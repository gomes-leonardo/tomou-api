using FluentValidation;
using Tomou.Application.UseCases.User.Validators.Common;
using Tomou.Communication.Requests.User.ForgotPassword;

namespace Tomou.Application.Validators.User;

public class ForgotPasswordValidator : AbstractValidator<RequestForgotPasswordJson>
{
    public ForgotPasswordValidator()
    {
        Include(new EmailFieldValidator<RequestForgotPasswordJson>(x => x.Email));
    }
}
