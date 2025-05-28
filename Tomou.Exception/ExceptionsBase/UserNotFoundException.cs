
using System.Net;

namespace Tomou.Exception.ExceptionsBase;
public class UserNotFoundException : TomouException
{
    public UserNotFoundException() : base(ResourceErrorMessages.USER_NOT_FOUND)
    {
    }
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
