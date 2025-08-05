namespace Tomou.Domain.Repositories.MedicatioLog;
public interface IMedicationsLogReadOnlyRepository
{
    Task<Tomou.Domain.Entities.MedicationLog> GetMedicationLog(Guid id, bool isCaregiver, string? nameFilter = null, bool ascending = true);
    Task<Tomou.Domain.Entities.MedicationLog> GetMedicationLogById(Guid id, bool isCaregiver, Guid medicationLogId);
}
