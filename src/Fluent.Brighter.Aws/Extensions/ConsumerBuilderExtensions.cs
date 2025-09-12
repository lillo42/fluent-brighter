using System;

using Amazon.Runtime.Internal;

using Fluent.Brighter.Aws;

using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter;

public static class ConsumerBuilderExtensions
{
    public static ConsumerBuilder AddSqsSubscription(this ConsumerBuilder builder, SqsSubscription subscription)
        => builder.AddSubscription(subscription);

    public static ConsumerBuilder AddSqsSubscription(this ConsumerBuilder builder,
        Action<SqsSubscriptionBuilder> configure)
    {
        var subscription = new SqsSubscriptionBuilder();
        configure(subscription);
        return builder.AddSubscription(subscription.Build());
    }
    
    public static ConsumerBuilder AddSqsSubscription<TRequest>(this ConsumerBuilder builder, Action<SqsSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        var subscription = new SqsSubscriptionBuilder();
        subscription.SetDataType(typeof(TRequest));
        configure(subscription);
        return builder.AddSubscription(subscription.Build());
    }
    
    public static ConsumerBuilder AddAwsChannelFactory(this ConsumerBuilder builder, AWSMessagingGatewayConnection configure) 
        => builder.AddChannelFactory(new ChannelFactory(configure));

    public static ConsumerBuilder AddAwsChannelFactory(this ConsumerBuilder builder, Action<AWSMessagingGatewayConnectionBuidler> configure)
    {
        var subscription = new AWSMessagingGatewayConnectionBuidler();
        configure(subscription);
        return builder.AddChannelFactory(new ChannelFactory(subscription.Build()));
    }
}