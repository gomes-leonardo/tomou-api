using Tomou.Domain.Repositories.Medications;
using Tomou.Infrastructure.DataAccess;

namespace Tomou.Infrastructure.Repositories.Medications;
internal class MedicationsRepository : IMedicationsWriteOnlyRepository
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
}
