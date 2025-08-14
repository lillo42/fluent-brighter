using System;

using Fluent.Brighter.RMQ.Sync;

namespace Fluent.Brighter;

public static class ProducersExtensions
{
    public static ProducerBuilder AddRabbitMqPublication(
        this ProducerBuilder builder,
        Action<RmqMessageProducerFactoryBuilder> configure)
    {
        var factory = new RmqMessageProducerFactoryBuilder();
        configure(factory);
        return builder.AddMessageProducerFactory(factory.Build());
    }
}