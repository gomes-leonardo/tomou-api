namespace Tomou.Domain.Repositories.UnitOfWork;
public interface IUnitOfWork
{
    Task Commit();
}
