namespace Tomou.Communication.Responses.Medications.Get;

public class ResponseMedicationShortJson
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<string> TimesToTake { get; set; } = new();
    public List<string> DaysOfWeek { get; set; } = new();

}
