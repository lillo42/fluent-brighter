using System;

using Fluent.Brighter.Kafka;

namespace Fluent.Brighter;

public static class ProducersExtensions
{
    public static ProducerBuilder AddKafkaPublication(
        this ProducerBuilder builder,
        Action<KafkaMessageProducerFactoryBuilder> configure)
    {
        var factory = new KafkaMessageProducerFactoryBuilder();
        configure(factory);
        return builder.AddMessageProducerFactory(factory.Build());
    }
}