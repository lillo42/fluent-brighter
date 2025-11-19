using System;

using Fluent.Brighter.GoogleCloud;

using Paramore.Brighter;
using Paramore.Brighter.Firestore;
using Paramore.Brighter.Inbox.Firestore;
using Paramore.Brighter.Inbox.Spanner;
using Paramore.Brighter.MessagingGateway.GcpPubSub;

using SpannerInboxBuilder = Fluent.Brighter.GoogleCloud.SpannerInboxBuilder;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring Google Cloud Platform services with the <see cref="ConsumerBuilder"/>.
/// </summary>
public static class ConsumerBuilderExtensions
{
    /// <summary>
    /// Adds a Google Cloud Pub/Sub subscription to the consumer builder.
    /// </summary>
    /// <param name="builder">The consumer builder instance.</param>
    /// <param name="subscription">The configured <see cref="GcpPubSubSubscription"/> to add.</param>
    /// <returns>The consumer builder for method chaining.</returns>
    public static ConsumerBuilder AddPubSubSubscription(this ConsumerBuilder builder, GcpPubSubSubscription subscription)
    {
        return builder.AddSubscription(subscription);
    }
    
    /// <summary>
    /// Adds a Google Cloud Pub/Sub subscription to the consumer builder using a configuration action.
    /// </summary>
    /// <param name="builder">The consumer builder instance.</param>
    /// <param name="configure">An action to configure the <see cref="GcpPubSubSubscriptionBuilder"/>.</param>
    /// <returns>The consumer builder for method chaining.</returns>
    public static ConsumerBuilder AddPubSubSubscription(this ConsumerBuilder builder,
        Action<GcpPubSubSubscriptionBuilder> configure)
    {
        var subscription = new GcpPubSubSubscriptionBuilder();
        configure(subscription);
        return builder.AddSubscription(subscription.Build());
    }
    
    /// <summary>
    /// Adds a Google Cloud Pub/Sub subscription to the consumer builder with a strongly-typed request using a configuration action.
    /// </summary>
    /// <typeparam name="TRequest">The type of request that this subscription handles.</typeparam>
    /// <param name="builder">The consumer builder instance.</param>
    /// <param name="configure">An action to configure the <see cref="GcpPubSubSubscriptionBuilder"/>.</param>
    /// <returns>The consumer builder for method chaining.</returns>
    public static ConsumerBuilder AddPubSubSubscription<TRequest>(this ConsumerBuilder builder, Action<GcpPubSubSubscriptionBuilder> configure) 
        where TRequest : class, IRequest
    {
        var subscription = new GcpPubSubSubscriptionBuilder();
        subscription.SetDataType(typeof(TRequest));
        configure(subscription);
        return builder.AddSubscription(subscription.Build());
    }
    
    /// <summary>
    /// Adds a Google Cloud Pub/Sub channel factory to the consumer builder.
    /// </summary>
    /// <param name="builder">The consumer builder instance.</param>
    /// <param name="configure">An action to configure the <see cref="GcpMessagingGatewayConnectionBuilder"/>.</param>
    /// <returns>The consumer builder for method chaining.</returns>
    public static ConsumerBuilder AddPubSubChannelFactory(this ConsumerBuilder builder, Action<GcpMessagingGatewayConnectionBuilder> configure)
    {
        var subscription = new GcpMessagingGatewayConnectionBuilder();
        configure(subscription);
        return builder.AddChannelFactory(new GcpPubSubChannelFactory(subscription.Build()));
    }

    /// <summary>
    /// Configures the consumer to use Google Cloud Firestore as the inbox storage.
    /// </summary>
    /// <param name="builder">The consumer builder instance.</param>
    /// <param name="configure">An action to configure the <see cref="FirestoreInboxBuilder"/>.</param>
    /// <returns>The consumer builder for method chaining.</returns>
    public static ConsumerBuilder UseFirestoreInbox(this ConsumerBuilder builder,
        Action<FirestoreInboxBuilder> configure)
    {
        var configuration = new FirestoreInboxBuilder();
        configure(configuration);
        return builder.UseFirestoreInbox(configuration.Build());
    }
    
    /// <summary>
    /// Configures the consumer to use Google Cloud Firestore as the inbox storage using an existing gateway connection.
    /// </summary>
    /// <param name="builder">The consumer builder instance.</param>
    /// <param name="connection">The <see cref="GcpMessagingGatewayConnection"/> containing credentials and project information.</param>
    /// <returns>The consumer builder for method chaining.</returns>
    public static ConsumerBuilder UseFirestoreInbox(this ConsumerBuilder builder, GcpMessagingGatewayConnection connection)
    {
        return builder.UseFirestoreInbox(cfg => cfg
            .SetConfiguration(c => c
                .SetCredential(connection.Credential)
                .SetProjectId(connection.ProjectId)
                .SetInbox(new FirestoreCollection{ Name = "inbox"})));
    }
    
    /// <summary>
    /// Configures the consumer to use Google Cloud Firestore as the inbox storage with a pre-configured inbox instance.
    /// </summary>
    /// <param name="builder">The consumer builder instance.</param>
    /// <param name="inbox">The configured <see cref="FirestoreInbox"/> instance.</param>
    /// <returns>The consumer builder for method chaining.</returns>
    public static ConsumerBuilder UseFirestoreInbox(this ConsumerBuilder builder, FirestoreInbox inbox)
    {
        return builder.SetInbox(cfg => cfg.SetInbox(inbox));
    }
    
    /// <summary>
    /// Configures the consumer to use Google Cloud Spanner as the inbox storage.
    /// </summary>
    /// <param name="builder">The consumer builder instance.</param>
    /// <param name="configure">An action to configure the <see cref="SpannerInboxBuilder"/>.</param>
    /// <returns>The consumer builder for method chaining.</returns>
    public static ConsumerBuilder UseSpannerInbox(this ConsumerBuilder builder,
        Action<SpannerInboxBuilder> configure)
    {
        var configuration = new SpannerInboxBuilder();
        configure(configuration);
        return builder.UseSpannerInbox(configuration.Build());
    }
    
    /// <summary>
    /// Configures the consumer to use Google Cloud Spanner as the inbox storage with a connection string.
    /// </summary>
    /// <param name="builder">The consumer builder instance.</param>
    /// <param name="connectionString">The Spanner connection string.</param>
    /// <returns>The consumer builder for method chaining.</returns>
    public static ConsumerBuilder UseSpannerInbox(this ConsumerBuilder builder, string connectionString)
    {
        return builder.UseSpannerInbox(cfg => cfg
            .SetConfiguration(c => c
                .SetConnectionString(connectionString)
                .SetInboxTableName("inbox")));
    }
    
    /// <summary>
    /// Configures the consumer to use Google Cloud Spanner as the inbox storage with a pre-configured inbox instance.
    /// </summary>
    /// <param name="builder">The consumer builder instance.</param>
    /// <param name="inbox">The configured <see cref="SpannerInboxAsync"/> instance.</param>
    /// <returns>The consumer builder for method chaining.</returns>
    public static ConsumerBuilder UseSpannerInbox(this ConsumerBuilder builder, SpannerInboxAsync inbox)
    {
        return builder.SetInbox(cfg => cfg.SetInbox(inbox));
    }
}