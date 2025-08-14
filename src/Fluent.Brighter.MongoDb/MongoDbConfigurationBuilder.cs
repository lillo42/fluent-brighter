using System;

using MongoDB.Driver;

using Paramore.Brighter;
using Paramore.Brighter.MongoDb;
using Paramore.Brighter.Observability;

namespace Fluent.Brighter.MongoDb;
public class MongoDbConfigurationBuilder
{
    private IMongoClient? _client;
    public MongoDbConfigurationBuilder SetClient(IMongoClient client)
    {
        _client = client;
        return this;
    }

    public MongoDbConfigurationBuilder SetConnectionString(string connectionString)
    {
        _client = new MongoClient(connectionString);
        return this;
    }

    private string? _databaseName;
    public MongoDbConfigurationBuilder SetDatabaseName(string databaseName)
    {
        _databaseName = databaseName;
        return this;
    }

    private TimeProvider _timeProvider = TimeProvider.System;
    public MongoDbConfigurationBuilder SetTimeProvider(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        return this;
    }

    private MongoDatabaseSettings? _databaseSettings;
    public MongoDbConfigurationBuilder SetDatabaseSettings(MongoDatabaseSettings databaseSettings)
    {
        _databaseSettings = databaseSettings;
        return this;
    }

    private InstrumentationOptions _instrumentationOptions = InstrumentationOptions.All;
    public MongoDbConfigurationBuilder SetInstrumentation(InstrumentationOptions instrumentationOptions)
    {
        _instrumentationOptions = instrumentationOptions;
        return this;
    }

    private MongoDbCollectionConfiguration? _outbox;
    public MongoDbConfigurationBuilder SetOutbox(MongoDbCollectionConfiguration outboxConfig)
    {
        _outbox = outboxConfig;
        return this;
    }

    private MongoDbCollectionConfiguration? _inbox;
    public MongoDbConfigurationBuilder SetInbox(MongoDbCollectionConfiguration inboxConfig)
    {
        _inbox = inboxConfig;
        return this;
    }

    private MongoDbCollectionConfiguration? _locking;
    public MongoDbConfigurationBuilder SetLocking(MongoDbCollectionConfiguration lockingConfig)
    {
        _locking = lockingConfig;
        return this;
    }

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