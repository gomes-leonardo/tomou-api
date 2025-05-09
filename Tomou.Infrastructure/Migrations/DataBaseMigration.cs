using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tomou.Infrastructure.DataAccess;

namespace Tomou.Infrastructure.Migrations;
public static class DataBaseMigration
{
    public async static Task MigrateDatabase(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<TomouDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
