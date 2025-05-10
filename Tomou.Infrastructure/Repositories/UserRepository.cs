using Microsoft.EntityFrameworkCore;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.User;
using Tomou.Infrastructure.DataAccess;

namespace Tomou.Infrastructure.Repositories;
internal class UserRepository : IUserWriteOnlyRepository
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
}
