using Tomou.Communication.Requests.User.ForgotPassword;

namespace Tomou.Application.UseCases.User.ForgotPassword;
public interface IForgotPasswordUseCase
{
    Task Execute(RequestForgotPasswordJson request);
}
