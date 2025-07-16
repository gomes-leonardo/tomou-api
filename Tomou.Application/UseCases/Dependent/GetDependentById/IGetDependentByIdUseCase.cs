using Tomou.Communication.Responses.Dependent.Get;

namespace Tomou.Application.UseCases.Dependent.GetDependentById;
public interface IGetDependentByIdUseCase
{
    Task<ResponseDependentShortJson> Execute(Guid id);
}
