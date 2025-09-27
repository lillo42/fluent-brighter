using System;

using Fluent.Brighter.RocketMQ;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RocketMQ;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring RocketMQ consumers in Brighter
/// </summary>
public static class ConsumerBuilderExtensions
{
    /// <summary>
    /// Adds a pre-configured RocketMQ subscription
    /// </summary>
    /// <param name="builder">Consumer builder</param>
    /// <param name="subscription">Pre-built subscription configuration</param>
    /// <returns>Configured consumer builder</returns>
    public static ConsumerBuilder AddRocketMqSubscription(
        this ConsumerBuilder builder, 
        RocketSubscription subscription)
        => builder.AddSubscription(subscription);
    
    /// <summary>
    /// Adds and configures a RocketMQ subscription using a fluent builder
    /// </summary>
    /// <param name="builder">Consumer builder</param>
    /// <param name="configure">Action to configure subscription parameters</param>
    /// <returns>Configured consumer builder</returns>
    /// <example>
    /// builder.AddRocketMqSubscription(sub => sub
    ///     .SetQueue(new ChannelName("orders"))
    ///     .SetTopic(new RoutingKey("order.events"))
    ///     .UseProactorMode()
    /// )
    /// </example>
    public static ConsumerBuilder AddRocketMqSubscription(
        this ConsumerBuilder builder, 
        Action<RocketSubscriptionBuilder> configure)
    {
        var subBuilder = new RocketSubscriptionBuilder();
        configure(subBuilder);
        return builder.AddSubscription(subBuilder.Build());
    }
    
    /// <summary>
    /// Adds and configures a strongly-typed RocketMQ subscription
    /// </summary>
    /// <typeparam name="TRequest">The message type implementing IRequest</typeparam>
    /// <param name="builder">Consumer builder</param>
    /// <param name="configure">Action to configure subscription parameters</param>
    /// <returns>Configured consumer builder</returns>
    /// <remarks>
    /// Automatically sets the message type before applying custom configurations.
    /// Recommended for typed message handlers.
    /// </remarks>
    /// <example>
    /// builder.AddRocketMqSubscription&lt;OrderEvent&gt;(sub => sub
    ///     .SetSubscription(new SubscriptionName("order-service"))
    ///     .SetBufferSize(5)
    /// )
    /// </example>
    public static ConsumerBuilder AddRocketMqSubscription<TRequest>(
        this ConsumerBuilder builder, 
        Action<RocketSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        var subBuilder = new RocketSubscriptionBuilder();
        subBuilder.SetRequestType(typeof(TRequest));
        configure(subBuilder);
        return builder.AddSubscription(subBuilder.Build());
    }

    /// <summary>
    /// Configures the RocketMQ channel factory using a fluent connection builder
    /// </summary>
    /// <param name="builder">Consumer builder</param>
    /// <param name="configure">Action to configure connection parameters</param>
    /// <returns>Configured consumer builder</returns>
    /// <example>
    /// builder.AddRocketMqChannelFactory(conn => conn
    ///     .SetClient(...)
    /// )
    /// </example>
    public static ConsumerBuilder AddRocketMqChannelFactory(
        this ConsumerBuilder builder,
        Action<RocketMessagingGatewayConnectionBuilder> configure)
    {
        var connectionBuilder = new RocketMessagingGatewayConnectionBuilder();
        configure(connectionBuilder);
        return builder.AddRocketMqChannelFactory(connectionBuilder.Build());
    }
    
    /// <summary>
    /// Sets the RocketMQ channel factory using a pre-configured connection
    /// </summary>
    /// <param name="builder">Consumer builder</param>
    /// <param name="connection">Pre-built connection configuration</param>
    /// <returns>Configured consumer builder</returns>
    public static ConsumerBuilder AddRocketMqChannelFactory(
        this ConsumerBuilder builder, 
        RocketMessagingGatewayConnection connection)
    {
        return builder.AddChannelFactory(new RocketMqChannelFactory(new RocketMessageConsumerFactory(connection)));
    }
}