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

    public Task<List<Medication>> GetMedications(Guid id, bool isCaregiver, string? nameFilter = null, bool ascending = true)
    {
        var query = _dbContext.Medications.AsNoTracking();

        query = isCaregiver
        ? query.Where(m => m.DependentId == id)
        : query.Where(m => m.UserId == id);

        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(m =>
                EF.Functions.Like(m.Name, $"%{nameFilter}%"));
        }

        query = ascending ? query.OrderBy(m => m.Name)
            : query.OrderByDescending(m => m.Name);

        return query.ToListAsync();

    }

    public Task<Medication?> GetMedicationsById(Guid id, bool isCaregiver, Guid medicationId)
    {
        var query = _dbContext.Medications.AsNoTracking();
        query = isCaregiver
          ? query.Where(m => m.DependentId == id && m.Id == medicationId)
          : query.Where(m => m.UserId == id && m.Id == medicationId);
        return query.SingleOrDefaultAsync();
    }

    public void Update(Domain.Entities.Medication medication)
    {
        _dbContext.Medications.Update(medication);
    }
}
