namespace Tomou.Domain.Repositories.Dependent;

public interface IDependentWriteOnlyRepository
{
    Task AddAsync(Tomou.Domain.Entities.Dependent dependent);
    Task<bool> DeleteAsync(Guid id);
}
