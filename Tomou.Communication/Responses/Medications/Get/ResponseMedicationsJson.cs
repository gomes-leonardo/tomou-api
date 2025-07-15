namespace Tomou.Communication.Responses.Medications.Get;
public class ResponseMedicationsJson
{
    public List<ResponseMedicationShortJson> Medications { get; set; } = new List<ResponseMedicationShortJson>();
}
