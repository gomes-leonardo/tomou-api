using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Tomou.Infrastructure.DataAccess;

public class TomouDbContextFactory : IDesignTimeDbContextFactory<TomouDbContext>
{
    public TomouDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("Connection");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));

        var optionsBuilder = new DbContextOptionsBuilder<TomouDbContext>();
        optionsBuilder.UseMySql(connectionString, serverVersion);

        return new TomouDbContext(optionsBuilder.Options);
    }
}
