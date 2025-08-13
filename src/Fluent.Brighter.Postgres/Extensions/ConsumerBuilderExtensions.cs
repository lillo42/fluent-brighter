using System;

using Fluent.Brighter.Postgres;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter;

public static class ConsumerBuilderExtensions
{
    public static ConsumerBuilder AddPostgresSubscription(this ConsumerBuilder consumer,
        PostgresSubscription subscription) 
        => consumer.AddSubscription(subscription);
    
    public static ConsumerBuilder AddPostgresSubscription(this ConsumerBuilder consumer,
            Action<PostgresSubscriptionBuilder> configuration)
    {
        var subscriptionBuilder = new PostgresSubscriptionBuilder();
        configuration(subscriptionBuilder);
        return consumer.AddSubscription(subscriptionBuilder.Build());
    }
    
    public static ConsumerBuilder AddPostgresSubscription<TRequest>(this ConsumerBuilder consumer,
        Action<PostgresSubscriptionBuilder> configuration)
        where TRequest : class, IRequest
    {
        var subscriptionBuilder = new PostgresSubscriptionBuilder();
        subscriptionBuilder.SetDataType(typeof(TRequest));
        configuration(subscriptionBuilder);
        return consumer.AddSubscription(subscriptionBuilder.Build());
    }

    public static ConsumerBuilder UsePostgresInbox(this ConsumerBuilder consumer, 
        IAmARelationalDatabaseConfiguration configuration)
        => consumer.UsePostgresInbox(cfg => cfg.SetConfiguration(configuration));

    public static ConsumerBuilder UsePostgresInbox(this ConsumerBuilder consumer, Action<PostgresInboxBuilder> configuration)
    {
        var builder = new PostgresInboxBuilder();
        configuration(builder);
        return consumer.SetInbox(cfg => cfg.SetInbox(builder.Build()));
    }

    public static ConsumerBuilder AddPostgresChannelFactory(this ConsumerBuilder builder, RelationalDatabaseConfiguration  configuration)
    {
        return builder
            .AddChannelFactory(new PostgresChannelFactory(new PostgresMessagingGatewayConnection(configuration)));
    }
}