using Tomou.Communication.Responses.MedicationLog.Get;

namespace Tomou.Application.UseCases.MedicationsLog.GetById;
public interface IGetMedicationLogByIdUseCase
{
    public Task<ResponseMedicationLogShortJson> Execute(Guid id, Guid medicationLogId);
}
