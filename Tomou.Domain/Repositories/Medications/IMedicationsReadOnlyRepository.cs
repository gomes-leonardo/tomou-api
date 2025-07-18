namespace Tomou.Domain.Repositories.Medications;
public interface IMedicationsReadOnlyRepository
{
    Task<List<Entities.Medication>> GetMedications(Guid id, bool isCaregiver, string? nameFilter = null, bool ascending = true);
    Task<Entities.Medication?> GetMedicationsById(Guid id, bool isCaregiver, Guid medicamentId);
}
