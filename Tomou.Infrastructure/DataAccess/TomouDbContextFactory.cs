using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Tomou.Infrastructure.DataAccess;

public class TomouDbContextFactory : IDesignTimeDbContextFactory<TomouDbContext>
{
    public TomouDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TomouDbContext>();

        var connectionString = "Server=localhost;Database=tomoudb;Uid=tomou_dev;Pwd=TomouDev123!"; 
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));

        optionsBuilder.UseMySql(connectionString, serverVersion);

        return new TomouDbContext(optionsBuilder.Options);
    }
}
