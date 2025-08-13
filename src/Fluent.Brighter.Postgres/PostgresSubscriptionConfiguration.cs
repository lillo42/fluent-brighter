using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter.Postgres;

public class PostgresSubscriptionConfiguration(PostgresChannelFactory channelFactory)
{
    internal List<PostgresSubscription> Subscriptions { get; } = [];

    public PostgresSubscriptionConfiguration AddPostgresSubscription(PostgresSubscription subscription)
    {
        Subscriptions.Add(subscription);
        return this;
    }
    
    public PostgresSubscriptionConfiguration AddPostgresSubscription(Action<PostgresSubscriptionBuilder> configure)
    {
        var builder = new PostgresSubscriptionBuilder();
        configure(builder);
        builder.SetChannelFactory(channelFactory);
        return AddPostgresSubscription(builder.Build());
    }


    public PostgresSubscriptionConfiguration AddPostgresSubscription<TRequest>(
        Action<PostgresSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        return AddPostgresSubscription(cfg =>
        {
            cfg.SetDataType(typeof(TRequest));
            configure(cfg);
        });
    }
}