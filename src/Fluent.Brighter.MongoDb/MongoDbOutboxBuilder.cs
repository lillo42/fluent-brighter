using Paramore.Brighter;
using Paramore.Brighter.MongoDb;
using Paramore.Brighter.Outbox.MongoDb;

namespace Fluent.Brighter.MongoDb;

public sealed class MongoDbOutboxBuilder
{
    private IAmAMongoDbConfiguration? _configuration;

    public MongoDbOutboxBuilder SetConfiguration(IAmAMongoDbConfiguration  configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmAMongoDbConnectionProvider? _connectionProvider;

    public MongoDbOutboxBuilder SetConnectionProvider(IAmAMongoDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }

    private MongoDbCollectionConfiguration? _collection;

    public MongoDbOutboxBuilder SetCollection(MongoDbCollectionConfiguration collection)
    {
        _collection = collection;
        return this;
    }
    
    internal MongoDbOutbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }

        if (_collection != null)
        {
            _configuration.Outbox = _collection;
        }
        
        return _connectionProvider == null ? new MongoDbOutbox(_configuration) : new MongoDbOutbox(_connectionProvider, _configuration);
    }
}