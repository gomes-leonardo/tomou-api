using Tomou.Domain.Entities;

namespace Tomou.Domain.Repositories.PasswordToken;
public interface IPasswordResetTokenRepository
{
    Task Save(PasswordResetToken token);
    Task<PasswordResetToken?> GetByToken(string token);
    Task MarkAsUsed(PasswordResetToken token);
    Task DeleteByUserId(long userId);
    Task Delete(PasswordResetToken token);
}
