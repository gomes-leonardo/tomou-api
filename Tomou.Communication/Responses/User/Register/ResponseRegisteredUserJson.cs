namespace Tomou.Communication.Responses.User.Register;
public class ResponseRegisteredUserJson
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsCaregiver { get; set; }
}
