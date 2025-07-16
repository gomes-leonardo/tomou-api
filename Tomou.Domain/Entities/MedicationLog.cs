using Tomou.Domain.Entities;
using Tomou.Domain.Enums;

public class MedicationLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MedicationId { get; set; }
    public Medication Medication { get; set; } = null!;

    public DateTime ScheduledFor { get; set; }  
    public DateTime? SnoozedUntil { get; set; } 
    public DateTime? TakenAt { get; set; }     
    public MedicationLogStatus Status { get; set; } 
}
