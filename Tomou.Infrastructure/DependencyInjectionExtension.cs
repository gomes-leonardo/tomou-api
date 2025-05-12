using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Domain.Security;
using Tomou.Infrastructure.DataAccess;
using Tomou.Infrastructure.DataAccess.UnitOfWork;
using Tomou.Infrastructure.Repositories;
using Tomou.Infrastructure.Security;

namespace Tomou.Infrastructure;
public static class DependencyInjectionExtension 
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IEncrypter, Encrypter>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
        var version = new Version(8, 0, 41);
        var serverVersion = new MySqlServerVersion(version);

        services.AddDbContext<TomouDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }
}
