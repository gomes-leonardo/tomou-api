namespace Tomou.Domain.Repositories.Medications.Filters;
public class MedicationsFilterById
{
    public Guid Id { get; }
    public bool IsCaregiver { get; }
    public Guid MedicamentId { get; set; }

    public MedicationsFilterById(Guid id, bool isCaregiver, Guid medicamentId )
    {
        Id = id;
        IsCaregiver = isCaregiver;
        MedicamentId = medicamentId;
    }
}

