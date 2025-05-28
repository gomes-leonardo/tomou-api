using Microsoft.EntityFrameworkCore;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.User;
using Tomou.Infrastructure.DataAccess;

namespace Tomou.Infrastructure.Repositories;
internal class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly TomouDbContext _dbContext;
    public UserRepository(TomouDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(User user)
    {
        await _dbContext.AddAsync(user);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserById(long id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

    }
}
