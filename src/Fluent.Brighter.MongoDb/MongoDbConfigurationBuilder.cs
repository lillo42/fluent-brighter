using System;

using MongoDB.Driver;

using Paramore.Brighter;
using Paramore.Brighter.MongoDb;
using Paramore.Brighter.Observability;

namespace Fluent.Brighter.MongoDb;

/// <summary>
/// Provides a fluent interface for configuring MongoDB integration with Brighter, including client, database,
/// instrumentation, and collection settings for outbox, inbox, and locking.
/// </summary>
public class MongoDbConfigurationBuilder
{
    private IMongoClient? _client;

    /// <summary>
    /// Sets the MongoDB client to use for database operations.
    /// </summary>
    /// <param name="client">The configured <see cref="IMongoClient"/> instance.</param>
    /// <returns>The current <see cref="MongoDbConfigurationBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="client"/> is null.</exception>
    public MongoDbConfigurationBuilder SetClient(IMongoClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        return this;
    }

    /// <summary>
    /// Configures the MongoDB client using a connection string.
    /// </summary>
    /// <param name="connectionString">The MongoDB connection string.</param>
    /// <returns>The current <see cref="MongoDbConfigurationBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="connectionString"/> is null or empty.</exception>
    public MongoDbConfigurationBuilder SetConnectionString(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
        }
        
        _client = new MongoClient(connectionString);
        return this;
    }

    private string? _databaseName;

    /// <summary>
    /// Specifies the name of the MongoDB database to use.
    /// </summary>
    /// <param name="databaseName">The name of the database.</param>
    /// <returns>The current <see cref="MongoDbConfigurationBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="databaseName"/> is null or empty.</exception>
    public MongoDbConfigurationBuilder SetDatabaseName(string databaseName)
    {
        if (string.IsNullOrEmpty(databaseName))
        {
            throw new ArgumentException("Database name cannot be null or empty.", nameof(databaseName));
        }
        
        _databaseName = databaseName;
        return this;
    }

    private TimeProvider _timeProvider = TimeProvider.System;

    /// <summary>
    /// Sets the <see cref="TimeProvider"/> used for time-related operations (e.g., message timestamps).
    /// Defaults to <see cref="TimeProvider.System"/> if not specified.
    /// </summary>
    /// <param name="timeProvider">The time provider implementation.</param>
    /// <returns>The current <see cref="MongoDbConfigurationBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="timeProvider"/> is null.</exception>
    public MongoDbConfigurationBuilder SetTimeProvider(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        return this;
    }

    private MongoDatabaseSettings? _databaseSettings;

    /// <summary>
    /// Configures advanced MongoDB database settings.
    /// </summary>
    /// <param name="databaseSettings">The database settings to apply.</param>
    /// <returns>The current <see cref="MongoDbConfigurationBuilder"/> instance for method chaining.</returns>
    public MongoDbConfigurationBuilder SetDatabaseSettings(MongoDatabaseSettings databaseSettings)
    {
        _databaseSettings = databaseSettings;
        return this;
    }

    private InstrumentationOptions _instrumentationOptions = InstrumentationOptions.All;

    /// <summary>
    /// Specifies which instrumentation features (e.g., metrics, tracing) should be enabled.
    /// Defaults to <see cref="InstrumentationOptions.All"/>.
    /// </summary>
    /// <param name="instrumentationOptions">The desired instrumentation options.</param>
    /// <returns>The current <see cref="MongoDbConfigurationBuilder"/> instance for method chaining.</returns>
    public MongoDbConfigurationBuilder SetInstrumentation(InstrumentationOptions instrumentationOptions)
    {
        _instrumentationOptions = instrumentationOptions;
        return this;
    }

    private MongoDbCollectionConfiguration? _outbox;

    /// <summary>
    /// Configures the MongoDB collection used for the outbox pattern (storing outgoing messages).
    /// </summary>
    /// <param name="outboxConfig">Configuration for the outbox collection.</param>
    /// <returns>The current <see cref="MongoDbConfigurationBuilder"/> instance for method chaining.</returns>
    public MongoDbConfigurationBuilder SetOutbox(MongoDbCollectionConfiguration outboxConfig)
    {
        _outbox = outboxConfig;
        return this;
    }

    private MongoDbCollectionConfiguration? _inbox;

    /// <summary>
    /// Configures the MongoDB collection used for the inbox pattern (deduplicating incoming messages).
    /// </summary>
    /// <param name="inboxConfig">Configuration for the inbox collection.</param>
    /// <returns>The current <see cref="MongoDbConfigurationBuilder"/> instance for method chaining.</returns>
    public MongoDbConfigurationBuilder SetInbox(MongoDbCollectionConfiguration inboxConfig)
    {
        _inbox = inboxConfig;
        return this;
    }

    private MongoDbCollectionConfiguration? _locking;

    /// <summary>
    /// Configures the MongoDB collection used for distributed locking (e.g., to prevent duplicate processing).
    /// </summary>
    /// <param name="lockingConfig">Configuration for the locking collection.</param>
    /// <returns>The current <see cref="MongoDbConfigurationBuilder"/> instance for method chaining.</returns>
    public MongoDbConfigurationBuilder SetLocking(MongoDbCollectionConfiguration lockingConfig)
    {
        _locking = lockingConfig;
        return this;
    }

    /// <summary>
    /// Builds the final <see cref="MongoDbConfiguration"/> instance based on the provided settings.
    /// </summary>
    /// <returns>A configured <see cref="MongoDbConfiguration"/> object.</returns>
    /// <exception cref="ConfigurationException">
    /// Thrown if neither <see cref="SetClient"/> nor <see cref="SetConnectionString"/> has been called,
    /// or if <see cref="SetDatabaseName"/> has not been called.
    /// </exception>
    internal MongoDbConfiguration Build()
    {
        if (_client == null)
        {
            throw new ConfigurationException("MongoDB client must be configured using SetClient or SetConnectionString.");
        }

        if (string.IsNullOrEmpty(_databaseName))
        {
            throw new ConfigurationException("Database name must be set using SetDatabaseName.");
        }

        var config = new MongoDbConfiguration(_client, _databaseName!)
        {
            TimeProvider = _timeProvider,
            DatabaseSettings = _databaseSettings,
            InstrumentationOptions = _instrumentationOptions,
            Outbox = _outbox,
            Inbox = _inbox,
            Locking = _locking
        };

        return config;
    }
}