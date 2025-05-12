namespace Tomou.Domain.Security;
public interface IEncrypter
{
    string Encrypt(string password);
}
