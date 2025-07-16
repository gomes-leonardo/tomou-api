using Tomou.Domain.Entities;

namespace Tomou.Domain.Repositories.User;
public interface IUserReadOnlyRepository
{
    Task<bool> ExistsByEmailAsync(string email);
    Task<Domain.Entities.User?> GetUserByEmail(string email);
    Task<Domain.Entities.User?> GetUserById(Guid id);
}
