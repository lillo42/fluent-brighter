using Paramore.Brighter;
using Paramore.Brighter.Inbox.MongoDb;
using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter.MongoDb;

public sealed class MongoDbInboxBuilder
{
    private IAmAMongoDbConfiguration? _configuration;

    public MongoDbInboxBuilder SetConfiguration(IAmAMongoDbConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmAMongoDbConnectionProvider? _connectionProvider;

    public MongoDbInboxBuilder SetConnectionProvider(IAmAMongoDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    private MongoDbCollectionConfiguration? _collection;

    public MongoDbInboxBuilder SetCollection(MongoDbCollectionConfiguration collection)
    {
        _collection = collection;
        return this;
    }
    
    internal MongoDbInbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }
        
        if (_collection != null)
        {
            _configuration.Inbox = _collection;
        }

        return _connectionProvider == null ? new MongoDbInbox(_configuration) : new MongoDbInbox(_connectionProvider, _configuration);
    }
}