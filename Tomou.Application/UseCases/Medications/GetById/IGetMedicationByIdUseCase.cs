using Tomou.Communication.Responses.Medications.Get;

namespace Tomou.Application.UseCases.Medications.GetById;
public interface IGetMedicationByIdUseCase
{
    Task<ResponseMedicationShortJson> Execute(Guid? id, Guid medicamentId);
}
