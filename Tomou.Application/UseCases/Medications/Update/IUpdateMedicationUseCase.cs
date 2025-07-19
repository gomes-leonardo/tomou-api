using Tomou.Communication.Requests.Medications.Update;
using Tomou.Communication.Responses.Medications.Update;

namespace Tomou.Application.UseCases.Medications.Update;
public interface IUpdateMedicationUseCase
{
    public Task<ResponseUpdatedMedicationJson> Execute(Guid? id, Guid medicamentId, RequestUpdateMedicationJson request);
}
