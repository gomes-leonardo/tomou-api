namespace Tomou.Application.Services.Auth;
public interface IUserContext
{
    long GetUserId();
    bool IsCaregiver();
}
