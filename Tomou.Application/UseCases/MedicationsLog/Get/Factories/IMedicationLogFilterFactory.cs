using Tomou.Communication.Requests.MedicationsLog.MedicationLogQuery;
using Tomou.Domain.Repositories.MedicatioLog;

namespace Tomou.Application.UseCases.MedicationsLog.Get.Factories;

public interface IMedicationLogFilterFactory
{
    MedicationLogFilter Create(MedicationLogQuery query, Guid ownerId, bool isCaregiver);
}
