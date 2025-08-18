using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Sync;

namespace Fluent.Brighter.RMQ.Sync;

/// <summary>
/// Configures RabbitMQ subscriptions for Brighter message consumers
/// </summary>
/// <param name="channelFactory">The channel factory responsible for creating RabbitMQ consumers</param>
public class RabbitMqSubscriptionConfigurator(ChannelFactory channelFactory)
{
    internal List<RmqSubscription> Subscriptions { get; } = [];

    /// <summary>
    /// Adds a pre-configured RabbitMQ subscription
    /// </summary>
    /// <param name="subscription">Pre-built subscription configuration</param>
    /// <returns>This configurator for method chaining</returns>
    public RabbitMqSubscriptionConfigurator AddSubscription(RmqSubscription subscription)
    {
        Subscriptions.Add(subscription);
        return this;
    }

    /// <summary>
    /// Adds and configures a RabbitMQ subscription using a fluent builder
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
    public RabbitMqSubscriptionConfigurator AddSubscription(Action<RmqSubscriptionBuilder> configure)
    {
        var sub = new RmqSubscriptionBuilder();
        sub.SetChannelFactory(channelFactory);
        configure(sub);
        return AddSubscription(sub.Build());
    }
    
    /// <summary>
    /// Adds and configures a strongly-typed RabbitMQ subscription
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
    public RabbitMqSubscriptionConfigurator AddSubscription<TRequest>(Action<RmqSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        return AddSubscription(cfg =>
        {
            cfg.SetDataType(typeof(TRequest));
            configure(cfg);
        });
    }
}