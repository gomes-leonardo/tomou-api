using Tomou.Domain.Entities;
using Tomou.Domain.Enums;

public class MedicationLog
{
    public long Id { get; set; }
    public long MedicationId { get; set; }
    public Medication Medication { get; set; } = null!;

    public DateTime ScheduledFor { get; set; }  
    public DateTime? SnoozedUntil { get; set; } 
    public DateTime? TakenAt { get; set; }     
    public MedicationLogStatus Status { get; set; } 
}
