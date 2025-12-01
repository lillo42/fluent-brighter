using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.GcpPubSub;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Configurator class for managing multiple Google Cloud Pub/Sub subscriptions.
/// Provides methods to add and configure subscriptions using fluent API patterns.
/// </summary>
/// <param name="channelFactory">The GCP Pub/Sub channel factory used to create channels for all subscriptions</param>
public class PubSubSubscriptionConfigurator(GcpPubSubChannelFactory channelFactory)
{
    /// <summary>
    /// Gets the internal list of configured GCP Pub/Sub subscriptions.
    /// </summary>
    internal List<GcpPubSubSubscription> Subscriptions { get; } = [];
    
    /// <summary>
    /// Adds a pre-configured GCP Pub/Sub subscription to the configurator.
    /// </summary>
    /// <param name="subscription">The subscription instance to add</param>
    /// <returns>The configurator instance for method chaining</returns>
    public PubSubSubscriptionConfigurator AddSubscription(GcpPubSubSubscription subscription)
    {
        Subscriptions.Add(subscription);
        return this;
    }

    /// <summary>
    /// Adds a new GCP Pub/Sub subscription by configuring it using a fluent builder.
    /// The channel factory is automatically set on the builder.
    /// </summary>
    /// <param name="configure">An action to configure the subscription using the builder</param>
    /// <returns>The configurator instance for method chaining</returns>
    /// <example>
    /// <code>
    /// configurator.AddSubscription(sub =>
    /// {
    ///     sub.SetChannelName(new ChannelName("my-subscription"))
    ///        .SetRoutingKey(new RoutingKey("my-topic"))
    ///        .SetAckDeadlineSeconds(60);
    /// });
    /// </code>
    /// </example>
    public PubSubSubscriptionConfigurator AddSubscription(Action<GcpPubSubSubscriptionBuilder> configure)
    {
        var sub = new GcpPubSubSubscriptionBuilder();
        sub.SetChannelFactory(channelFactory);
        configure(sub);
        return AddSubscription(sub.Build());
    }

    /// <summary>
    /// Adds a new GCP Pub/Sub subscription for a specific request type using a fluent builder.
    /// The data type is automatically set based on the generic parameter, and subscription name,
    /// channel name, and routing key are derived from the type name if not explicitly configured.
    /// The channel factory is automatically set on the builder.
    /// </summary>
    /// <typeparam name="TRequest">The type of request message this subscription will handle</typeparam>
    /// <param name="configure">An action to configure the subscription using the builder</param>
    /// <returns>The configurator instance for method chaining</returns>
    /// <example>
    /// <code>
    /// configurator.AddSubscription&lt;MyCommand&gt;(sub =>
    /// {
    ///     sub.SetAckDeadlineSeconds(60)
    ///        .SetNoOfPerformers(5);
    /// });
    /// </code>
    /// </example>
    public PubSubSubscriptionConfigurator AddSubscription<TRequest>(Action<GcpPubSubSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        return AddSubscription(cfg =>
        {
            cfg.SetDataType(typeof(TRequest));
            configure(cfg);
        });
    }
}