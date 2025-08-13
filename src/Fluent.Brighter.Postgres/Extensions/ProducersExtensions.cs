using System;

using Fluent.Brighter.Postgres;

using Paramore.Brighter;
using Paramore.Brighter.Locking.PostgresSql;

namespace Fluent.Brighter;

public static class ProducersExtensions
{
    public static ProducerBuilder AddPostgresPublication(this ProducerBuilder producer,
        Action<PostgresMessageProducerFactoryBuilder> configuration)
    {
        var builder = new PostgresMessageProducerFactoryBuilder();
        configuration(builder);

        return producer.AddMessageProducerFactory(builder.Build());
    }

    public static ProducerBuilder UsePostgresOutbox(this ProducerBuilder producer,
        RelationalDatabaseConfiguration configuration)
    {
        return producer.UsePostgresOutbox(cfg => cfg.SetConfiguration(configuration));
    }
    
    public static ProducerBuilder UsePostgresOutbox(this ProducerBuilder producer,
        Action<PostgresOutboxBuilder> configuration)
    {
        var builder = new PostgresOutboxBuilder();
        configuration(builder);
        producer.SetOutbox(builder.Build());
        return producer;
    }

    public static ProducerBuilder UsePostgresDistributedLock(this ProducerBuilder producer, IAmARelationalDatabaseConfiguration configuration)
        => producer.UsePostgresDistributedLock(configuration.ConnectionString);
    public static ProducerBuilder UsePostgresDistributedLock(this ProducerBuilder producer, string connectionString) 
        => producer.SetDistributedLock(new PostgresLockingProvider(new PostgresLockingProviderOptions(connectionString)));
}