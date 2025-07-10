namespace Tomou.Communication.Requests.Medications.Register;
public class RequestRegisterMedicationsJson
{
    public string Name { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string TimeToTake { get; set; } = string.Empty;
    public long? DependentId { get; set; }

}   
