using System.Diagnostics.CodeAnalysis;
using Business.Interfaces;
using Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Business;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IReservationService, ReservationService>();
        return services;
    }
}
