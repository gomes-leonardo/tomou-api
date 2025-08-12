namespace Tomou.Domain.Repositories.MedicatioLog;
public interface IMedicationsLogReadOnlyRepository
{
    Task<List<Tomou.Domain.Entities.MedicationLog>> GetMedicationLog(MedicationLogFilter filter);
    Task<Tomou.Domain.Entities.MedicationLog?> GetMedicationLogById(Guid id, bool isCaregiver, Guid medicationLogId);
}
