using System;

using Fluent.Brighter.MySql;

using Paramore.Brighter;
using Paramore.Brighter.Locking.MySql;
using Paramore.Brighter.MySql;

namespace Fluent.Brighter;

public static class ProducersExtensions
{
    public static ProducerBuilder UseMySqlOutbox(this ProducerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UseMySqlOutbox(configuration.Build());
    }

    public static ProducerBuilder UseMySqlOutbox(this ProducerBuilder builder,
        RelationalDatabaseConfiguration configuration)
    {
        return builder.UseMySqlOutbox(cfg => cfg.SetConfiguration(configuration));
    }
    
    public static ProducerBuilder UseMySqlOutbox(this ProducerBuilder builder,
        Action<MySqlOutboxBuilder> configuration)
    {
        var outbox = new MySqlOutboxBuilder();
        configuration(outbox);
        builder.SetOutbox(outbox.Build());
        return builder;
    }
    
    public static ProducerBuilder UseMySqlDistributedLock(this ProducerBuilder builder, IAmARelationalDatabaseConfiguration configuration)
        => builder.SetDistributedLock(new MySqlLockingProvider(new MySqlConnectionProvider(configuration)));
    
    public static ProducerBuilder UseMySqlDistributedLock(this ProducerBuilder builder, string connectionString) 
        => builder.UseMySqlDistributedLock(new RelationalDatabaseConfiguration(connectionString));
}