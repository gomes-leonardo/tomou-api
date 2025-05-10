namespace Tomou.Exception.ExceptionsBase;
public abstract class TomouException(string? message) : SystemException(message)
{
    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}
