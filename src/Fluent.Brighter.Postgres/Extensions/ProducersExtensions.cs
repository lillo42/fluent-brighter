using System;

using Fluent.Brighter.Postgres;

using Paramore.Brighter;
using Paramore.Brighter.Locking.PostgresSql;

namespace Fluent.Brighter;

public static class ProducersExtensions
{
    public static ProducerBuilder AddPostgresPublication(this ProducerBuilder builder,
        Action<PostgresMessageProducerFactoryBuilder> configure)
    {
        var factory = new PostgresMessageProducerFactoryBuilder();
        configure(factory);

        return builder.AddMessageProducerFactory(factory.Build());
    }


    public static ProducerBuilder UsePostgresOutbox(this ProducerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UsePostgresOutbox(configuration.Build());
    }

    public static ProducerBuilder UsePostgresOutbox(this ProducerBuilder builder,
        RelationalDatabaseConfiguration configuration)
    {
        return builder.UsePostgresOutbox(cfg => cfg.SetConfiguration(configuration));
    }
    
    public static ProducerBuilder UsePostgresOutbox(this ProducerBuilder builder,
        Action<PostgresOutboxBuilder> configuration)
    {
        var outbox = new PostgresOutboxBuilder();
        configuration(outbox);
        builder.SetOutbox(outbox.Build());
        return builder;
    }

    public static ProducerBuilder UsePostgresDistributedLock(this ProducerBuilder builder, IAmARelationalDatabaseConfiguration configuration)
        => builder.UsePostgresDistributedLock(configuration.ConnectionString);
    
    public static ProducerBuilder UsePostgresDistributedLock(this ProducerBuilder builder, string connectionString) 
        => builder.SetDistributedLock(new PostgresLockingProvider(new PostgresLockingProviderOptions(connectionString)));
}