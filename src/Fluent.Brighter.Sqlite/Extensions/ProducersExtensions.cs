using System;

using Fluent.Brighter.Sqlite;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class ProducersExtensions
{
    public static ProducerBuilder UseSqliteOutbox(this ProducerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UseSqliteOutbox(configuration.Build());
    }

    public static ProducerBuilder UseSqliteOutbox(this ProducerBuilder builder,
        RelationalDatabaseConfiguration configuration)
    {
        return builder.UseSqliteOutbox(cfg => cfg.SetConfiguration(configuration));
    }
    
    public static ProducerBuilder UseSqliteOutbox(this ProducerBuilder builder,
        Action<SqliteOutboxBuilder> configuration)
    {
        var outbox = new SqliteOutboxBuilder();
        configuration(outbox);
        builder.SetOutbox(outbox.Build());
        return builder;
    }
}