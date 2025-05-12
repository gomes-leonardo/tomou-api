
using System.Net;

namespace Tomou.Exception.ExceptionsBase;
public class EmailAlreadyExistsException() : TomouException(ResourceErrorMessages.EMAIL_ALREADY_EXISTS)
{

    public override int StatusCode => (int)HttpStatusCode.Conflict;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
