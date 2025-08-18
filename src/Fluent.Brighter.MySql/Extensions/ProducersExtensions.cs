using System;

using Fluent.Brighter.MySql;

using Paramore.Brighter;
using Paramore.Brighter.Locking.MySql;
using Paramore.Brighter.MySql;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring MySQL-related components in Brighter's producer pipeline
/// </summary>
/// <remarks>
/// These fluent extensions simplify MySQL outbox and distributed lock configuration.
/// </remarks>
public static class ProducersExtensions
{
    /// <summary>
    /// Configures and enables the MySQL outbox using a fluent configuration delegate
    /// </summary>
    /// <param name="builder">The producer builder instance</param>
    /// <param name="configure">Action that defines the MySQL outbox configuration</param>
    /// <returns>The producer builder for fluent chaining</returns>
    public static ProducerBuilder UseMySqlOutbox(this ProducerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UseMySqlOutbox(configuration.Build());
    }

    /// <summary>
    /// Configures the MySQL outbox using a pre-built configuration
    /// </summary>
    /// <param name="builder">The producer builder instance</param>
    /// <param name="configuration">The relational database configuration</param>
    /// <returns>The producer builder for fluent chaining</returns>
    /// <remarks>
    /// Use this overload when you have a pre-configured <see cref="RelationalDatabaseConfiguration"/> instance.
    /// </remarks>
    public static ProducerBuilder UseMySqlOutbox(this ProducerBuilder builder,
        RelationalDatabaseConfiguration configuration)
    {
        return builder.UseMySqlOutbox(cfg => cfg.SetConfiguration(configuration));
    }
    
    /// <summary>
    /// Configures the MySQL outbox using a direct outbox builder delegate
    /// </summary>
    /// <param name="builder">The producer builder instance</param>
    /// <param name="configuration">Action that configures the MySQL outbox builder</param>
    /// <returns>The producer builder for fluent chaining</returns>
    /// <remarks>
    /// Provides full control over MySQL outbox configuration:
    /// <code>
    /// .UseMySqlOutbox(outboxBuilder => 
    /// {
    ///     outboxBuilder
    ///         .SetConfiguration(/* config */)
    ///         .SetConnectionProvider(/* custom provider */);
    /// })
    /// </code>
    /// </remarks>
    public static ProducerBuilder UseMySqlOutbox(this ProducerBuilder builder,
        Action<MySqlOutboxBuilder> configuration)
    {
        var outbox = new MySqlOutboxBuilder();
        configuration(outbox);
        builder.SetOutbox(outbox.Build());
        return builder;
    }
    
    /// <summary>
    /// Configures a MySQL-based distributed lock using a database configuration
    /// </summary>
    /// <param name="builder">The producer builder instance</param>
    /// <param name="configuration">The relational database configuration</param>
    /// <returns>The producer builder for fluent chaining</returns>
    /// <remarks>
    /// Enables distributed locking for producer message deduplication.
    /// Requires a valid <see cref="IAmARelationalDatabaseConfiguration"/> instance.
    /// </remarks> 
    public static ProducerBuilder UseMySqlDistributedLock(this ProducerBuilder builder, IAmARelationalDatabaseConfiguration configuration)
        => builder.SetDistributedLock(new MySqlLockingProvider(new MySqlConnectionProvider(configuration)));
    
    /// <summary>
    /// Configures a MySQL-based distributed lock using a connection string
    /// </summary>
    /// <param name="builder">The producer builder instance</param>
    /// <param name="connectionString">MySQL connection string</param>
    /// <returns>The producer builder for fluent chaining</returns>
    /// <remarks>
    /// Simplifies lock configuration by accepting a direct connection string:
    /// <code>
    /// .UseMySqlDistributedLock("Server=localhost;Database=brighter;Uid=user;Pwd=pass;")
    /// </code>
    /// </remarks>
    public static ProducerBuilder UseMySqlDistributedLock(this ProducerBuilder builder, string connectionString) 
        => builder.UseMySqlDistributedLock(new RelationalDatabaseConfiguration(connectionString));
}