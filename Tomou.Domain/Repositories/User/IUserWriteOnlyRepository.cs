using Tomou.Domain.Entities;

namespace Tomou.Domain.Repositories.User;
public interface IUserWriteOnlyRepository
{
    Task Add(Entities.User user);
}
