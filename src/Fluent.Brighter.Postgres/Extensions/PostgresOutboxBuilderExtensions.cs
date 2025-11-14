using System;

using Fluent.Brighter.Postgres;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="PostgresOutboxBuilder"/> to simplify configuration.
/// These extensions enable fluent configuration of PostgreSQL outbox settings using builder patterns.
/// </summary>
public static class PostgresOutboxBuilderExtensions
{
    /// <summary>
    /// Sets the database configuration for the PostgreSQL outbox using a fluent configuration builder.
    /// This extension method creates a <see cref="RelationalDatabaseConfigurationBuilder"/>, applies the provided configuration,
    /// and sets the resulting configuration on the outbox builder.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresOutboxBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RelationalDatabaseConfigurationBuilder"/> with connection details and settings.</param>
    /// <returns>The <see cref="PostgresOutboxBuilder"/> instance for method chaining.</returns>
    public static PostgresOutboxBuilder SetConfiguration(this PostgresOutboxBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}