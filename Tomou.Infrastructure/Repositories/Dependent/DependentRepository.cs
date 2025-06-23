using Microsoft.EntityFrameworkCore;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Infrastructure.DataAccess;

namespace Tomou.Infrastructure.Repositories.Dependent;
internal class DependentRepository : IDependentWriteOnlyRepository, IDependentReadOnlyRepository
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

    public async Task<List<Domain.Entities.Dependent>> GetByCaregiverId(long caregiverId)
    {
        return await _dbContext.Dependents
         .AsNoTracking()
         .Where(d => d.CaregiverId == caregiverId)
         .ToListAsync();
    }
}
