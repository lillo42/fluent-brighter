using System;

using Paramore.Brighter;
using Paramore.Brighter.Locking.MongoDb;
using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter.MongoDb;

/// <summary>
/// A fluent builder for creating instances of <see cref="MongoDbLockingProvider"/>.
/// Provides a clean, readable API for configuring distributed locking behavior and MongoDB-specific settings.
/// </summary>
public class MongoDbDistributedLockBuilder
{
    private MongoDbConfiguration? _configuration;
    
    /// <summary>
    /// Sets the MongoDB-specific database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="MongoDbDistributedLockBuilder "/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbDistributedLockBuilder Connection(MongoDbConfiguration? configuration)
    {
        _configuration = configuration;
        return this;
    }
    
    /// <summary>
    /// Sets the MongoDB-specific database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbDistributedLockBuilder Connection(Action<MongoDbConfigurationBuilder> configuration)
    {
        var builder = new MongoDbConfigurationBuilder();
        configuration(builder);
        _configuration = builder.Build();
        return this;
    }

    /// <summary>
    /// Sets the MongoDB-specific database configuration if it was not set yet.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="MongoDbConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbDistributedLockBuilder SetConnectionIfIsMissing(MongoDbConfiguration? configuration)
    {
        _configuration ??= configuration;
        return this;
    }

    /// <summary>
    /// Create a new instance of <see cref="MongoDbLockingProvider"/> based on provided information
    /// </summary>
    /// <returns>A new instance of <see cref="MongoDbLockingProvider"/></returns>
    public MongoDbLockingProvider Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration not configured");
        }
        
        return new MongoDbLockingProvider(_configuration);
    }
}