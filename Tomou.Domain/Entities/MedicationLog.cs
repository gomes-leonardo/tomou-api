public class MedicationLog
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public bool WasTaken { get; set; }
    public long MedicationId { get; set; }
    public Medication Medication { get; set; } = null!;
}
