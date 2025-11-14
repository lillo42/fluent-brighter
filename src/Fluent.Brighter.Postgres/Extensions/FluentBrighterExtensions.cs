using System;

using Fluent.Brighter.Postgres;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="FluentBrighterBuilder"/> to configure PostgreSQL integration.
/// These extensions enable easy setup of PostgreSQL-based messaging, outbox patterns, inbox patterns,
/// distributed locking, and message subscriptions/publications.
/// </summary>
public static class FluentBrighterExtensions
{
    /// <summary>
    /// Configures Fluent Brighter to use PostgreSQL for messaging infrastructure.
    /// This method provides a fluent API for setting up all PostgreSQL-related features including
    /// message queues, outbox/inbox patterns, distributed locking, and subscriptions/publications.
    /// </summary>
    /// <param name="builder">The <see cref="FluentBrighterBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="PostgresConfigurator"/> with PostgreSQL-specific settings.</param>
    /// <returns>The <see cref="FluentBrighterBuilder"/> instance for method chaining.</returns>
    public static FluentBrighterBuilder UsingPostgres(this FluentBrighterBuilder builder,
        Action<PostgresConfigurator> configure)
    {
        var configurator = new PostgresConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}