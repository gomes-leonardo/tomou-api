using Microsoft.EntityFrameworkCore;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Infrastructure.DataAccess;

namespace Tomou.Infrastructure.Repositories.Dependent;
internal class DependentRepository : IDependentWriteOnlyRepository, IDependentReadOnlyRepository, IDependentUpdateOnlyRepository
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

    async Task<Domain.Entities.Dependent?> IDependentUpdateOnlyRepository.GetById(long id)
    {
        return await _dbContext.Dependents.FirstOrDefaultAsync(dependent => dependent.Id.Equals(id));
    }

    public void Update(Domain.Entities.Dependent dependent)
    {
        _dbContext.Dependents.Update(dependent);
    }

    public async Task<bool> Delete(long id)
    {
        var result = await _dbContext.Dependents.FirstOrDefaultAsync(dependent => dependent.Id.Equals(id));

        if (result == null)
        {
            return false;
        }

        _dbContext.Dependents.Remove(result);
        return true;
    }

    public async Task<Domain.Entities.Dependent?> GetByIdAsync(long id)
    {
        return await _dbContext.Dependents
            .AsNoTracking().
            FirstOrDefaultAsync(d => d.Id == id);
    }
}
