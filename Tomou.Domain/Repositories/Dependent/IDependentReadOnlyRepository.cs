namespace Tomou.Domain.Repositories.Dependent;
public interface IDependentReadOnlyRepository
{
    Task<List<Entities.Dependent>> GetByCaregiverId(long caregiverId, string? nameFilter = null, bool ascending = true);
    Task<Entities.Dependent?> GetByIdAsync(long id);
}
