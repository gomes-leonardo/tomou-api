
using System.Net;

namespace Tomou.Exception.ExceptionsBase;
public class InvalidCredentialsException : TomouException
{
    public InvalidCredentialsException() : base(ResourceErrorMessages.INVALID_CREDENTIALS)
    {
    }

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
