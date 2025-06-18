using Tomou.Domain.Repositories.Dependent;
using Tomou.Infrastructure.DataAccess;

namespace Tomou.Infrastructure.Repositories.Dependent;
internal class DependentRepository : IDependentWriteOnlyRepository
{
    private readonly TomouDbContext _dbContext;

    public DependentRepository(TomouDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(Domain.Entities.Dependent dependent)
    {
        await _dbContext.Dependents.AddAsync(dependent);
    }
}
