namespace Tomou.Application.Services.Auth;
public interface IUserContext
{
    Guid GetUserId();
    bool IsCaregiver();
}
