using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter.Kafka;

/// <summary>
/// Configures a collection of Kafka subscriptions for a Brighter-based application,
/// using a provided channel factory to create messaging channels.
/// Supports fluent registration of subscriptions with type-safe or dynamic configuration.
/// </summary>
public sealed class KafkaSubscriptionConfigurator(ChannelFactory channelFactory)
{
    /// <summary>
    /// Gets the list of configured Kafka subscriptions.
    /// </summary>
    internal List<KafkaSubscription> Subscriptions { get; } = [];

    /// <summary>
    /// Adds a pre-built Kafka subscription to the configuration.
    /// </summary>
    /// <param name="subscription">The subscription to add.</param>
    /// <returns>The current configurator instance for chaining.</returns>
    public KafkaSubscriptionConfigurator AddSubscription(KafkaSubscription subscription)
    {
        Subscriptions.Add(subscription);
        return this;
    }

    /// <summary>
    /// Adds a Kafka subscription by configuring it through a builder action.
    /// The provided channel factory is automatically applied to the builder.
    /// </summary>
    /// <param name="configure">An action that configures a <see cref="KafkaSubscriptionBuilder"/>.</param>
    /// <returns>The current configurator instance for chaining.</returns>
    public KafkaSubscriptionConfigurator AddSubscription(Action<KafkaSubscriptionBuilder> configure)
    {
        var sub = new KafkaSubscriptionBuilder();
        sub.SetChannelFactory(channelFactory);
        configure(sub);
        return AddSubscription(sub.Build());
    }
    
    /// <summary>
    /// Adds a Kafka subscription for a specific request type, enabling type-safe message handling.
    /// The request type is automatically registered, and additional configuration can be applied.
    /// </summary>
    /// <typeparam name="TRequest">The request type, which must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="configure">An action to further configure the subscription builder.</param>
    /// <returns>The current configurator instance for chaining.</returns>
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