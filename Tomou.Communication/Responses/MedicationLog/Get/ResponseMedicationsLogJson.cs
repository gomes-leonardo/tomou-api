namespace Tomou.Communication.Responses.MedicationLog.Get;
public class ResponseMedicationsLogJson
{
    public List<ResponseMedicationLogShortJson> MedicationsLog {  get; set; } = new List<ResponseMedicationLogShortJson>();
}
