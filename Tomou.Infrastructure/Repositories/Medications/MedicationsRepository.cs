using Microsoft.EntityFrameworkCore;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.Medications.Filters;
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

    public async Task<bool> Delete(Guid medicamentId)
    {
        var result = await _dbContext.Medications.FirstOrDefaultAsync(m => m.Id == medicamentId);

        if (result is null)
        {
            return false;
        }

        _dbContext.Medications.Remove(result);
        return true;
    }

    public Task<List<Medication>> GetMedicationsByOwner(MedicationsFilter filter)
    {
        var query = _dbContext.Medications.AsNoTracking();

        query = filter.IsCaregiver
        ? query.Where(m => m.DependentId == filter.OwnerId)
        : query.Where(m => m.UserId == filter.OwnerId);

        if (!string.IsNullOrEmpty(filter.NameContains))
        {
            query = query.Where(m =>
                EF.Functions.Like(m.Name, $"%{filter.NameContains}%"));
        }

        query = filter.Ascending ? query.OrderBy(m => m.Name)
            : query.OrderByDescending(m => m.Name);

        return query.ToListAsync();

    }

    public Task<Medication?> GetMedicationsById(MedicationsFilterById filter)
    {
        var query = _dbContext.Medications.AsNoTracking();
        query = filter.IsCaregiver
          ? query.Where(m => m.DependentId == filter.Id && m.Id == filter.MedicamentId)
          : query.Where(m => m.UserId == filter.Id && m.Id == filter.MedicamentId);
        return query.SingleOrDefaultAsync();
    }

    public void Update(Domain.Entities.Medication medication)
    {
        _dbContext.Medications.Update(medication);
    }
}
