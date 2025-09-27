using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RocketMQ;

namespace Fluent.Brighter.RocketMQ;

/// <summary>
/// Configures RocketMQ subscriptions for Brighter message consumers
/// </summary>
/// <param name="channelFactory">The channel factory responsible for creating RocketMQ consumers</param>
public class RocketMqSubscriptionConfigurator(RocketMqChannelFactory channelFactory)
{
    internal List<RocketSubscription> Subscriptions { get; } = [];

    /// <summary>
    /// Adds a pre-configured RocketMQ subscription
    /// </summary>
    /// <param name="subscription">Pre-built subscription configuration</param>
    /// <returns>This configurator for method chaining</returns>
    public RocketMqSubscriptionConfigurator AddSubscription(RocketSubscription subscription)
    {
        Subscriptions.Add(subscription);
        return this;
    }

    /// <summary>
    /// Adds and configures a RocketMQ subscription using a fluent builder
    /// </summary>
    /// <param name="configure">Action to configure subscription parameters</param>
    /// <returns>This configurator for method chaining</returns>
    /// <remarks>
    /// Automatically associates the subscription with the channel factory.
    /// </remarks>
    /// <example>
    /// AddSubscription(sub => sub
    ///     .SetSubscription(new SubscriptionName("orders"))
    ///     .SetQueue(new ChannelName("orders.queue"))
    ///     .SetTopic(new RoutingKey("order.events"))
    ///     .UseProactorMode()
    /// )
    /// </example>
    public RocketMqSubscriptionConfigurator AddSubscription(Action<RocketSubscriptionBuilder> configure)
    {
        var sub = new RocketSubscriptionBuilder();
        sub.SetChannelFactory(channelFactory);
        configure(sub);
        return AddSubscription(sub.Build());
    }
    
    /// <summary>
    /// Adds and configures a strongly-typed RocketMQ subscription
    /// </summary>
    /// <typeparam name="TRequest">The message type implementing IRequest</typeparam>
    /// <param name="configure">Action to configure subscription parameters</param>
    /// <returns>This configurator for method chaining</returns>
    /// <remarks>
    /// Automatically sets the message type before applying custom configurations.
    /// Recommended for typed message handlers.
    /// </remarks>
    /// <example>
    /// AddSubscription&lt;OrderEvent&gt;(sub => sub
    ///     .SetBufferSize(5)
    ///     .SetNumberOfPerformers(3)
    /// )
    /// </example>
    public RocketMqSubscriptionConfigurator AddSubscription<TRequest>(Action<RocketSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        return AddSubscription(cfg =>
        {
            cfg.SetRequestType(typeof(TRequest));
            configure(cfg);
        });
    }
}