namespace Tomou.Domain.Repositories.Medications;
public interface IMedicationsReadOnlyRepository
{
    Task<List<Entities.Medication>> GetMedications(long userOrDependentId, bool isCaregiver, string? nameFilter = null, bool ascending = true);
}
