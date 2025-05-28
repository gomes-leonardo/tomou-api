
using System.Net;

namespace Tomou.Exception.ExceptionsBase;
public class InvalidTokenException : TomouException
{
    public InvalidTokenException() : base(ResourceErrorMessages.INVALID_TOKEN)
    {
    }
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
