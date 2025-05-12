
using System.Net;

namespace Tomou.Exception.ExceptionsBase;
public class EmailAlreadyExistsException(List<string> message) : TomouException(string.Empty)
{
    private readonly List<string> _errors = message;

    public override int StatusCode => (int)HttpStatusCode.Conflict;

    public override List<string> GetErrors()
    {
        return _errors;
    }
}
