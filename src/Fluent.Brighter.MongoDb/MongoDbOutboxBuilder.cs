using System;

using Paramore.Brighter;
using Paramore.Brighter.MongoDb;
using Paramore.Brighter.Outbox.MongoDb;

namespace Fluent.Brighter.MongoDb;

/// <summary>
/// Provides a fluent builder for configuring and creating a <see cref="MongoDbOutbox"/> instance,
/// which supports the outbox pattern for reliable message publishing using MongoDB.
/// </summary>
public sealed class MongoDbOutboxBuilder
{
    private IAmAMongoDbConfiguration? _configuration;

    /// <summary>
    /// Sets the MongoDB configuration to use for the outbox.
    /// </summary>
    /// <param name="configuration">The MongoDB configuration implementing <see cref="IAmAMongoDbConfiguration"/>.</param>
    /// <returns>The current <see cref="MongoDbOutboxBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    public MongoDbOutboxBuilder SetConfiguration(IAmAMongoDbConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        return this;
    }

    private IAmAMongoDbConnectionProvider? _connectionProvider;

    /// <summary>
    /// Optionally sets a custom MongoDB connection provider for advanced scenarios such as multi-tenancy,
    /// dynamic database routing, or custom client management.
    /// </summary>
    /// <param name="connectionProvider">An implementation of <see cref="IAmAMongoDbConnectionProvider"/>.</param>
    /// <returns>The current <see cref="MongoDbOutboxBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionProvider"/> is null.</exception>
    public MongoDbOutboxBuilder SetConnectionProvider(IAmAMongoDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
        return this;
    }

    private MongoDbCollectionConfiguration? _collection;

    /// <summary>
    /// Configures the MongoDB collection settings used for storing outgoing messages (e.g., collection name, indexes).
    /// If provided, this overrides any outbox collection configuration already present in the main configuration.
    /// </summary>
    /// <param name="collection">The outbox collection configuration.</param>
    /// <returns>The current <see cref="MongoDbOutboxBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
    public MongoDbOutboxBuilder SetCollection(MongoDbCollectionConfiguration collection)
    {
        _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        return this;
    }

    /// <summary>
    /// Builds and returns a configured <see cref="MongoDbOutbox"/> instance.
    /// </summary>
    /// <returns>A new instance of <see cref="MongoDbOutbox"/>.</returns>
    /// <exception cref="ConfigurationException">
    /// Thrown if <see cref="SetConfiguration"/> has not been called or the configuration is null.
    /// </exception>
    internal MongoDbOutbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }

        // Override outbox collection configuration if explicitly set
        if (_collection != null)
        {
            _configuration.Outbox = _collection;
        }

        // Use custom connection provider if available; otherwise, use the configuration's embedded client
        return _connectionProvider == null
            ? new MongoDbOutbox(_configuration)
            : new MongoDbOutbox(_connectionProvider, _configuration);
    }
}