using Tomou.Communication.Responses.MedicationLog.Get;

namespace Tomou.Application.UseCases.MedicationsLog.Get;
public interface IGetMedicationsLogUseCase
{
    Task<ResponseMedicationsLogJson> Execute(MedicationLogFilter filter);
}
