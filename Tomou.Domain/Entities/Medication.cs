using System.ComponentModel.DataAnnotations.Schema;
using Tomou.Domain.Entities;

public class Medication
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public TimeSpan TimeToTake { get; set; }  

    public long DependentId { get; set; }
    public Dependent Dependent { get; set; } = null!;
    public ICollection<MedicationLog> Logs { get; set; } = new List<MedicationLog>();
}
