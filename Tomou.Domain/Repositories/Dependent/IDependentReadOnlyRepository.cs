using Tomou.Domain.Repositories.Dependent.Filters;

namespace Tomou.Domain.Repositories.Dependent;
public interface IDependentReadOnlyRepository
{
    Task<IReadOnlyList<Entities.Dependent>> GetByCaregiverId(DependentFilter filter);
    Task<Entities.Dependent?> GetByIdAsync(Guid id);

}
