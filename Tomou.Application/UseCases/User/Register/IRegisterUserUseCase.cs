using Tomou.Communication.Requests.User.Register;
using Tomou.Communication.Responses.User.Register;

namespace Tomou.Application.UseCases.User.Register;
public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
}
