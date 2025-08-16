using System;

using Fluent.Brighter.RMQ.Sync;

namespace Fluent.Brighter;

/// <summary>
/// Provides fluent extension methods for configuring RabbitMQ gateway connections
/// </summary>
public static class RmqMessagingGatewayConnectionBuilderExtensions
{
    /// <summary>
    /// Configures the AMQP URI using a connection string
    /// </summary>
    /// <param name="builder">Connection builder</param>
    /// <param name="uri">RabbitMQ connection string (amqp://user:pass@host:port/vhost)</param>
    /// <returns>Configured connection builder</returns>
    /// <example>
    /// builder.SetAmpq("amqp://guest:guest@localhost:5672/")
    /// </example>
    public static RmqMessagingGatewayConnectionBuilder SetAmpq(
        this RmqMessagingGatewayConnectionBuilder builder, 
        string uri)
    {
        return builder.SetAmpq(cfg => cfg.SetUri(uri));
    }
    
    /// <summary>
    /// Configures the AMQP URI using a Uri object
    /// </summary>
    /// <param name="builder">Connection builder</param>
    /// <param name="uri">RabbitMQ connection URI</param>
    /// <returns>Configured connection builder</returns>
    public static RmqMessagingGatewayConnectionBuilder SetAmpq(
        this RmqMessagingGatewayConnectionBuilder builder, 
        Uri uri)
    {
        return builder.SetAmpq(cfg => cfg.SetUri(uri));
    }
    
    /// <summary>
    /// Configures advanced AMQP URI settings using a fluent builder
    /// </summary>
    /// <param name="builder">Connection builder</param>
    /// <param name="configure">Action to configure URI specification</param>
    /// <returns>Configured connection builder</returns>
    /// <example>
    /// builder.SetAmpq(cfg => cfg
    ///     .SetUri("amqp://localhost")
    ///     .SetConnectionRetryCount(3)
    ///     .SetCircuitBreakTimeInMilliseconds(5000)
    /// )
    /// </example>
    public static RmqMessagingGatewayConnectionBuilder SetAmpq(
        this RmqMessagingGatewayConnectionBuilder builder,
        Action<AmqpUriSpecificationBuilder> configure)
    {
        var specification = new AmqpUriSpecificationBuilder();
        configure(specification);
        return builder.SetAmpq(specification.Build());
    }

    /// <summary>
    /// Configures the primary exchange using a fluent builder
    /// </summary>
    /// <param name="builder">Connection builder</param>
    /// <param name="configure">Action to configure exchange settings</param>
    /// <returns>Configured connection builder</returns>
    /// <example>
    /// builder.SetExchange(ex => ex
    ///     .SetName("orders")
    ///     .SetType(ExchangeType.Direct)
    ///     .SetDurable(true)
    /// )
    /// </example>
    public static RmqMessagingGatewayConnectionBuilder SetExchange(
        this RmqMessagingGatewayConnectionBuilder builder,
        Action<ExchangeBuilder> configure)
    {
        var exchangeBuilder = new ExchangeBuilder();
        configure(exchangeBuilder);
        return builder.SetExchange(exchangeBuilder.Build());
    }
    
    /// <summary>
    /// Configures the dead letter exchange using a fluent builder
    /// </summary>
    /// <param name="builder">Connection builder</param>
    /// <param name="configure">Action to configure DLX settings</param>
    /// <returns>Configured connection builder</returns>
    /// <example>
    /// builder.SetDeadLetterExchange(dlx => dlx
    ///     .SetName("orders.dead-letters")
    ///     .SetType(ExchangeType.Fanout)
    ///     .SetDurable(true)
    /// )
    /// </example>
    public static RmqMessagingGatewayConnectionBuilder SetDeadLetterExchange(
        this RmqMessagingGatewayConnectionBuilder builder,
        Action<ExchangeBuilder> configure)
    {
        var exchangeBuilder = new ExchangeBuilder();
        configure(exchangeBuilder);
        return builder.SetDeadLetterExchange(exchangeBuilder.Build());
    }
}