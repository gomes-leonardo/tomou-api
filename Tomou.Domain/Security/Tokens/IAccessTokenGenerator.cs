using Tomou.Domain.Entities;

namespace Tomou.Domain.Security.Tokens;
public interface IAccessTokenGenerator
{
    string Generate(Guid userId, string name, string email, bool isCaregiver);
}
