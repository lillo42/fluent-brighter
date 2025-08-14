using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter.Postgres;

public class PostgresSubscriptionConfigurator(PostgresChannelFactory channelFactory)
{
    internal List<PostgresSubscription> Subscriptions { get; } = [];

    public PostgresSubscriptionConfigurator AddSubscription(PostgresSubscription subscription)
    {
        Subscriptions.Add(subscription);
        return this;
    }
    
    public PostgresSubscriptionConfigurator AddSubscription(Action<PostgresSubscriptionBuilder> configure)
    {
        var builder = new PostgresSubscriptionBuilder();
        configure(builder);
        builder.SetChannelFactory(channelFactory);
        return AddSubscription(builder.Build());
    }


    public PostgresSubscriptionConfigurator AddSubscription<TRequest>(
        Action<PostgresSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        return AddSubscription(cfg =>
        {
            cfg.SetDataType(typeof(TRequest));
            configure(cfg);
        });
    }
}