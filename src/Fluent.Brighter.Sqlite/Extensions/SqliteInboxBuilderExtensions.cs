using System;

using Fluent.Brighter.Sqlite;

namespace Fluent.Brighter;

public static class SqliteInboxBuilderExtensions
{
    public static SqliteInboxBuilder SetConfiguration(this SqliteInboxBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}