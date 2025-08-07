using Tomou.Communication.Enums.MedicationLog;
using Microsoft.AspNetCore.Mvc;

namespace Tomou.Communication.Requests.MedicationsLog.MedicationLogQuery;
public class MedicationLogQuery
{
    [FromQuery(Name = "id")]
    public Guid? Id { get; set; }

    [FromQuery(Name = "medicationId")]
    public Guid? MedicationId { get; set; }

    [FromQuery(Name = "status")]
    public MedicationLogStatus? Status { get; set; }

    [FromQuery(Name = "scheduledFrom")]
    public DateTime? ScheduledFrom { get; set; }

    [FromQuery(Name = "scheduledTo")]
    public DateTime? ScheduledTo { get; set; }

    [FromQuery(Name = "takenFrom")]
    public DateTime? TakenFrom { get; set; }

    [FromQuery(Name = "takenTo")]
    public DateTime? TakenTo { get; set; }

    [FromQuery(Name = "onlyOverdue")]
    public bool? OnlyOverdue { get; set; }

    [FromQuery(Name = "isSnoozed")]
    public bool? IsSnoozed { get; set; }

    [FromQuery(Name = "name")]
    public string? NameContains { get; set; }

    [FromQuery(Name = "order")]
    public string Order { get; set; } = "asc";

    [FromQuery(Name = "page")]
    public int? Page { get; set; }

    [FromQuery(Name = "pageSize")]
    public int? PageSize { get; set; }
}
