using System;

using Paramore.Brighter;
using Paramore.Brighter.Locking.MongoDb;
using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter.MongoDb;

/// <summary>
/// Provides a fluent builder for configuring and creating a <see cref="MongoDbLockingProvider"/> instance,
/// which enables distributed locking using a MongoDB collection to coordinate message processing.
/// </summary>
public sealed class MongoDbLockingBuilder
{
    private IAmAMongoDbConfiguration? _configuration;

    /// <summary>
    /// Sets the MongoDB configuration to use for distributed locking.
    /// </summary>
    /// <param name="configuration">The MongoDB configuration implementing <see cref="IAmAMongoDbConfiguration"/>.</param>
    /// <returns>The current <see cref="MongoDbLockingBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    public MongoDbLockingBuilder SetConfiguration(IAmAMongoDbConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        return this;
    }

    private IAmAMongoDbConnectionProvider? _connectionProvider;

    /// <summary>
    /// Optionally sets a custom MongoDB connection provider for advanced scenarios such as multi-tenancy,
    /// dynamic connection routing, or custom client lifecycle management.
    /// </summary>
    /// <param name="connectionProvider">An implementation of <see cref="IAmAMongoDbConnectionProvider"/>.</param>
    /// <returns>The current <see cref="MongoDbLockingBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionProvider"/> is null.</exception>
    public MongoDbLockingBuilder SetConnectionProvider(IAmAMongoDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
        return this;
    }

    private MongoDbCollectionConfiguration? _collection;

    /// <summary>
    /// Configures the MongoDB collection settings used for storing distributed locks (e.g., collection name, indexes).
    /// If specified, this overrides any locking collection configuration already present in the main configuration.
    /// </summary>
    /// <param name="collection">The locking collection configuration.</param>
    /// <returns>The current <see cref="MongoDbLockingBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
    public MongoDbLockingBuilder SetCollection(MongoDbCollectionConfiguration collection)
    {
        _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        return this;
    }

    /// <summary>
    /// Builds and returns a configured <see cref="MongoDbLockingProvider"/> instance.
    /// </summary>
    /// <returns>A new instance of <see cref="MongoDbLockingProvider"/>.</returns>
    /// <exception cref="ConfigurationException">
    /// Thrown if <see cref="SetConfiguration"/> has not been called or the configuration is null.
    /// </exception>
    internal MongoDbLockingProvider Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }

        // Override locking collection configuration if explicitly provided
        if (_collection != null)
        {
            _configuration.Locking = _collection;
        }

        // Instantiate the locking provider with or without a custom connection provider
        return _connectionProvider == null
            ? new MongoDbLockingProvider(_configuration)
            : new MongoDbLockingProvider(_connectionProvider, _configuration);
    }
}