using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Sync;

namespace Fluent.Brighter.RMQ.Sync;

public class RabbitMqSubscriptionConfigurator(ChannelFactory channelFactory)
{
    internal List<RmqSubscription> Subscriptions { get; } = [];

    public RabbitMqSubscriptionConfigurator AddSubscription(RmqSubscription subscription)
    {
        Subscriptions.Add(subscription);
        return this;
    }

    public RabbitMqSubscriptionConfigurator AddSubscription(Action<RmqSubscriptionBuilder> configure)
    {
        var sub = new RmqSubscriptionBuilder();
        sub.SetChannelFactory(channelFactory);
        configure(sub);
        return AddSubscription(sub.Build());
    }
    
    public RabbitMqSubscriptionConfigurator AddSubscription<TRequest>(Action<RmqSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        return AddSubscription(cfg =>
        {
            cfg.SetDataType(typeof(TRequest));
            configure(cfg);
        });
    }
}