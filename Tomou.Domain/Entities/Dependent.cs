namespace Tomou.Domain.Entities;
public class Dependent
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long CaregiverId { get; set; }
    public User Caregiver { get; set; } = null!;
    public ICollection<Medication> Medications { get; set; } = new List<Medication>();
}
