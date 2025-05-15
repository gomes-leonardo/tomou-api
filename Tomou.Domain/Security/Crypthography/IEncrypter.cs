namespace Tomou.Domain.Security.Crypthography;
public interface IEncrypter
{
    string Encrypt(string password);
    bool Compare(string password, string hashedPassword);
}
