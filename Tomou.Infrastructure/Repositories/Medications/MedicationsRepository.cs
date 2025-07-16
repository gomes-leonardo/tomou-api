using Microsoft.EntityFrameworkCore;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.Medications;
using Tomou.Infrastructure.DataAccess;

namespace Tomou.Infrastructure.Repositories.Medications;
internal class MedicationsRepository : IMedicationsWriteOnlyRepository, IMedicationsReadOnlyRepository
{
    private readonly TomouDbContext _dbContext;

    public MedicationsRepository(TomouDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(Tomou.Domain.Entities.Medication medication)
    {
        await _dbContext.Medications.AddAsync(medication);
    }

    public Task<List<Medication>> GetMedications(Guid userOrDependentId, bool isCaregiver, string? nameFilter = null, bool ascending = true)
    {
        var query = _dbContext.Medications.AsNoTracking();

        query = isCaregiver
        ? query.Where(m => m.DependentId == userOrDependentId)
        : query.Where(m => m.UserId == userOrDependentId);

        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(m =>
                EF.Functions.Like(m.Name, $"%{nameFilter}%"));
        }

        query = ascending ? query.OrderBy(m => m.Name)
            : query.OrderByDescending(m => m.Name);

        return query.ToListAsync();

    }
}
