using System;

using Fluent.Brighter.RocketMQ;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring RocketMQ message producers in Brighter
/// </summary>
public static class ProducersExtensions
{
    /// <summary>
    /// Configures RocketMQ message producers using a fluent builder
    /// </summary>
    /// <param name="builder">Producer registry builder</param>
    /// <param name="configure">Action to configure producer factories and publications</param>
    /// <returns>Configured producer builder</returns>
    /// <example>
    /// builder.AddRocketMqPublication(factory => factory
    ///     .SetConnection(conn => conn
    ///         .SetClient(...))
    ///     .AddPublication(pub => pub
    ///         .SetTopic(new RoutingKey("order.created"))
    ///         .CreateTopicIfMissing())
    /// )
    /// </example>
    /// <remarks>
    /// This method configures the entire RocketMQ producer infrastructure including:
    /// - Connection settings (client configuration)
    /// - Publication settings (routing, topic management)
    /// - Multiple publications can be added using AddPublication within the configuration
    /// </remarks>
    public static ProducerBuilder AddRocketMqPublication(
        this ProducerBuilder builder,
        Action<RocketMessageProducerFactoryBuilder> configure)
    {
        var factoryBuilder = new RocketMessageProducerFactoryBuilder();
        configure(factoryBuilder);
        return builder.AddMessageProducerFactory(factoryBuilder.Build());
    }
}