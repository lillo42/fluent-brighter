using System;

using Fluent.Brighter.Sqlite;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring a SQLite outbox with Brighter's producer pipeline
/// </summary>
public static class ProducersExtensions
{
    /// <summary>
    /// Configures a SQLite outbox using fluent database configuration
    /// </summary>
    /// <param name="builder">The producer builder to extend</param>
    /// <param name="configure">Action to configure SQLite database settings</param>
    /// <returns>The producer builder for method chaining</returns>
    public static ProducerBuilder UseSqliteOutbox(this ProducerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UseSqliteOutbox(configuration.Build());
    }

    /// <summary>
    /// Configures a SQLite outbox using a pre-configured database settings object
    /// </summary>
    /// <param name="builder">The producer builder to extend</param>
    /// <param name="configuration">Pre-configured SQLite database settings</param>
    /// <returns>The producer builder for method chaining</returns>
    public static ProducerBuilder UseSqliteOutbox(this ProducerBuilder builder,
        RelationalDatabaseConfiguration configuration)
    {
        return builder.UseSqliteOutbox(cfg => cfg.SetConfiguration(configuration));
    }
    
    /// <summary>
    /// Configures a SQLite outbox using a custom outbox builder configuration
    /// </summary>
    /// <param name="builder">The producer builder to extend</param>
    /// <param name="configuration">Action to customize SQLite outbox settings</param>
    /// <returns>The producer builder for method chaining</returns>
    public static ProducerBuilder UseSqliteOutbox(this ProducerBuilder builder,
        Action<SqliteOutboxBuilder> configuration)
    {
        var outbox = new SqliteOutboxBuilder();
        configuration(outbox);
        builder.SetOutbox(outbox.Build());
        return builder;
    }
}