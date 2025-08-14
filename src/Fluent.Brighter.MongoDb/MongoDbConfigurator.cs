using System;

using Paramore.Brighter;
using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter.MongoDb;

public sealed class MongoDbConfigurator
{
    private IAmAMongoDbConfiguration? _configuration;
    private Action<FluentBrighterBuilder> _action = _ => { };
    
    public MongoDbConfigurator SetConnection(Action<MongoDbConfigurationBuilder> configuration)
    {
        var builder = new MongoDbConfigurationBuilder(); 
        configuration(builder);
        return SetConnection(builder.Build());
    }

    public MongoDbConfigurator SetConnection(IAmAMongoDbConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    public MongoDbConfigurator UseInbox()
    {
        _action += fluent => fluent.Subscriptions(s => s.UseMongoDbInbox(_configuration!));
        return this;
    }

    public MongoDbConfigurator UseInbox(string collectionName)
    {
        _action += fluent => fluent.Subscriptions(x => x
            .UseMongoDbInbox(cfg => cfg
                .SetCollection(collectionName)
                .SetConfiguration(_configuration!)));
        return this;
    }

    public MongoDbConfigurator UseInbox(Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        _action += fluent => fluent.Subscriptions(x => x
            .UseMongoDbInbox(cfg => cfg
                .SetCollection(configure)
                .SetConfiguration(_configuration!)));
        return this;
    }
    
    public MongoDbConfigurator UseOutbox()
    {
        _action += fluent => fluent.Producers(x => x.UseMongoDbOutbox(_configuration!));
        return this;
    }
    
    public MongoDbConfigurator UseOutbox(string collectionName)
    {
        _action += fluent => fluent.Producers(x => x
            .UseMongoDbOutbox(cfg => cfg
                .SetCollection(collectionName)
                .SetConfiguration(_configuration!)));
        return this;
    }

    public MongoDbConfigurator UseOutbox(Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        _action += fluent => fluent.Producers(x => x
            .UseMongoDbOutbox(cfg => cfg
                .SetCollection(configure)
                .SetConfiguration(_configuration!)));
        return this;
    }
    
    public MongoDbConfigurator UseDistributedLock()
    {
        _action += fluent => fluent.Producers(x => x.UseMongoDbDistributedLock(_configuration!));
        return this;
    }
    
    public MongoDbConfigurator UseDistributedLock(string collectionName)
    {
        _action += fluent => fluent.Producers(x => x
            .UseMongoDbDistributedLock(cfg => cfg
                .SetCollection(collectionName)
                .SetConfiguration(_configuration!)));
        return this;
    }

    public MongoDbConfigurator UseDistributedLock(Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        _action += fluent => fluent.Producers(x => x
            .UseMongoDbDistributedLock(cfg => cfg
                .SetCollection(configure)
                .SetConfiguration(_configuration!)));
        return this;
    }

    internal void SetFluentBrighter(FluentBrighterBuilder fluentBrighter)
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("No RelationalDatabaseConfiguration was set");
        }
        
        _action(fluentBrighter);
    }
}