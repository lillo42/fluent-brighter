using System;

using Fluent.Brighter.RMQ.Async;

namespace Fluent.Brighter;

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