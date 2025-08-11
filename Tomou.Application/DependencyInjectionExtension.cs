using Microsoft.Extensions.DependencyInjection;
using Tomou.Application.AutoMapper;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Dependent.Delete;
using Tomou.Application.UseCases.Dependent.GetAll;
using Tomou.Application.UseCases.Dependent.GetDependentById;
using Tomou.Application.UseCases.Dependent.Register;
using Tomou.Application.UseCases.Dependent.Update;
using Tomou.Application.UseCases.Medications.Delete;
using Tomou.Application.UseCases.Medications.Get;
using Tomou.Application.UseCases.Medications.GetById;
using Tomou.Application.UseCases.Medications.Register;
using Tomou.Application.UseCases.Medications.Update;
using Tomou.Application.UseCases.MedicationsLog.Get;
using Tomou.Application.UseCases.MedicationsLog.Get.Factories;
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
        services.AddScoped<IUserContext, UserContext>();

        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();

        services.AddScoped<IForgotPasswordUseCase, ForgotPasswordUseCase>();
        services.AddScoped<IResetPasswordUseCase, ResetPasswordUseCase>();

        services.AddScoped<IRegisterDependentUseCase, RegisterDependentUseCase>();
        services.AddScoped<IGetDependentsUseCase, GetDependentsUseCase>();
        services.AddScoped<IGetDependentByIdUseCase, GetDependentByIdUseCase>();
        services.AddScoped<IUpdateDependentUseCase, UpdateDependentUseCase>();
        services.AddScoped<IDeleteDependentUseCase, DeleteDependentUseCase>();


        services.AddScoped<IRegisterMedicationUseCase, RegisterMedicationUseCase>();
        services.AddScoped<IGetMedicationsUseCase, GetMedicationsUseCase>();
        services.AddScoped<IGetMedicationByIdUseCase, GetMedicationByIdUseCase>();
        services.AddScoped<IUpdateMedicationUseCase, UpdateMedicationUseCase>();
        services.AddScoped<IDeleteMedicationUseCase, DeleteMedicationUseCase>();

        services.AddScoped<IGetMedicationsLogUseCase,  GetMedicationsLogUseCase>();
        services.AddScoped<IMedicationLogFilterFactory, MedicationLogFilterFactory>();

    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }
}
