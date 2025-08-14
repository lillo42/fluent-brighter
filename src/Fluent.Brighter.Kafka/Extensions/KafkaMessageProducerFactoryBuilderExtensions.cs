using System;

using Fluent.Brighter.Kafka;

namespace Fluent.Brighter;

public static class KafkaMessageProducerFactoryBuilderExtensions
{
    public static KafkaMessageProducerFactoryBuilder SetConfiguration(this KafkaMessageProducerFactoryBuilder builder,
        Action<KafkaMessagingGatewayConfigurationBuilder> configure)
    {
        var configuration = new KafkaMessagingGatewayConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }

}