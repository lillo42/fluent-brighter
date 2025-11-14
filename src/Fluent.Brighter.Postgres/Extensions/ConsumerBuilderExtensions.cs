using System;

using Fluent.Brighter.Postgres;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="ConsumerBuilder"/> to configure PostgreSQL-based message consumers.
/// These extensions enable easy setup of PostgreSQL subscriptions, inbox patterns, and channel factories.
/// </summary>
public static class ConsumerBuilderExtensions
{
    /// <summary>
    /// Adds a pre-configured PostgreSQL subscription to the consumer builder.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> instance to configure.</param>
    /// <param name="subscription">The pre-configured <see cref="PostgresSubscription"/> to add.</param>
    /// <returns>The <see cref="ConsumerBuilder"/> instance for method chaining.</returns>
    public static ConsumerBuilder AddPostgresSubscription(this ConsumerBuilder builder, PostgresSubscription subscription) 
        => builder.AddSubscription(subscription);
    
    /// <summary>
    /// Adds a PostgreSQL subscription to the consumer builder using a configuration action.
    /// This method creates a new <see cref="PostgresSubscriptionBuilder"/>, applies the configuration, and builds the subscription.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="PostgresSubscriptionBuilder"/> with subscription settings.</param>
    /// <returns>The <see cref="ConsumerBuilder"/> instance for method chaining.</returns>
    public static ConsumerBuilder AddPostgresSubscription(this ConsumerBuilder builder,
            Action<PostgresSubscriptionBuilder> configure)
    {
        var sub = new PostgresSubscriptionBuilder();
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }
    
    /// <summary>
    /// Adds a PostgreSQL subscription for a specific request type to the consumer builder.
    /// This method automatically configures the subscription with the specified <typeparamref name="TRequest"/> type
    /// and then applies additional configuration provided by the action.
    /// </summary>
    /// <typeparam name="TRequest">The type of request/message that this subscription will handle. Must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="PostgresSubscriptionBuilder"/> with additional settings.</param>
    /// <returns>The <see cref="ConsumerBuilder"/> instance for method chaining.</returns>
    public static ConsumerBuilder AddPostgresSubscription<TRequest>(this ConsumerBuilder builder,
        Action<PostgresSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        var sub = new PostgresSubscriptionBuilder();
        sub.SetDataType(typeof(TRequest));
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }
    
    /// <summary>
    /// Configures the consumer to use a PostgreSQL inbox pattern using a database configuration builder.
    /// The inbox pattern ensures message deduplication and idempotent message processing by storing
    /// information about received messages in PostgreSQL.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RelationalDatabaseConfigurationBuilder"/> with connection details.</param>
    /// <returns>The <see cref="ConsumerBuilder"/> instance for method chaining.</returns>
    public static ConsumerBuilder UsePostgresInbox(this ConsumerBuilder builder, 
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UsePostgresInbox(configuration.Build());
    }

    /// <summary>
    /// Configures the consumer to use a PostgreSQL inbox pattern using a pre-configured database configuration.
    /// The inbox pattern ensures message deduplication and idempotent message processing by storing
    /// information about received messages in PostgreSQL.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> instance to configure.</param>
    /// <param name="configuration">The pre-configured relational database configuration containing connection details.</param>
    /// <returns>The <see cref="ConsumerBuilder"/> instance for method chaining.</returns>
    public static ConsumerBuilder UsePostgresInbox(this ConsumerBuilder builder, IAmARelationalDatabaseConfiguration configuration)
        => builder.UsePostgresInbox(cfg => cfg.SetConfiguration(configuration));

    /// <summary>
    /// Configures the consumer to use a PostgreSQL inbox pattern using a custom inbox builder configuration.
    /// This method provides the most flexibility by allowing direct configuration of the <see cref="PostgresInboxBuilder"/>,
    /// including custom connection providers and other advanced settings.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="PostgresInboxBuilder"/> with inbox settings.</param>
    /// <returns>The <see cref="ConsumerBuilder"/> instance for method chaining.</returns>
    public static ConsumerBuilder UsePostgresInbox(this ConsumerBuilder builder, Action<PostgresInboxBuilder> configure)
    {
        var inbox = new PostgresInboxBuilder();
        configure(inbox);
        return builder.SetInbox(cfg => cfg.SetInbox(inbox.Build()));
    }

    /// <summary>
    /// Adds a PostgreSQL channel factory to the consumer builder using a database configuration builder.
    /// Channel factories are responsible for creating message channels that consume messages from PostgreSQL queues.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RelationalDatabaseConfigurationBuilder"/> with connection details.</param>
    /// <returns>The <see cref="ConsumerBuilder"/> instance for method chaining.</returns>
    public static ConsumerBuilder AddPostgresChannelFactory(this ConsumerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.AddPostgresChannelFactory(configuration.Build());
    }

    /// <summary>
    /// Adds a PostgreSQL channel factory to the consumer builder using a pre-configured database configuration.
    /// Channel factories are responsible for creating message channels that consume messages from PostgreSQL queues.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> instance to configure.</param>
    /// <param name="configuration">The pre-configured relational database configuration containing connection details.</param>
    /// <returns>The <see cref="ConsumerBuilder"/> instance for method chaining.</returns>
    public static ConsumerBuilder AddPostgresChannelFactory(this ConsumerBuilder builder, RelationalDatabaseConfiguration  configuration)
    {
        return builder
            .AddChannelFactory(new PostgresChannelFactory(new PostgresMessagingGatewayConnection(configuration)));
    }
}