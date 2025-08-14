using System;

using Fluent.Brighter.RMQ.Async;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Async;

namespace Fluent.Brighter;

public static class ConsumerBuilderExtensions
{
    public static ConsumerBuilder AddRabbitMqSubscription(this ConsumerBuilder builder, RmqSubscription subscription)
        => builder.AddSubscription(subscription);
    
    public static ConsumerBuilder AddRabbitMqSubscription(this ConsumerBuilder builder, 
        Action<RmqSubscriptionBuilder> configure)
    {
        var sub = new RmqSubscriptionBuilder();
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }
    
    public static ConsumerBuilder AddRabbitMqSubscription<TRequest>(this ConsumerBuilder builder, 
        Action<RmqSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        var sub = new RmqSubscriptionBuilder();
        sub.SetDataType(typeof(TRequest));
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }

    public static ConsumerBuilder AddRabbitMqChannelFactory(this ConsumerBuilder builder,
        Action<RmqMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new RmqMessagingGatewayConnectionBuilder();
        configure(connection);
        return builder.AddRabbitMqChannelFactory(connection.Build());
    }
    
    public static ConsumerBuilder AddRabbitMqChannelFactory(this ConsumerBuilder builder, RmqMessagingGatewayConnection connection)
    {
        return builder
            .AddChannelFactory(new ChannelFactory(new RmqMessageConsumerFactory(connection)));
    }
}