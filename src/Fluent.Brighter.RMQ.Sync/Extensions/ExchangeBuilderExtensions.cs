using Fluent.Brighter.RMQ.Sync;

using RabbitMQ.Client;

namespace Fluent.Brighter;

/// <summary>
/// Provides fluent extension methods for configuring RabbitMQ exchanges
/// </summary>
public static class ExchangeBuilderExtensions
{
    #region Durable 
    /// <summary>
    /// Configures the exchange as durable (survives broker restarts)
    /// </summary>
    /// <remarks>
    /// Durable exchanges persist to disk and survive broker restarts.
    /// Recommended for persistent messaging scenarios.
    /// </remarks>
    /// <param name="builder">Exchange builder</param>
    /// <returns>Configured exchange builder</returns>
    public static ExchangeBuilder EnableDurable(this ExchangeBuilder builder) 
        => builder.SetDurable(true);
    
    /// <summary>
    /// Configures the exchange as transient (deleted on broker restart)
    /// </summary>
    /// <remarks>
    /// Transient exchanges are deleted when the broker restarts.
    /// Suitable for temporary or ephemeral messaging patterns.
    /// </remarks>
    /// <param name="builder">Exchange builder</param>
    /// <returns>Configured exchange builder</returns>
    public static ExchangeBuilder DisableDurable(this ExchangeBuilder builder)
        => builder.SetDurable(false);
    #endregion

    #region Support delay
    /// <summary>
    /// Enables delayed message support for the exchange
    /// </summary>
    /// <remarks>
    /// Requires RabbitMQ delayed message exchange plugin.
    /// Allows messages to be scheduled for future delivery.
    /// </remarks>
    /// <param name="builder">Exchange builder</param>
    /// <returns>Configured exchange builder</returns>
    public static ExchangeBuilder EnableSupportDelay(this ExchangeBuilder builder) 
        => builder.SetSupportDelay(true);
    
    /// <summary>
    /// Disables delayed message support
    /// </summary>
    /// <remarks>
    /// Default behavior. Messages are delivered immediately.
    /// </remarks>
    /// <param name="builder">Exchange builder</param>
    /// <returns>Configured exchange builder</returns>
    public static ExchangeBuilder DisableSupportDelay(this ExchangeBuilder builder) 
        => builder.SetSupportDelay(false);
    #endregion

    #region Exchange type
    /// <summary>
    /// Configures the exchange as Direct type
    /// </summary>
    /// <remarks>
    /// Direct exchanges route messages to queues based on exact routing key matches.
    /// Suitable for point-to-point messaging.
    /// </remarks>
    /// <param name="builder">Exchange builder</param>
    /// <returns>Configured exchange builder</returns>
    public static ExchangeBuilder DirectType(this ExchangeBuilder builder)
        => builder.SetType(ExchangeType.Direct);
    
    /// <summary>
    /// Configures the exchange as Fanout type
    /// </summary>
    /// <remarks>
    /// Fanout exchanges broadcast messages to all bound queues.
    /// Ideal for publish-subscribe patterns.
    /// </remarks>
    /// <param name="builder">Exchange builder</param>
    /// <returns>Configured exchange builder</returns>
    public static ExchangeBuilder FanoutType(this ExchangeBuilder builder)
        => builder.SetType(ExchangeType.Fanout);
    
    /// <summary>
    /// Configures the exchange as Headers type
    /// </summary>
    /// <remarks>
    /// Headers exchanges route based on message header values.
    /// More flexible but less performant than other types.
    /// </remarks>
    /// <param name="builder">Exchange builder</param>
    /// <returns>Configured exchange builder</returns>
    public static ExchangeBuilder HeadersType(this ExchangeBuilder builder)
        => builder.SetType(ExchangeType.Headers);
    
    /// <summary>
    /// Configures the exchange as Topic type
    /// </summary>
    /// <remarks>
    /// Topic exchanges route using wildcard pattern matching on routing keys.
    /// Supports complex routing scenarios with multiple subscribers.
    /// </remarks>
    /// <param name="builder">Exchange builder</param>
    /// <returns>Configured exchange builder</returns>
    public static ExchangeBuilder TopicType(this ExchangeBuilder builder)
        => builder.SetType(ExchangeType.Topic);
    #endregion
}