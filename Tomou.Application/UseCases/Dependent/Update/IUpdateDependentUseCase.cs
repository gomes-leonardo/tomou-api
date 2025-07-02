using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Responses.Dependent.Update;

namespace Tomou.Application.UseCases.Dependent.Update;
public interface IUpdateDependentUseCase
{
    Task<ResponseUpdatedDependentJson> Execute(RequestUpdateDependentJson request, long id);
}
