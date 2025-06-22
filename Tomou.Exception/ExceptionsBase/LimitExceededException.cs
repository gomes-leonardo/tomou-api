using System.Net;

namespace Tomou.Exception.ExceptionsBase;

public class LimitExceededException : TomouException
{
    public LimitExceededException(string message) : base(message)
    {
    }

    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
        return new List<string> { Message };
    }
}
