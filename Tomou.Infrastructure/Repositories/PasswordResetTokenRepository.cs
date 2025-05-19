using Microsoft.EntityFrameworkCore;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.PasswordToken;
using Tomou.Infrastructure.DataAccess;

namespace Tomou.Infrastructure.Repositories;
internal class PasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly TomouDbContext _dbContext;
    public PasswordResetTokenRepository(TomouDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Save(PasswordResetToken token)
    {
        await _dbContext.PasswordResetTokens.AddAsync(token);
    }
    public async Task<PasswordResetToken?> GetByToken(string token)
    {
        return await _dbContext.PasswordResetTokens
             .Include(t => t.User)
             .FirstOrDefaultAsync(t => t.Token == token);
    }

    public Task MarkAsUsed(PasswordResetToken token)
    {
        token.Used = true;
        _dbContext.PasswordResetTokens.Update(token);

        return Task.CompletedTask;
    }

}
