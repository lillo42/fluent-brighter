using System;

using Fluent.Brighter.Redis;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="FluentBrighterBuilder"/> to configure Redis integration.
/// These extensions enable easy setup of Redis-based messaging, including message subscriptions and publications.
/// </summary>
public static class FluentBrighterExtensions
{
    /// <summary>
    /// Configures Fluent Brighter to use Redis for messaging infrastructure.
    /// This method provides a fluent API for setting up all Redis-related features including
    /// message queues, subscriptions, publications, and Redis-specific settings.
    /// </summary>
    /// <param name="builder">The <see cref="FluentBrighterBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RedisConfigurator"/> with Redis-specific settings.</param>
    /// <returns>The <see cref="FluentBrighterBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ConfigurationException">Thrown when the Redis configuration is invalid or incomplete (e.g., no Redis connection configured).</exception>
    public static FluentBrighterBuilder UsingRedis(this FluentBrighterBuilder builder,
        Action<RedisConfigurator> configure)
    {
        var configurator = new RedisConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}