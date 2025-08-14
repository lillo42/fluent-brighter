using System;

using Fluent.Brighter.Kafka;

namespace Fluent.Brighter;

public static class FluentBrighterExtensions
{
    public static FluentBrighterBuilder UsingKafka(this FluentBrighterBuilder builder,
        Action<KafkaConfigurator> configure)
    {
        var configurator = new KafkaConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}