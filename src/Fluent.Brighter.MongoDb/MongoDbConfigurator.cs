using System;

using Paramore.Brighter;
using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter.MongoDb; // Replace with actual namespace

/// <summary>
/// A fluent configurator for MongoDB integration with Brighter message handling infrastructure.
/// Provides methods to configure MongoDB-based outbox, inbox, distributed locks, and luggage store components.
/// </summary>
/// <remarks>
/// This class enables a fluent API for defining MongoDB connection settings and configuring message persistence components.
/// Use the <see cref="AddMongoDb(IBrighterConfigurator)"/> method internally to register MongoDB services with Brighter.
/// </remarks>
public class MongoDbConfigurator
{
    private MongoDbConfiguration? _configurator;

    /// <summary>
    /// Sets the MongoDB configuration directly using an existing <see cref="MongoDbConfiguration"/> instance.
    /// </summary>
    /// <param name="configure">The pre-configured MongoDB configuration.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for fluent chaining.</returns>
    /// <remarks>
    /// This method is typically used in advanced scenarios where the configuration is built externally.
    /// For standard use, prefer the <see cref="Connection(Action{MongoDbConfigurationBuilder})"/> overload.
    /// </remarks>
    public MongoDbConfigurator Connection(MongoDbConfiguration configure)
    {
        _configurator = configure;
        return this;
    }

    /// <summary>
    /// Configures MongoDB connection settings using a fluent <see cref="MongoDbConfigurationBuilder"/>.
    /// </summary>
    /// <param name="configure">An action to customize the <see cref="MongoDbConfigurationBuilder"/>.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is null.</exception>
    public MongoDbConfigurator Connection(Action<MongoDbConfigurationBuilder> configure)
    {
        var builder = new MongoDbConfigurationBuilder();
        configure(builder);
        _configurator = builder.Build();
        return this;
    }

    private MongoDbOutboxBuilder? _outboxBuilder;

    /// <summary>
    /// Configures MongoDB-based outbox for message persistence.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="MongoDbOutboxBuilder"/>.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for fluent chaining.</returns>
    /// <remarks>
    /// If no configuration is provided, uses default settings with the current MongoDB connection.
    /// </remarks>
    public MongoDbConfigurator UsingOutbox(Action<MongoDbOutboxBuilder>? configure)
    {
        _outboxBuilder = new MongoDbOutboxBuilder();
        configure?.Invoke(_outboxBuilder);
        return this;
    }

    private MongoDbInboxBuilder? _inboxBuilder;

    /// <summary>
    /// Configures MongoDB-based inbox for message de-duplication.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="MongoDbInboxBuilder"/>.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for fluent chaining.</returns>
    /// <remarks>
    /// If no configuration is provided, uses default settings with the current MongoDB connection.
    /// </remarks>
    public MongoDbConfigurator UsingInbox(Action<MongoDbInboxBuilder>? configure)
    {
        _inboxBuilder = new MongoDbInboxBuilder();
        configure?.Invoke(_inboxBuilder);
        return this;
    }

    private MongoDbDistributedLockBuilder? _distributedLockBuilder;

    /// <summary>
    /// Configures MongoDB-based distributed lock mechanism.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="MongoDbDistributedLockBuilder"/>.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for fluent chaining.</returns>
    /// <remarks>
    /// If no configuration is provided, uses default settings with the current MongoDB connection.
    /// </remarks>
    public MongoDbConfigurator UsingDistributedLock(Action<MongoDbDistributedLockBuilder>? configure)
    {
        _distributedLockBuilder = new MongoDbDistributedLockBuilder();
        configure?.Invoke(_distributedLockBuilder);
        return this;
    }

    private MongoDbLuggageStoreBuilder? _luggageStoreOptionsBuilder;

    /// <summary>
    /// Configures MongoDB-based luggage store for message attachments or large payloads.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="MongoDbLuggageStoreBuilder"/>.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for fluent chaining.</returns>
    /// <remarks>
    /// The luggage store is used for persisting large message payloads or attachments separately from the main message body.
    /// </remarks>
    public MongoDbConfigurator UsingLuggageStore(Action<MongoDbLuggageStoreBuilder>? configure)
    {
        _luggageStoreOptionsBuilder = new MongoDbLuggageStoreBuilder();
        configure?.Invoke(_luggageStoreOptionsBuilder);
        return this;
    }

    /// <summary>
    /// Internal method that registers MongoDB-based services into the <see cref="IBrighterConfigurator"/>.
    /// Validates required configuration and applies MongoDB-specific implementations.
    /// </summary>
    /// <param name="configurator">The Brighter configurator to extend.</param>
    /// <returns>The updated <see cref="IBrighterConfigurator"/> instance with MongoDB services registered.</returns>
    /// <exception cref="ConfigurationException">Thrown when no MongoDB configuration has been provided.</exception>
    internal IBrighterConfigurator AddMongoDb(IBrighterConfigurator configurator)
    {
        if (_outboxBuilder != null)
        {
            configurator.SetOutbox(_outboxBuilder
                .SetConnectionIfIsMissing(_configurator)
                .Build());
        }

        if (_inboxBuilder != null)
        {
            configurator.SetInbox(_inboxBuilder
                .SetConnectionIfIsMissing(_configurator)
                .Build());
        }

        if (_distributedLockBuilder != null)
        {
            configurator.SetDistributedLock(_distributedLockBuilder
                .SetConnectionIfIsMissing(_configurator)
                .Build());
        }

        if (_luggageStoreOptionsBuilder != null)
        {
            configurator.SetLuggageStore(_luggageStoreOptionsBuilder.Build());
        }

        return configurator;
    }
}