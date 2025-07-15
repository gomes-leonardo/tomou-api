using Tomou.Communication.Responses.Medications.Get;

namespace Tomou.Application.UseCases.Medications.Get;
public interface IGetMedicationsUseCase
{
    Task<ResponseMedicationsJson> Execute(long? userOrDependentId, string? nameFilter = null, bool ascending = true);
}
