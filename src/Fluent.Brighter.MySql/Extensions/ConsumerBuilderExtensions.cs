using System;

using Fluent.Brighter.MySql;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class ConsumerBuilderExtensions
{
    public static ConsumerBuilder UseMySqlInbox(this ConsumerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UseMySqlInbox(configuration.Build());
    }

    public static ConsumerBuilder UseMySqlInbox(this ConsumerBuilder builder, IAmARelationalDatabaseConfiguration configuration)
        => builder.UseMySqlInbox(cfg => cfg.SetConfiguration(configuration));

    public static ConsumerBuilder UseMySqlInbox(this ConsumerBuilder builder, Action<MySqlInboxBuilder> configure)
    {
        var inbox = new MySqlInboxBuilder();
        configure(inbox);
        return builder.SetInbox(cfg => cfg.SetInbox(inbox.Build()));
    }
}