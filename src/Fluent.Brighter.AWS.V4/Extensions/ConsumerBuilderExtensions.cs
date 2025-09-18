using System;

using Amazon.Runtime.Internal;

using Paramore.Brighter.Inbox.DynamoDB.V4;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4.Extensions;

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

    public static ConsumerBuilder AddAwsChannelFactory(this ConsumerBuilder builder, Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var subscription = new AWSMessagingGatewayConnectionBuilder();
        configure(subscription);
        return builder.AddChannelFactory(new ChannelFactory(subscription.Build()));
    }

    public static ConsumerBuilder UseDynamoDbInbox(this ConsumerBuilder builder, Action<DynamoDbInboxBuilder> configure)
    {
        var configuration = new DynamoDbInboxBuilder();
        configure(configuration);
        return builder.UseDynamoDbInbox(configuration.Build());
    }
    
    public static ConsumerBuilder UseDynamoDbInbox(this ConsumerBuilder builder, AWSMessagingGatewayConnection connection)
    {
        return builder.UseDynamoDbInbox(cfg => cfg.SetConnection(connection));
    }

    public static ConsumerBuilder UseDynamoDbInbox(this ConsumerBuilder builder, DynamoDbInbox inbox) 
        => builder.SetInbox(cfg => cfg.SetInbox(inbox));
}