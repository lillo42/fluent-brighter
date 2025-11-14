using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Redis;

namespace Fluent.Brighter.Redis;

/// <summary>
/// Configurator class for managing Redis message subscriptions.
/// This class provides a fluent API for adding and configuring multiple subscriptions
/// that consume messages from Redis-based messaging channels.
/// </summary>
/// <param name="channelFactory">The Redis channel factory used to create message channels for subscriptions.</param>
public sealed class RedisSubscriptionConfigurator(ChannelFactory channelFactory)
{
    /// <summary>
    /// Gets the list of configured Redis subscriptions.
    /// This property is used internally to collect all subscriptions that have been added to the configurator.
    /// </summary>
    internal List<RedisSubscription> Subscriptions { get; } = [];
    
    /// <summary>
    /// Adds a pre-configured Redis subscription to the configurator.
    /// </summary>
    /// <param name="subscription">The <see cref="RedisSubscription"/> instance to add.</param>
    /// <returns>The current <see cref="RedisSubscriptionConfigurator"/> instance for method chaining.</returns>
    public RedisSubscriptionConfigurator AddSubscription(RedisSubscription subscription)
    {
        Subscriptions.Add(subscription);
        return this;
    }
    
    /// <summary>
    /// Adds a Redis subscription using a builder configuration action.
    /// This method creates a new <see cref="RedisSubscriptionBuilder"/>, applies the provided configuration,
    /// and automatically sets the channel factory before building the subscription.
    /// </summary>
    /// <param name="configure">An action that configures the <see cref="RedisSubscriptionBuilder"/> to define subscription settings.</param>
    /// <returns>The current <see cref="RedisSubscriptionConfigurator"/> instance for method chaining.</returns>
    public RedisSubscriptionConfigurator AddSubscription(Action<RedisSubscriptionBuilder> configure)
    {
        var sub = new RedisSubscriptionBuilder();
        sub.SetChannelFactory(channelFactory);
        configure(sub);
        return AddSubscription(sub.Build());
    }
    
    /// <summary>
    /// Adds a Redis subscription for a specific request type using a builder configuration action.
    /// This method automatically configures the subscription with the specified <typeparamref name="TRequest"/> type
    /// and then applies any additional configuration provided.
    /// </summary>
    /// <typeparam name="TRequest">The type of request/message that this subscription will handle. Must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="configure">An action that configures the <see cref="RedisSubscriptionBuilder"/> with additional settings.</param>
    /// <returns>The current <see cref="RedisSubscriptionConfigurator"/> instance for method chaining.</returns>
    public RedisSubscriptionConfigurator AddSubscription<TRequest>(Action<RedisSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        return AddSubscription(cfg =>
        {
            cfg.SetDataType(typeof(TRequest));
            configure(cfg);
        });
    }
}