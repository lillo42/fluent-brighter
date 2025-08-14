using System;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter.ServiceActivator.Extensions.DependencyInjection;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for integrating Brighter with Microsoft Dependency Injection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Brighter to the service collection using a fluent configuration interface
    /// </summary>
    /// <remarks>
    /// This is the main entry point for configuring Brighter in your application. It provides a fluent interface
    /// to configure all aspects of Brighter including consumers, producers, handlers, mappers, and transformers.
    /// </remarks>
    /// <param name="services">The service collection to add Brighter to</param>
    /// <param name="configure">Configuration action for setting up Brighter components</param>
    /// <returns>The service collection for chaining</returns>
    /// <exception cref="ArgumentNullException">Thrown if the configure action is null</exception>
    /// <example>
    /// <code>
    /// services.AddFluentBrighter(builder =>
    /// {
    ///     builder.Subscriptions(c => ...)
    ///            .RequestHandlers(h => ...)
    ///            .Mappers(m => ...)
    ///            .Producers(p => ...);
    /// });
    /// </code>
    /// </example>
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