using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter.Kafka;

public sealed class KafkaSubscriptionConfigurator(ChannelFactory channelFactory)
{
    internal List<KafkaSubscription> Subscriptions { get; } = [];

    public KafkaSubscriptionConfigurator AddSubscription(KafkaSubscription subscription)
    {
        Subscriptions.Add(subscription);
        return this;
    }

    public KafkaSubscriptionConfigurator AddSubscription(Action<KafkaSubscriptionBuilder> configure)
    {
        var sub = new KafkaSubscriptionBuilder();
        sub.SetChannelFactory(channelFactory);
        configure(sub);
        return AddSubscription(sub.Build());
    }
    
    public KafkaSubscriptionConfigurator AddSubscription<TRequest>(Action<KafkaSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        return AddSubscription(cfg =>
        {
            cfg.SetDataType(typeof(TRequest));
            configure(cfg);
        });
    }
}