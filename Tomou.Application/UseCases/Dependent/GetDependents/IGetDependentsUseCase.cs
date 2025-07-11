using Tomou.Communication.Responses.Dependent.Get;

namespace Tomou.Application.UseCases.Dependent.GetAll;
public interface IGetDependentsUseCase
{
    Task<ResponseDependentsJson> Execute(string? nameFilter = null, bool ascending = true);
}
