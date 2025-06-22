
using System.Net;

namespace Tomou.Exception.ExceptionsBase;
public class UnauthorizedException : TomouException
{
    public UnauthorizedException() : base(ResourceErrorMessages.UNAUTHORIZED)
    {
    }

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
