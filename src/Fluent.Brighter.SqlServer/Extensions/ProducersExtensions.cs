using System;

using Fluent.Brighter.SqlServer;

using Paramore.Brighter;
using Paramore.Brighter.Locking.MsSql;
using Paramore.Brighter.MsSql;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for the <see cref="ProducerBuilder"/> to configure SQL Server-based
/// message publishing, outbox integration, and distributed locking within the Fluent Brighter framework.
/// </summary>
/// <remarks>
/// These extensions enable reliable, transactional message production using Microsoft SQL Server
/// for outbox persistence, publication routing, and coordination via distributed locks.
/// Part of the Fluent Brighter library (https://github.com/lillo42/fluent-brighter/),
/// which extends Paramore.Brighter with fluent APIs and relational database support.
/// </remarks>
public static class ProducersExtensions
{
    /// <summary>
    /// Adds a SQL Server message producer factory by configuring it fluently using a <see cref="SqlServerMessageProducerFactoryBuilder"/>.
    /// This factory enables publishing messages to an outbox table in SQL Server for reliable delivery.
    /// </summary>
    /// <param name="builder">The producer builder to extend.</param>
    /// <param name="configure">An action that customizes the message producer factory builder (e.g., connection, publications).</param>
    /// <returns>The updated <see cref="ProducerBuilder"/> to allow method chaining.</returns>
    public static ProducerBuilder AddMicrosoftSqlServerPublication(this ProducerBuilder builder,
        Action<SqlServerMessageProducerFactoryBuilder> configure)
    {
        var factory = new SqlServerMessageProducerFactoryBuilder();
        configure(factory);
        return builder.AddMessageProducerFactory(factory.Build());
    }

    /// <summary>
    /// Configures the producer to use a SQL Server-based outbox for reliable message publishing,
    /// using a fluent configuration action for database settings (e.g., connection string, schema).
    /// </summary>
    /// <param name="builder">The producer builder to extend.</param>
    /// <param name="configure">An action that configures the relational database connection.</param>
    /// <returns>The updated <see cref="ProducerBuilder"/> to allow method chaining.</returns>
    public static ProducerBuilder UseMicrosoftSqlServerOutbox(this ProducerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UseMicrosoftSqlServerOutbox(configuration.Build());
    }

    /// <summary>
    /// Configures the producer to use a SQL Server-based outbox with an explicit database configuration.
    /// </summary>
    /// <param name="builder">The producer builder to extend.</param>
    /// <param name="configuration">The relational database configuration containing connection details.</param>
    /// <returns>The updated <see cref="ProducerBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    public static ProducerBuilder UseMicrosoftSqlServerOutbox(this ProducerBuilder builder,
        RelationalDatabaseConfiguration configuration)
    {
        return builder.UseMicrosoftSqlServerOutbox(cfg => cfg.SetConfiguration(configuration ?? throw new ArgumentNullException(nameof(configuration))));
    }
    
    /// <summary>
    /// Configures the producer to use a SQL Server-based outbox via a fully customized <see cref="SqlServerOutboxBuilder"/>.
    /// Allows advanced setup such as custom connection providers.
    /// </summary>
    /// <param name="builder">The producer builder to extend.</param>
    /// <param name="configuration">An action that configures the outbox builder.</param>
    /// <returns>The updated <see cref="ProducerBuilder"/> to allow method chaining.</returns>
    public static ProducerBuilder UseMicrosoftSqlServerOutbox(this ProducerBuilder builder,
        Action<SqlServerOutboxBuilder> configuration)
    {
        var outbox = new SqlServerOutboxBuilder();
        configuration(outbox);
        builder.SetOutbox(outbox.Build());
        return builder;
    }
    
    /// <summary>
    /// Configures the producer to use a SQL Server-based distributed lock provider with the given connection string.
    /// Used to coordinate singleton or exclusive message processing across multiple instances.
    /// </summary>
    /// <param name="builder">The producer builder to extend.</param>
    /// <param name="connectionString">The SQL Server connection string.</param>
    /// <returns>The updated <see cref="ProducerBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="connectionString"/> is null or empty.</exception>
    public static ProducerBuilder UseMicrosoftSqlServerDistributedLock(this ProducerBuilder builder, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
        }
        
        return builder.UseMicrosoftSqlServerDistributedLock(new RelationalDatabaseConfiguration(connectionString));
    }

    /// <summary>
    /// Configures the producer to use a SQL Server-based distributed lock provider with an explicit database configuration.
    /// </summary>
    /// <param name="builder">The producer builder to extend.</param>
    /// <param name="configuration">The relational database configuration.</param>
    /// <returns>The updated <see cref="ProducerBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    public static ProducerBuilder UseMicrosoftSqlServerDistributedLock(this ProducerBuilder builder, IAmARelationalDatabaseConfiguration configuration)
    {
        return builder.SetDistributedLock(new MsSqlLockingProvider(new MsSqlConnectionProvider(configuration ?? throw new ArgumentNullException(nameof(configuration)))));
    }
}