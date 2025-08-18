using System;

using Fluent.Brighter.Sqlite;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring SQLite outbox settings using a fluent builder pattern
/// </summary>
public static class SqliteOutboxBuilderExtensions
{
    /// <summary>
    /// Configures the SQLite database settings using a fluent configuration builder
    /// </summary>
    /// <param name="builder">The SQLite outbox builder to extend</param>
    /// <param name="configure">Action to configure SQLite database settings</param>
    /// <returns>The SQLite outbox builder for method chaining</returns>
    /// <remarks>
    /// This extension method enables fluent configuration of SQLite database settings
    /// through a delegate that acts on a <see cref="RelationalDatabaseConfigurationBuilder"/>.
    /// The configuration is built and applied to the outbox builder.
    /// </remarks>
    public static SqliteOutboxBuilder SetConfiguration(this SqliteOutboxBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}