namespace Tomou.Domain.Repositories.Dependent;
public interface IDependentUpdateOnlyRepository
{
    Task<Entities.Dependent?> GetById(Guid id);
    void Update(Entities.Dependent dependent);
}
