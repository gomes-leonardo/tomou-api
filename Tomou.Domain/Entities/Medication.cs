namespace Tomou.Domain.Entities;
public class Medication
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<TimeOnly> TimesToTake { get; set; } = new();
    public List<DayOfWeek> DaysOfWeek { get; set; } = new();
    public Guid? DependentId { get; set; }
    public Dependent? Dependent { get; set; }
    public Guid? UserId { get; set; } 
    public User? User { get; set; }
    public ICollection<MedicationLog> Logs { get; set; } = new List<MedicationLog>();
}
