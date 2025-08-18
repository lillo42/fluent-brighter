using System;

using Fluent.Brighter.Sqlite;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring a SQLite inbox with Brighter's consumer pipeline
/// </summary>
public static class ConsumerBuilderExtensions
{
    /// <summary>
    /// Configures a SQLite inbox using a fluent configuration builder
    /// </summary>
    /// <param name="builder">The consumer builder to extend</param>
    /// <param name="configure">Action to configure SQLite database settings</param>
    /// <returns>The consumer builder for method chaining</returns>
    public static ConsumerBuilder UseSqliteInbox(this ConsumerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UseSqliteInbox(configuration.Build());
    }

    /// <summary>
    /// Configures a SQLite inbox using an existing database configuration
    /// </summary>
    /// <param name="builder">The consumer builder to extend</param>
    /// <param name="configuration">Pre-configured SQLite database settings</param>
    /// <returns>The consumer builder for method chaining</returns>
    public static ConsumerBuilder UseSqliteInbox(this ConsumerBuilder builder, IAmARelationalDatabaseConfiguration configuration)
        => builder.UseSqliteInbox(cfg => cfg.SetConfiguration(configuration));

    /// <summary>
    /// Configures a SQLite inbox using a custom inbox builder configuration
    /// </summary>
    /// <param name="builder">The consumer builder to extend</param>
    /// <param name="configure">Action to customize SQLite inbox settings</param>
    /// <returns>The consumer builder for method chaining</returns>
    public static ConsumerBuilder UseSqliteInbox(this ConsumerBuilder builder, Action<SqliteInboxBuilder> configure)
    {
        var inbox = new SqliteInboxBuilder();
        configure(inbox);
        return builder.SetInbox(cfg => cfg.SetInbox(inbox.Build()));
    }
}