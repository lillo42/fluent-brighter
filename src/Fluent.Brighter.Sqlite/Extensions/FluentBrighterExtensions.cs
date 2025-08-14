using System;

using Fluent.Brighter.Sqlite;

namespace Fluent.Brighter;

public static class FluentBrighterExtensions
{
    public static FluentBrighterBuilder UsingSqlite(this FluentBrighterBuilder builder,
        Action<SqliteConfigurator> configure)
    {
        var configurator = new SqliteConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}