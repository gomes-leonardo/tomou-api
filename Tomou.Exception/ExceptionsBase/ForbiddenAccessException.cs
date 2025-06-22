using System.Net;

namespace Tomou.Exception.ExceptionsBase;

public class ForbiddenAccessException : TomouException
{
    public ForbiddenAccessException(string message) : base(message)
    {
    }

    public override int StatusCode => (int)HttpStatusCode.Forbidden;

    public override List<string> GetErrors()
    {
        return new List<string> { Message };
    }
}
