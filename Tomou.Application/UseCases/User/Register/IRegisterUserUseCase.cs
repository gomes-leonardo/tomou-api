using Tomou.Communication.Requests.User;
using Tomou.Communication.Responses.User;

namespace Tomou.Application.UseCases.User.Register;
public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
}
