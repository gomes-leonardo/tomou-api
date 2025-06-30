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

    public async Task<List<Domain.Entities.Dependent>> GetByCaregiverId(
        long caregiverId,
        string? nameFilter = null,
        bool ascending = true)
    {
        var query = _dbContext.Dependents
        .AsNoTracking()
        .Where(d => d.CaregiverId == caregiverId);

        if (!string.IsNullOrEmpty(nameFilter))
        {
             query = query.Where(d =>
             EF.Functions.Like(d.Name, $"%{nameFilter}%"));
        }

        query = ascending
            ? query.OrderBy(d => d.Name)
            : query.OrderByDescending(d => d.Name);

        return await query.ToListAsync();
    }

   

    
}
