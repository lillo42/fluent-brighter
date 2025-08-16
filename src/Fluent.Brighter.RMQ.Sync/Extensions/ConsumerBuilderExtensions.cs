using System;

using Fluent.Brighter.RMQ.Sync;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Sync;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring RabbitMQ consumers in Brighter
/// </summary>
public static class ConsumerBuilderExtensions
{
    /// <summary>
    /// Adds a pre-configured RabbitMQ subscription
    /// </summary>
    /// <param name="builder">Consumer builder</param>
    /// <param name="subscription">Pre-built subscription configuration</param>
    /// <returns>Configured consumer builder</returns>
    public static ConsumerBuilder AddRabbitMqSubscription(
        this ConsumerBuilder builder, 
        RmqSubscription subscription)
        => builder.AddSubscription(subscription);
    
    /// <summary>
    /// Adds and configures a RabbitMQ subscription using a fluent builder
    /// </summary>
    /// <param name="builder">Consumer builder</param>
    /// <param name="configure">Action to configure subscription parameters</param>
    /// <returns>Configured consumer builder</returns>
    /// <example>
    /// builder.AddRabbitMqSubscription(sub => sub
    ///     .SetQueue(new ChannelName("orders"))
    ///     .SetTopic(new RoutingKey("order.events"))
    ///     .UseProactorMode()
    /// )
    /// </example>
    public static ConsumerBuilder AddRabbitMqSubscription(
        this ConsumerBuilder builder, 
        Action<RmqSubscriptionBuilder> configure)
    {
        var subBuilder = new RmqSubscriptionBuilder();
        configure(subBuilder);
        return builder.AddSubscription(subBuilder.Build());
    }
    
    /// <summary>
    /// Adds and configures a strongly-typed RabbitMQ subscription
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
    /// builder.AddRabbitMqSubscription&lt;OrderEvent&gt;(sub => sub
    ///     .SetSubscription(new SubscriptionName("order-service"))
    ///     .SetBufferSize(5)
    /// )
    /// </example>
    public static ConsumerBuilder AddRabbitMqSubscription<TRequest>(
        this ConsumerBuilder builder, 
        Action<RmqSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        var subBuilder = new RmqSubscriptionBuilder();
        subBuilder.SetDataType(typeof(TRequest));
        configure(subBuilder);
        return builder.AddSubscription(subBuilder.Build());
    }

    /// <summary>
    /// Configures the RabbitMQ channel factory using a fluent connection builder
    /// </summary>
    /// <param name="builder">Consumer builder</param>
    /// <param name="configure">Action to configure connection parameters</param>
    /// <returns>Configured consumer builder</returns>
    /// <example>
    /// builder.AddRabbitMqChannelFactory(conn => conn
    ///     .SetAmpq("amqp://localhost")
    ///     .SetExchange(ex => ex.SetName("app.events").DirectType())
    /// )
    /// </example>
    public static ConsumerBuilder AddRabbitMqChannelFactory(
        this ConsumerBuilder builder,
        Action<RmqMessagingGatewayConnectionBuilder> configure)
    {
        var connectionBuilder = new RmqMessagingGatewayConnectionBuilder();
        configure(connectionBuilder);
        return builder.AddRabbitMqChannelFactory(connectionBuilder.Build());
    }
    
    /// <summary>
    /// Sets the RabbitMQ channel factory using a pre-configured connection
    /// </summary>
    /// <param name="builder">Consumer builder</param>
    /// <param name="connection">Pre-built connection configuration</param>
    /// <returns>Configured consumer builder</returns>
    public static ConsumerBuilder AddRabbitMqChannelFactory(
        this ConsumerBuilder builder, 
        RmqMessagingGatewayConnection connection)
    {
        return builder.AddChannelFactory(new ChannelFactory(new RmqMessageConsumerFactory(connection)));
    }
}