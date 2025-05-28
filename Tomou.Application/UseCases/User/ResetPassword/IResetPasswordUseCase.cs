using Tomou.Communication.Requests.User.ForgotPassword;

namespace Tomou.Application.UseCases.User.ResetPassword;
public interface IResetPasswordUseCase
{
    Task Execute(RequestResetPasswordJson request);
}
