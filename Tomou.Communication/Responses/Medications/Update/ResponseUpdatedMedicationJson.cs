namespace Tomou.Communication.Responses.Medications.Update;
public class ResponseUpdatedMedicationJson
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
