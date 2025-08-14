using System;

using Fluent.Brighter.MongoDb;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter;

public static class ProducersExtensions
{
    public static ProducerBuilder UseMongoDbOutbox(this ProducerBuilder builder, Action<MongoDbConfigurationBuilder> configure)
    {
        var configuration = new MongoDbConfigurationBuilder();
        configure(configuration);
        return builder.UseMongoDbOutbox(configuration.Build());
    }

    public static ProducerBuilder UseMongoDbOutbox(this ProducerBuilder builder, IAmAMongoDbConfiguration  configuration)
    {
        return builder.UseMongoDbOutbox(cfg => cfg.SetConfiguration(configuration));
    }
    
    public static ProducerBuilder UseMongoDbOutbox(this ProducerBuilder builder,
        Action<MongoDbOutboxBuilder> configuration)
    {
        var outbox = new MongoDbOutboxBuilder();
        configuration(outbox);
        builder.SetOutbox(outbox.Build());
        return builder;
    }
    
    public static ProducerBuilder UseMongoDbDistributedLock(this ProducerBuilder builder, Action<MongoDbConfigurationBuilder> configure)
    {
        var configuration = new MongoDbConfigurationBuilder();
        configure(configuration);
        return builder.UseMongoDbDistributedLock(configuration.Build());
    }

    public static ProducerBuilder UseMongoDbDistributedLock(this ProducerBuilder builder, IAmAMongoDbConfiguration configuration)
    {
        return builder.UseMongoDbDistributedLock(cfg => cfg.SetConfiguration(configuration));
    }
    
    public static ProducerBuilder UseMongoDbDistributedLock(this ProducerBuilder builder,
        Action<MongoDbLockingBuilder> configuration)
    {
        var locking = new MongoDbLockingBuilder();
        configuration(locking);
        builder.SetDistributedLock(locking.Build());
        return builder;
    }
}