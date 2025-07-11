namespace Tomou.Domain.Entities;
public class Medication
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<TimeOnly> TimesToTake { get; set; } = new();
    public List<DayOfWeek> DaysOfWeek { get; set; } = new();
    public long? DependentId { get; set; }
    public Dependent? Dependent { get; set; }
    public long? UserId { get; set; }
    public User? User { get; set; }
    public ICollection<MedicationLog> Logs { get; set; } = new List<MedicationLog>();
}
