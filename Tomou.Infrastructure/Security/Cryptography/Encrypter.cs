using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomou.Domain.Security.Crypthography;

namespace Tomou.Infrastructure.Security.Cryptography;
internal class Encrypter : IEncrypter
{
    public bool Compare(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    public string Encrypt(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
