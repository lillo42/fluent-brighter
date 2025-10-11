using System;

using Paramore.Brighter;
using Paramore.Brighter.Inbox.MongoDb;
using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter.MongoDb;

/// <summary>
/// Provides a fluent builder for configuring and creating a <see cref="MongoDbInbox"/> instance,
/// which supports idempotent message processing using the inbox pattern in MongoDB.
/// </summary>
public sealed class MongoDbInboxBuilder
{
    private IAmAMongoDbConfiguration? _configuration;

    /// <summary>
    /// Sets the MongoDB configuration to use for the inbox.
    /// </summary>
    /// <param name="configuration">The MongoDB configuration implementing <see cref="IAmAMongoDbConfiguration"/>.</param>
    /// <returns>The current <see cref="MongoDbInboxBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    public MongoDbInboxBuilder SetConfiguration(IAmAMongoDbConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        return this;
    }

    private IAmAMongoDbConnectionProvider? _connectionProvider;

    /// <summary>
    /// Optionally sets a custom connection provider for MongoDB operations.
    /// This allows advanced control over client/database access (e.g., for multi-tenancy or connection pooling).
    /// </summary>
    /// <param name="connectionProvider">An implementation of <see cref="IAmAMongoDbConnectionProvider"/>.</param>
    /// <returns>The current <see cref="MongoDbInboxBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionProvider"/> is null.</exception>
    public MongoDbInboxBuilder SetConnectionProvider(IAmAMongoDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
        return this;
    }

    private MongoDbCollectionConfiguration? _collection;

    /// <summary>
    /// Configures the MongoDB collection settings specifically for the inbox (e.g., collection name, indexes).
    /// If provided, this overrides any inbox configuration already present in the main MongoDB configuration.
    /// </summary>
    /// <param name="collection">The inbox collection configuration.</param>
    /// <returns>The current <see cref="MongoDbInboxBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
    public MongoDbInboxBuilder SetCollection(MongoDbCollectionConfiguration collection)
    {
        _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        return this;
    }

    /// <summary>
    /// Builds and returns a configured <see cref="MongoDbInbox"/> instance.
    /// </summary>
    /// <returns>A new instance of <see cref="MongoDbInbox"/>.</returns>
    /// <exception cref="ConfigurationException">
    /// Thrown if <see cref="SetConfiguration"/> has not been called or the provided configuration is null.
    /// </exception>
    internal MongoDbInbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }

        // Override inbox collection configuration if explicitly set
        if (_collection != null)
        {
            _configuration.Inbox = _collection;
        }

        // Use connection provider if available; otherwise, rely on configuration's embedded client
        return _connectionProvider == null 
            ? new MongoDbInbox(_configuration) 
            : new MongoDbInbox(_connectionProvider, _configuration);
    }
}