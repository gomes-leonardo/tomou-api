namespace Tomou.Communication.Requests.Medications.Register;
public class RequestRegisterMedicationsJson
{
    public string Name { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<string> TimesToTake { get; set; } = new();
    public List<string> DaysOfWeek { get; set; } = new();

    public long? DependentId { get; set; }
}

