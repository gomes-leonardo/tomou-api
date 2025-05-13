using Tomou.Domain.Entities;

namespace Tomou.Domain.Repositories.User.Register;
public interface IUserWriteOnlyRepository
{
    Task Add(Entities.User user);
}
