using System;

using Fluent.Brighter.Sqlite;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class ConsumerBuilderExtensions
{
    public static ConsumerBuilder UseSqliteInbox(this ConsumerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UseSqliteInbox(configuration.Build());
    }

    public static ConsumerBuilder UseSqliteInbox(this ConsumerBuilder builder, IAmARelationalDatabaseConfiguration configuration)
        => builder.UseSqliteInbox(cfg => cfg.SetConfiguration(configuration));

    public static ConsumerBuilder UseSqliteInbox(this ConsumerBuilder builder, Action<SqliteInboxBuilder> configure)
    {
        var inbox = new SqliteInboxBuilder();
        configure(inbox);
        return builder.SetInbox(cfg => cfg.SetInbox(inbox.Build()));
    }
}