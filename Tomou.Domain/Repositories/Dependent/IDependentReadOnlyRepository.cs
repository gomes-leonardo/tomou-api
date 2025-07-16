namespace Tomou.Domain.Repositories.Dependent;
public interface IDependentReadOnlyRepository
{
    Task<List<Entities.Dependent>> GetDependents(Guid caregiverId, string? nameFilter = null, bool ascending = true);
    Task<Entities.Dependent?> GetByIdAsync(Guid id);
}
