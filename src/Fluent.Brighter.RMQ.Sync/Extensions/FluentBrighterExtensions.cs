using System;

namespace Fluent.Brighter.RMQ.Sync.Extensions;

public static class FluentBrighterExtensions
{
    public static FluentBrighterBuilder UsingRabbitMq(this FluentBrighterBuilder builder,
        Action<RabbitMqConfigurator> configure)
    {
        var configurator = new RabbitMqConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}