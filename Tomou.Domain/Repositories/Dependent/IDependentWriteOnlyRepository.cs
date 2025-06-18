namespace Tomou.Domain.Repositories.Dependent;
public interface IDependentWriteOnlyRepository
{
    Task Add(Entities.Dependent dependent);

}
