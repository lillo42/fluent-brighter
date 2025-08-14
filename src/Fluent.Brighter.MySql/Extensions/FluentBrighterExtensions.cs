using System;

using Fluent.Brighter.MySql;

namespace Fluent.Brighter;

public static class FluentBrighterExtensions
{
    public static FluentBrighterBuilder UsingSqlite(this FluentBrighterBuilder builder,
        Action<MySqlConfigurator> configure)
    {
        var configurator = new MySqlConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}