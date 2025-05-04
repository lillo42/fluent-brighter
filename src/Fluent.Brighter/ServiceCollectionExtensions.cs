using System;

using Microsoft.Extensions.DependencyInjection;

namespace Fluent.Brighter;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBrighter(this IServiceCollection services, Action<IBrighterRegister> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var register = new BrighterRegister(services, new BrighterOptions());
        configure(register);
        register.AddBrighter();

        return services;
    }
}