using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tomou.Application.Services.Email;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.PasswordToken;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Domain.Security.Crypthography;
using Tomou.Domain.Security.Tokens;
using Tomou.Infrastructure.DataAccess;
using Tomou.Infrastructure.DataAccess.UnitOfWork;
using Tomou.Infrastructure.Repositories.Dependent;
using Tomou.Infrastructure.Repositories.Medications;
using Tomou.Infrastructure.Repositories.User;
using Tomou.Infrastructure.Security.Cryptography;
using Tomou.Infrastructure.Security.Tokens;

namespace Tomou.Infrastructure;
public static class DependencyInjectionExtension 
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddRepositories(services);
        AddToken(services, configuration);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IEncrypter, Encrypter>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
        services.AddScoped<IEmailService, FakeEmailService>();
        services.AddScoped<IDependentWriteOnlyRepository, DependentRepository>();
        services.AddScoped<IDependentReadOnlyRepository, DependentRepository>();
        services.AddScoped<IDependentUpdateOnlyRepository, DependentRepository>();
        services.AddScoped<IMedicationsWriteOnlyRepository, MedicationsRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
        var version = new Version(8, 0, 41);
        var serverVersion = new MySqlServerVersion(version);

        services.AddDbContext<TomouDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration["Jwt:SecretKey"];

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new ArgumentException("JWT secret key is missing in configuration.");
        if (!int.TryParse(configuration["Jwt:ExpirationMinutes"], out var expirationMinutes))
            throw new ArgumentException("JWT expiration time is missing or invalid.");

        services.AddScoped<IAccessTokenGenerator>(_ =>
            new JwtTokenGenerator(expirationMinutes, secretKey));
    }

}
