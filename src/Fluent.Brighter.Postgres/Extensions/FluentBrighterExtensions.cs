using System;

using Fluent.Brighter.Postgres;

namespace Fluent.Brighter;

public static class FluentBrighterExtensions
{
    public static FluentBrighterBuilder UsePostgres(this FluentBrighterBuilder fluentBrighter,
        Action<PostgresConfigurator> configure)
    {
        var builder = new PostgresConfigurator();
        configure(builder);
        builder.SetFluentBrighter(fluentBrighter);
        return fluentBrighter;
    }
}