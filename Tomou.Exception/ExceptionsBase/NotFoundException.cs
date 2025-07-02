using System.Net;

namespace Tomou.Exception.ExceptionsBase;
public class NotFoundException : TomouException
{
    public NotFoundException(string message) : base(message)
    {

    }

    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return new List<string>() { Message };
    }
}