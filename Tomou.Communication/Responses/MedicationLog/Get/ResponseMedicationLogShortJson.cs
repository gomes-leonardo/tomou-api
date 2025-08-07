using Tomou.Communication.Enums.MedicationLog;

namespace Tomou.Communication.Responses.MedicationLog.Get;
public class ResponseMedicationLogShortJson
{
    public Guid Id { get; set; } 
    public string MedicationName { get; set; } = string.Empty;
    public string? DependentName { get; set; }
    public DateTime ScheduledFor { get; set; }
    public DateTime SnoozedUntil { get; set; }
    public DateTime? TakenAt { get; set; }
    public MedicationLogStatus Status { get; set; }

}
