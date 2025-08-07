using Tomou.Domain.Enums;

public class MedicationLogFilter
{
    public Guid OwnerId { get; }
    public bool IsCaregiver { get; }

    public Guid? MedicationId { get; }
    public MedicationLogStatus? Status { get; }
    public DateTime? ScheduledFrom { get; }
    public DateTime? ScheduledTo   { get; }
    public DateTime? TakenFrom     { get; }
    public DateTime? TakenTo       { get; }
    public bool? OnlyOverdue       { get; }
    public bool? IsSnoozed         { get; }
    public string? NameContains    { get; }

    public bool Ascending   { get; }
    public int?  Page       { get; }
    public int?  PageSize   { get; }

    public MedicationLogFilter(
        Guid ownerId,
        bool isCaregiver,
        Guid? medicationId            = null,
        MedicationLogStatus? status   = null,
        DateTime? scheduledFrom       = null,
        DateTime? scheduledTo         = null,
        DateTime? takenFrom           = null,
        DateTime? takenTo             = null,
        bool? onlyOverdue             = null,
        bool? isSnoozed               = null,
        string? nameContains          = null,
        bool ascending                = true,
        int? page                     = null,
        int? pageSize                 = null)
    {
        OwnerId       = ownerId;
        IsCaregiver   = isCaregiver;
        MedicationId  = medicationId;
        Status        = status;
        ScheduledFrom = scheduledFrom;
        ScheduledTo   = scheduledTo;
        TakenFrom     = takenFrom;
        TakenTo       = takenTo;
        OnlyOverdue   = onlyOverdue;
        IsSnoozed     = isSnoozed;
        NameContains  = nameContains;
        Ascending     = ascending;
        Page          = page;
        PageSize      = pageSize;
    }
}
