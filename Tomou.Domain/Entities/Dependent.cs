namespace Tomou.Domain.Entities;
public class Dependent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public Guid CaregiverId { get; set; }
    public User Caregiver { get; set; } = null!;
    public ICollection<Medication> Medications { get; set; } = new List<Medication>();
}
