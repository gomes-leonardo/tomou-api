using Tomou.Communication.Requests.MedicationsLog.MedicationLogQuery;
using Tomou.Domain.Repositories.MedicatioLog;
using DStatus = Tomou.Domain.Enums.MedicationLogStatus;
using CStatus = Tomou.Communication.Enums.MedicationLog.MedicationLogStatus;

namespace Tomou.Application.UseCases.MedicationsLog.Get.Factories;

public sealed class MedicationLogFilterFactory : IMedicationLogFilterFactory
{

    public MedicationLogFilter Create(MedicationLogQuery query, Guid ownerId, bool isCaregiver)
    {
        var ascending = string.IsNullOrWhiteSpace(query.Order)
            || query.Order.Equals("asc", StringComparison.OrdinalIgnoreCase);

        DStatus? status = null;
        if (query.Status.HasValue)
        {
            status = (DStatus)(int)query.Status.Value;
        }

        return new MedicationLogFilter(
            ownerId: ownerId,
            isCaregiver: isCaregiver,
            medicationId: query.MedicationId,
            status: status,
            scheduledFrom: query.ScheduledFrom,
            scheduledTo: query.ScheduledTo,
            takenFrom: query.TakenFrom,
            takenTo: query.TakenTo,
            onlyOverdue: query.OnlyOverdue,
            isSnoozed: query.IsSnoozed,
            nameContains: query.NameContains,
            ascending: ascending,
            page: query.Page,
            pageSize: query.PageSize
        );
    }
}
