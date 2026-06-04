using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Profiles.Application.Abstractions;
using Profiles.Application.Services;

namespace Profiles.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IReceptionistService, ReceptionistService>();
        services.AddTransient<IEmailService, TempEmailService>();

        services.AddSingleton<IPasswordGenerator, PasswordService>();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }
}