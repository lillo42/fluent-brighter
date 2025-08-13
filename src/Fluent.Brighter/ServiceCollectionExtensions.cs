using System;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter.ServiceActivator.Extensions.DependencyInjection;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring Brighter services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Brighter messaging and command processing services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configure">An action to configure the Brighter services.</param>
    /// <returns>The original service collection for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddFluentBrighter(this IServiceCollection services,
        Action<FluentBrighterBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = new FluentBrighterBuilder();
        configure(builder);

        var brighter = services.AddConsumers(opt => builder.SetConsumerOptions(opt));
        builder.SetBrighterBuilder(brighter);
        return services;
    }
}