﻿using API.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace API.ServiceCollectionExtensions;

public static class ValidationServices
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<CarValidator>();
        services.AddTransient<CarValidator>(); 
        
        return services;
    }
}
