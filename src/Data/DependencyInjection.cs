using Business.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class DependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services)
    {
        var uniqueDatabaseName = $"InMemoryDb-{Guid.NewGuid()}";
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(uniqueDatabaseName));

        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        
        return services;
    }
}
