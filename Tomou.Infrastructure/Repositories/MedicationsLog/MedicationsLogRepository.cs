

using Microsoft.EntityFrameworkCore;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.MedicatioLog;
using Tomou.Infrastructure.DataAccess;

internal class MedicationsLogRepository : IMedicationsLogReadOnlyRepository
{
    private readonly TomouDbContext _dbContext;

    public MedicationsLogRepository(TomouDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<MedicationLog>> GetMedicationLog(MedicationLogFilter filter)
    {
        IQueryable<MedicationLog> query = _dbContext.MedicationLogs
            .AsNoTracking()
            .Include(l => l.Medication);

        if (filter.IsCaregiver)
            query = query.Where(l => l.Medication.DependentId == filter.OwnerId);
        else
            query = query.Where(l => l.Medication.UserId == filter.OwnerId);

        if (filter.MedicationId.HasValue)
            query = query.Where(l => l.MedicationId == filter.MedicationId.Value);

        if (filter.Status.HasValue)
            query = query.Where(l => l.Status == filter.Status.Value);

        if (filter.ScheduledFrom.HasValue)
            query = query.Where(l => l.ScheduledFor >= filter.ScheduledFrom.Value);
        if (filter.ScheduledTo.HasValue)
            query = query.Where(l => l.ScheduledFor <= filter.ScheduledTo.Value);

        if (filter.TakenFrom.HasValue)
            query = query.Where(l => l.TakenAt >= filter.TakenFrom.Value);
        if (filter.TakenTo.HasValue)
            query = query.Where(l => l.TakenAt <= filter.TakenTo.Value);

        if (filter.OnlyOverdue == true)
            query = query.Where(l =>
                l.ScheduledFor < DateTime.UtcNow &&
                l.TakenAt == null);

        if (filter.IsSnoozed == true)
            query = query.Where(l => l.SnoozedUntil.HasValue);

        if (!string.IsNullOrWhiteSpace(filter.NameContains))
            query = query.Where(l =>
                l.Medication.Name.Contains(filter.NameContains));

        query = filter.Ascending
            ? query.OrderBy(l => l.ScheduledFor)
            : query.OrderByDescending(l => l.ScheduledFor);

        if (filter.Page.HasValue && filter.PageSize.HasValue)
        {
            int skip = (filter.Page.Value - 1) * filter.PageSize.Value;
            query = query.Skip(skip).Take(filter.PageSize.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<MedicationLog?> GetMedicationLogById(Guid ownerId, bool isCaregiver, Guid medicationLogId)
    {
        var query = _dbContext.MedicationLogs
         .AsNoTracking()
         .Where(l => l.Id == medicationLogId);

        query = isCaregiver
            ? query.Where(l => l.Medication.DependentId == ownerId)
            : query.Where(l => l.Medication.UserId == ownerId);

        return await query.SingleOrDefaultAsync();
    }
}

