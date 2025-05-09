using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tomou.Infrastructure.DataAccess;

namespace Tomou.Infrastructure;
public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
        var version = new Version(8, 0, 41);
        var serverVersion = new MySqlServerVersion(version);

        services.AddDbContext<TomouDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }
}
