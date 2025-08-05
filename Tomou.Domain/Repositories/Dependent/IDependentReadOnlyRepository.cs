namespace Tomou.Domain.Repositories.Dependent;
public interface IDependentReadOnlyRepository
{
    Task<IReadOnlyList<Entities.Dependent>> GetByCaregiverId(Guid caregiverId, string? nameFilter = null, bool ascending = true);
    Task<Entities.Dependent?> GetByIdAsync(Guid id);

}
