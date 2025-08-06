namespace Tomou.Domain.Repositories.Medications.Filters;
public class MedicationsFilter
{
    public Guid OwnerId { get; }
    public bool IsCaregiver { get; }
    public string? NameContains { get; }
    public bool Ascending { get; }

    public MedicationsFilter(Guid ownerId, bool isCaregiver, string? nameContains, bool ascending)
    {
        OwnerId = ownerId;
        IsCaregiver = isCaregiver;
        NameContains = nameContains;
        Ascending = ascending;
    }
}

