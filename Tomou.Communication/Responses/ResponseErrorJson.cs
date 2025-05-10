namespace Tomou.Communication.Responses;
public class ResponseErrorJson 
{
    public List<string> Errors { get; set; }
    public ResponseErrorJson(string errorMessage) 
    {
        Errors = [errorMessage];
    }

    public ResponseErrorJson(List<string> errorMessage)
    {
        Errors = errorMessage;
    }
}
