namespace Tomou.Communication.Requests.User.Register;
public class RequestRegisterUserJson
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsCaregiver { get; set; }
}
