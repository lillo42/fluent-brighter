using System;

using Fluent.Brighter.MongoDb;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="ProducerBuilder"/> to configure MongoDB-backed
/// outbox and distributed locking features in a fluent and composable way.
/// </summary>
public static class ProducersExtensions
{
    #region Outbox

    /// <summary>
    /// Configures the MongoDB outbox using a fluent configuration delegate that defines connection and database settings.
    /// A new <see cref="MongoDbConfiguration"/> is built internally and used to instantiate the outbox.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> to extend.</param>
    /// <param name="configure">An action that configures a <see cref="MongoDbConfigurationBuilder"/>.</param>
    /// <returns>The same <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="configure"/> is null.
    /// </exception>
    public static ProducerBuilder UseMongoDbOutbox(this ProducerBuilder builder, Action<MongoDbConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var configuration = new MongoDbConfigurationBuilder();
        configure(configuration);
        return builder.UseMongoDbOutbox(configuration.Build());
    }

    /// <summary>
    /// Configures the MongoDB outbox using a pre-built, shared MongoDB configuration.
    /// Useful when reusing the same database settings across inbox, outbox, and locking.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> to extend.</param>
    /// <param name="configuration">An existing MongoDB configuration implementing <see cref="IAmAMongoDbConfiguration"/>.</param>
    /// <returns>The same <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="configuration"/> is null.
    /// </exception>
    public static ProducerBuilder UseMongoDbOutbox(this ProducerBuilder builder, IAmAMongoDbConfiguration configuration)
    {
        return builder.UseMongoDbOutbox(cfg => cfg.SetConfiguration(configuration));
    }

    /// <summary>
    /// Configures the MongoDB outbox using a dedicated outbox builder for fine-grained control
    /// over collection settings, connection providers, and other outbox-specific options.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> to extend.</param>
    /// <param name="configuration">An action that configures a <see cref="MongoDbOutboxBuilder"/>.</param>
    /// <returns>The same <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="configuration"/> is null.
    /// </exception>
    public static ProducerBuilder UseMongoDbOutbox(this ProducerBuilder builder,
        Action<MongoDbOutboxBuilder> configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        
        var outbox = new MongoDbOutboxBuilder();
        configuration(outbox);
        return builder.SetOutbox(outbox.Build())
            .SetConnectionProvider(typeof(MongoDbConnectionProvider))
            .SetTransactionProvider(typeof(MongoDbUnitOfWork));
    }

    #endregion

    #region Distributed Locking

    /// <summary>
    /// Configures MongoDB-based distributed locking using a fluent configuration delegate that defines connection and database settings.
    /// A new <see cref="MongoDbConfiguration"/> is built internally and used to instantiate the locking provider.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> to extend.</param>
    /// <param name="configure">An action that configures a <see cref="MongoDbConfigurationBuilder"/>.</param>
    /// <returns>The same <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="configure"/> is null.
    /// </exception>
    public static ProducerBuilder UseMongoDbDistributedLock(this ProducerBuilder builder, Action<MongoDbConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var configuration = new MongoDbConfigurationBuilder();
        configure(configuration);
        return builder.UseMongoDbDistributedLock(configuration.Build());
    }

    /// <summary>
    /// Configures MongoDB-based distributed locking using a pre-built, shared MongoDB configuration.
    /// Ideal for consistency when the same database is used for inbox, outbox, and locking.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> to extend.</param>
    /// <param name="configuration">An existing MongoDB configuration implementing <see cref="IAmAMongoDbConfiguration"/>.</param>
    /// <returns>The same <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="configuration"/> is null.
    /// </exception>
    public static ProducerBuilder UseMongoDbDistributedLock(this ProducerBuilder builder, IAmAMongoDbConfiguration configuration)
    {
        return builder.UseMongoDbDistributedLock(cfg => cfg.SetConfiguration(configuration));
    }

    /// <summary>
    /// Configures MongoDB-based distributed locking using a dedicated locking builder for fine-grained control
    /// over lock collection settings, TTL, and connection behavior.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> to extend.</param>
    /// <param name="configuration">An action that configures a <see cref="MongoDbLockingBuilder"/>.</param>
    /// <returns>The same <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="configuration"/> is null.
    /// </exception>
    public static ProducerBuilder UseMongoDbDistributedLock(this ProducerBuilder builder,
        Action<MongoDbLockingBuilder> configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        
        var locking = new MongoDbLockingBuilder();
        configuration(locking);
        builder.SetDistributedLock(locking.Build());
        return builder;
    }

    #endregion
}