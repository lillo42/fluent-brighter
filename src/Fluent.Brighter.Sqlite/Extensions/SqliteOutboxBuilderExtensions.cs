using System;

using Fluent.Brighter.Sqlite;

namespace Fluent.Brighter;

public static class SqliteOutboxBuilderExtensions
{
    public static SqliteOutboxBuilder SetConfiguration(this SqliteOutboxBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}