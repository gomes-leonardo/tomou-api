
using System.Net;

namespace Tomou.Exception.ExceptionsBase;
public class ExpiredTokenException : TomouException
{
    public ExpiredTokenException() : base(ResourceErrorMessages.EXPIRED_TOKEN)
    {
    }
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
