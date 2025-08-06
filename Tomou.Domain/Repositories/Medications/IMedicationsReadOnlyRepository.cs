using Tomou.Domain.Repositories.Medications.Filters;

namespace Tomou.Domain.Repositories.Medications;
public interface IMedicationsReadOnlyRepository
{
    Task<List<Entities.Medication>> GetMedicationsByOwner(MedicationsFilter filter);
    Task<Entities.Medication?> GetMedicationsById(MedicationsFilterById filter);
}
