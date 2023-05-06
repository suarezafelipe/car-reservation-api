using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace API.ServiceCollectionExtensions;

public static class ApiVersioningService
{
    public static IServiceCollection AddApiVersioningService(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ApiVersionReader = new MediaTypeApiVersionReader();

            // Assume a default API version if the client doesn't specify one
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);

            // Report the supported API versions in the response header
            options.ReportApiVersions = true;
        });

        return services;
    }
}
