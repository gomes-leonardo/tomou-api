using Tomou.Communication.Requests.User.Login;
using Tomou.Communication.Responses.User.Login;

namespace Tomou.Application.UseCases.User.Login;
public interface IDoLoginUseCase
{
    Task<ResponseLoggedUserJson> Execute(RequestLoginUserJson request);
}
