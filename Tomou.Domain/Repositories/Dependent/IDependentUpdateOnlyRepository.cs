namespace Tomou.Domain.Repositories.Dependent;
public interface IDependentUpdateOnlyRepository
{
    void UpdateAsync(Entities.Dependent dependent);
}
