using System;

using Fluent.Brighter.Kafka;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter;

public static class ConsumerBuilderExtensions
{
    public static ConsumerBuilder AddKafkaSubscription(this ConsumerBuilder builder, KafkaSubscription subscription)
        => builder.AddSubscription(subscription);
    
    public static ConsumerBuilder AddKafkaSubscription(this ConsumerBuilder builder, 
        Action<KafkaSubscriptionBuilder> configure)
    {
        var sub = new KafkaSubscriptionBuilder();
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }
    
    public static ConsumerBuilder AddKafkaSubscription<TRequest>(this ConsumerBuilder builder, 
        Action<KafkaSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        var sub = new KafkaSubscriptionBuilder();
        sub.SetDataType(typeof(TRequest));
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }

    public static ConsumerBuilder AddKafkaChannelFactory(this ConsumerBuilder builder,
        Action<KafkaMessagingGatewayConfigurationBuilder> configure)
    {
        var connection = new KafkaMessagingGatewayConfigurationBuilder();
        configure(connection);
        return builder.AddKafkaChannelFactory(connection.Build());
    }
    
    public static ConsumerBuilder AddKafkaChannelFactory(this ConsumerBuilder builder, KafkaMessagingGatewayConfiguration connection)
    {
        return builder
            .AddChannelFactory(new ChannelFactory(new KafkaMessageConsumerFactory(connection)));
    }
}