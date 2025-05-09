using Tomou.Communication.Requests.User;
using Tomou.Communication.Responses.User;

namespace Tomou.Application.UseCases.User;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        throw new NotImplementedException();
    }
}
