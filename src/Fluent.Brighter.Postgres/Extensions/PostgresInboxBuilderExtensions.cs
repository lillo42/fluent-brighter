using System;

using Fluent.Brighter.Postgres;

namespace Fluent.Brighter;

public static class PostgresInboxBuilderExtensions
{
    public static PostgresInboxBuilder SetConfiguration(this PostgresInboxBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}