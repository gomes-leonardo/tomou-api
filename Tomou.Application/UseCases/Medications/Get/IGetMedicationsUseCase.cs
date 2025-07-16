using Tomou.Communication.Responses.Medications.Get;

namespace Tomou.Application.UseCases.Medications.Get;
public interface IGetMedicationsUseCase
{
    Task<ResponseMedicationsJson> Execute(Guid? userOrDependentId, string? nameFilter = null, bool ascending = true);
}
