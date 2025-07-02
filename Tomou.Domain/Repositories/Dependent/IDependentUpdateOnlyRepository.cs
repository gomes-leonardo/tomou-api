namespace Tomou.Domain.Repositories.Dependent;
public interface IDependentUpdateOnlyRepository
{
    Task<Entities.Dependent?> GetById(long id);
    void Update(Entities.Dependent dependent);
}
