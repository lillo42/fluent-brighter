using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

public sealed class SqsSubscriptionConfigurator(ChannelFactory channelFactory)
{
    internal List<SqsSubscription> Subscriptions { get; } = [];

    public SqsSubscriptionConfigurator AddSubscription(SqsSubscription subscription)
    {
        Subscriptions.Add(subscription);
        return this;
    }

    public SqsSubscriptionConfigurator AddSubscription(Action<SqsSubscriptionBuilder> configure)
    {
        var sub = new  SqsSubscriptionBuilder();
        sub.SetChannelFactory(channelFactory);
        configure(sub);
        return AddSubscription(sub.Build());
    }
    
    public SqsSubscriptionConfigurator AddSubscription<TRequest>(Action<SqsSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        return AddSubscription(cfg =>
        {
            cfg.SetDataType(typeof(TRequest));
            configure(cfg);
        });
    }
}