using System;

using Fluent.Brighter.Postgres;

namespace Fluent.Brighter;

public static class PostgresOutboxBuilderExtensions
{
    public static PostgresOutboxBuilder SetConfiguration(this PostgresOutboxBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}