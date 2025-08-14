using System;

using Fluent.Brighter.Postgres;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter;

public static class ConsumerBuilderExtensions
{
    public static ConsumerBuilder AddPostgresSubscription(this ConsumerBuilder builder, PostgresSubscription subscription) 
        => builder.AddSubscription(subscription);
    
    public static ConsumerBuilder AddPostgresSubscription(this ConsumerBuilder builder,
            Action<PostgresSubscriptionBuilder> configure)
    {
        var sub = new PostgresSubscriptionBuilder();
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }
    
    public static ConsumerBuilder AddPostgresSubscription<TRequest>(this ConsumerBuilder builder,
        Action<PostgresSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        var sub = new PostgresSubscriptionBuilder();
        sub.SetDataType(typeof(TRequest));
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }
    
    
    public static ConsumerBuilder UsePostgresInbox(this ConsumerBuilder builder, 
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UsePostgresInbox(configuration.Build());
    }

    public static ConsumerBuilder UsePostgresInbox(this ConsumerBuilder builder, IAmARelationalDatabaseConfiguration configuration)
        => builder.UsePostgresInbox(cfg => cfg.SetConfiguration(configuration));

    public static ConsumerBuilder UsePostgresInbox(this ConsumerBuilder builder, Action<PostgresInboxBuilder> configure)
    {
        var inbox = new PostgresInboxBuilder();
        configure(inbox);
        return builder.SetInbox(cfg => cfg.SetInbox(inbox.Build()));
    }

    public static ConsumerBuilder AddPostgresChannelFactory(this ConsumerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.AddPostgresChannelFactory(configuration.Build());
    }

    public static ConsumerBuilder AddPostgresChannelFactory(this ConsumerBuilder builder, RelationalDatabaseConfiguration  configuration)
    {
        return builder
            .AddChannelFactory(new PostgresChannelFactory(new PostgresMessagingGatewayConnection(configuration)));
    }
}