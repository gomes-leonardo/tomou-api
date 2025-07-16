using Microsoft.EntityFrameworkCore;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.User;
using Tomou.Infrastructure.DataAccess;

namespace Tomou.Infrastructure.Repositories.User;
internal class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly TomouDbContext _dbContext;
    public UserRepository(TomouDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(Domain.Entities.User user)
    {
        await _dbContext.AddAsync(user);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<Domain.Entities.User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Domain.Entities.User?> GetUserById(Guid id)
    {
        return await _dbContext.Users
       .Include(u => u.Dependents)
       .FirstOrDefaultAsync(u => u.Id == id);

    }
}
