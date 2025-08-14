using System;

using Fluent.Brighter.Postgres;

namespace Fluent.Brighter;

public static class FluentBrighterExtensions
{
    public static FluentBrighterBuilder UsingPostgres(this FluentBrighterBuilder builder,
        Action<PostgresConfigurator> configure)
    {
        var configurator = new PostgresConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}