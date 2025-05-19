using FluentValidation;
using FluentValidation.Validators;
using Tomou.Application.UseCases.User;
using Tomou.Application.UseCases.User.Validators.Common;
using Tomou.Communication.Requests;
using Tomou.Communication.Requests.User.ForgotPassword;
using Tomou.Communication.Requests.User.Login;

namespace Tomou.Application.Validators.User;

public class ForgotPasswordValidator : AbstractValidator<RequestForgotPasswordJson>
{
    public ForgotPasswordValidator()
    {
        Include(new EmailFieldValidator<RequestForgotPasswordJson>(x => x.Email));
    }
}
