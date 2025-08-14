using Paramore.Brighter;
using Paramore.Brighter.Locking.MongoDb;
using Paramore.Brighter.MongoDb;
using Paramore.Brighter.Outbox.MongoDb;

namespace Fluent.Brighter.MongoDb;

public sealed class MongoDbLockingBuilder
{
    private IAmAMongoDbConfiguration? _configuration;

    public MongoDbLockingBuilder SetConfiguration(IAmAMongoDbConfiguration  configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmAMongoDbConnectionProvider? _connectionProvider;

    public MongoDbLockingBuilder SetConnectionProvider(IAmAMongoDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }

    private MongoDbCollectionConfiguration? _collection;

    public MongoDbLockingBuilder SetCollection(MongoDbCollectionConfiguration collection)
    {
        _collection = collection;
        return this;
    }
    
    internal MongoDbLockingProvider Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }

        if (_collection != null)
        {
            _configuration.Locking = _collection;
        }
        
        return _connectionProvider == null ? new MongoDbLockingProvider(_configuration) : new MongoDbLockingProvider(_connectionProvider, _configuration);
    }
}