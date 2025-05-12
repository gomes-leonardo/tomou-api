using Tomou.Domain.Repositories.UnitOfWork;

namespace Tomou.Infrastructure.DataAccess.UnitOfWork;
internal class UnitOfWork : IUnitOfWork
{
    private readonly TomouDbContext _dbContext;
    public UnitOfWork(TomouDbContext dbContext)
    {
        _dbContext = dbContext; 
    }
    public async Task Commit()
    {
        await _dbContext.SaveChangesAsync();
    }
}
