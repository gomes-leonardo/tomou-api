namespace Tomou.Communication.Responses.Medications.Get;
public class ResponseMedicationShortJson
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
}
