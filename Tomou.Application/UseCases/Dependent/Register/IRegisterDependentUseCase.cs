using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Responses.Dependent.Register;

namespace Tomou.Application.UseCases.Dependent.Register;
public interface IRegisterDependentUseCase
{
    Task<ResponseCreateDependentJson> Execute(long caregiverId, RequestRegisterDependentJson request);
}
