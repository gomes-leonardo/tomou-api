using System.Net;

namespace Tomou.Exception.ExceptionsBase;
public class ErrorOnValidationException : TomouException
{
    private readonly List<string> _errors;
    public ErrorOnValidationException(List<string> message) : base(string.Empty)
    {
        _errors = message;
    }

    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
       return _errors;
    }
}
