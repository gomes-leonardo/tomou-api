using Microsoft.Extensions.DependencyInjection;
using Tomou.Application.AutoMapper;
using Tomou.Application.UseCases.Dependent.Register;
using Tomou.Application.UseCases.User.ForgotPassword;
using Tomou.Application.UseCases.User.Login;
using Tomou.Application.UseCases.User.Register;
using Tomou.Application.UseCases.User.ResetPassword;

namespace Tomou.Application;
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
        AddAutoMapper(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IForgotPasswordUseCase, ForgotPasswordUseCase>();
        services.AddScoped<IResetPasswordUseCase, ResetPasswordUseCase>();
        services.AddScoped<IRegisterDependentUseCase, RegisterDependentUseCase>();
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }
}
