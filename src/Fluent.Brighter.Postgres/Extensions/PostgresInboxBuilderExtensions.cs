using System;

using Fluent.Brighter.Postgres;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="PostgresInboxBuilder"/> to simplify configuration.
/// These extensions enable fluent configuration of PostgreSQL inbox settings using builder patterns.
/// </summary>
public static class PostgresInboxBuilderExtensions
{
    /// <summary>
    /// Sets the database configuration for the PostgreSQL inbox using a fluent configuration builder.
    /// This extension method creates a <see cref="RelationalDatabaseConfigurationBuilder"/>, applies the provided configuration,
    /// and sets the resulting configuration on the inbox builder.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresInboxBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RelationalDatabaseConfigurationBuilder"/> with connection details and settings.</param>
    /// <returns>The <see cref="PostgresInboxBuilder"/> instance for method chaining.</returns>
    public static PostgresInboxBuilder SetConfiguration(this PostgresInboxBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}