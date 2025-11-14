using System;

using Fluent.Brighter.Postgres;

using Paramore.Brighter;
using Paramore.Brighter.Locking.PostgresSql;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="ProducerBuilder"/> to configure PostgreSQL-based message producers.
/// These extensions enable easy setup of PostgreSQL publications, outbox patterns, and distributed locking for message production.
/// </summary>
public static class ProducersExtensions
{
    /// <summary>
    /// Adds a PostgreSQL publication to the producer builder using a configuration action.
    /// Publications define how messages are published to PostgreSQL, including topic mappings and message metadata.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="PostgresMessageProducerFactoryBuilder"/> with publication settings.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    public static ProducerBuilder AddPostgresPublication(this ProducerBuilder builder,
        Action<PostgresMessageProducerFactoryBuilder> configure)
    {
        var factory = new PostgresMessageProducerFactoryBuilder();
        configure(factory);

        return builder.AddMessageProducerFactory(factory.Build());
    }


    /// <summary>
    /// Configures the producer to use a PostgreSQL outbox pattern using a database configuration builder.
    /// The outbox pattern ensures reliable message publishing by storing outgoing messages in PostgreSQL
    /// as part of the same transaction as domain changes, guaranteeing eventual consistency.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RelationalDatabaseConfigurationBuilder"/> with connection details.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    public static ProducerBuilder UsePostgresOutbox(this ProducerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UsePostgresOutbox(configuration.Build());
    }

    /// <summary>
    /// Configures the producer to use a PostgreSQL outbox pattern using a pre-configured database configuration.
    /// The outbox pattern ensures reliable message publishing by storing outgoing messages in PostgreSQL
    /// as part of the same transaction as domain changes, guaranteeing eventual consistency.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance to configure.</param>
    /// <param name="configuration">The pre-configured relational database configuration containing connection details.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    public static ProducerBuilder UsePostgresOutbox(this ProducerBuilder builder,
        RelationalDatabaseConfiguration configuration)
    {
        return builder.UsePostgresOutbox(cfg => cfg.SetConfiguration(configuration));
    }
    
    /// <summary>
    /// Configures the producer to use a PostgreSQL outbox pattern using a custom outbox builder configuration.
    /// This method provides the most flexibility by allowing direct configuration of the <see cref="PostgresOutboxBuilder"/>,
    /// including custom connection providers, data sources, and other advanced settings.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance to configure.</param>
    /// <param name="configuration">An action that configures the <see cref="PostgresOutboxBuilder"/> with outbox settings.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    public static ProducerBuilder UsePostgresOutbox(this ProducerBuilder builder,
        Action<PostgresOutboxBuilder> configuration)
    {
        var outbox = new PostgresOutboxBuilder();
        configuration(outbox);
        builder.SetOutbox(outbox.Build());
        return builder;
    }

    /// <summary>
    /// Configures the producer to use PostgreSQL-based distributed locking using a database configuration.
    /// Distributed locking ensures that only one producer instance can send messages at a time in a distributed environment,
    /// preventing duplicate message publishing and ensuring consistency.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance to configure.</param>
    /// <param name="configuration">The relational database configuration containing connection details.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    public static ProducerBuilder UsePostgresDistributedLock(this ProducerBuilder builder, IAmARelationalDatabaseConfiguration configuration)
    {
        return builder.UsePostgresDistributedLock(configuration.ConnectionString);
    }

    /// <summary>
    /// Configures the producer to use PostgreSQL-based distributed locking using a connection string.
    /// Distributed locking ensures that only one producer instance can send messages at a time in a distributed environment,
    /// preventing duplicate message publishing and ensuring consistency.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance to configure.</param>
    /// <param name="connectionString">The PostgreSQL database connection string.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    public static ProducerBuilder UsePostgresDistributedLock(this ProducerBuilder builder, string connectionString)
    {
        return builder.SetDistributedLock(
            new PostgresLockingProvider(new PostgresLockingProviderOptions(connectionString)));
    }
}