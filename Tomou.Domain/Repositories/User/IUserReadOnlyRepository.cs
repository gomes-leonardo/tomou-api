namespace Tomou.Domain.Repositories.User;
public interface IUserReadOnlyRepository
{
    Task<bool> ExistsByEmailAsync(string email);
}
