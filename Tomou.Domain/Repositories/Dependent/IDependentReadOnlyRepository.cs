namespace Tomou.Domain.Repositories.Dependent;
public interface IDependentReadOnlyRepository
{
    Task<List<Entities.Dependent>> GetByCaregiverId(long caregiverId);
}
