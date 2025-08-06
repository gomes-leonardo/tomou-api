using Microsoft.EntityFrameworkCore;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.Dependent.Filters;
using Tomou.Infrastructure.DataAccess;

namespace Tomou.Infrastructure.Repositories.Dependent;
internal class DependentRepository : IDependentWriteOnlyRepository, IDependentReadOnlyRepository, IDependentUpdateOnlyRepository
{
    private readonly TomouDbContext _dbContext;

    public DependentRepository(TomouDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddAsync(Domain.Entities.Dependent dependent)
    {
        await _dbContext.Dependents.AddAsync(dependent);
    }

    public async Task<IReadOnlyList<Domain.Entities.Dependent>> GetByCaregiverId(
       DependentFilter filter)
    {
        var query = _dbContext.Dependents
        .AsNoTracking()
        .Where(d => d.CaregiverId == filter.CaregiverId);

        if (!string.IsNullOrEmpty(filter.NameContains))
        {
             query = query.Where(d =>
             EF.Functions.Like(d.Name, $"%{filter.NameContains}%"));
        }

        query = filter.Ascending
            ? query.OrderBy(d => d.Name)
            : query.OrderByDescending(d => d.Name);

        return await query.ToListAsync();
    }

    public void UpdateAsync(Domain.Entities.Dependent dependent)
    {
        _dbContext.Dependents.Update(dependent);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _dbContext.Dependents.FirstOrDefaultAsync(dependent => dependent.Id == id);

        if (result is null)
            return false;

        _dbContext.Dependents.Remove(result);
        return true;
    }

    public async Task<Domain.Entities.Dependent?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Dependents
            .AsNoTracking().
            FirstOrDefaultAsync(d => d.Id == id);
    }
}
