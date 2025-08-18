using System;

using Fluent.Brighter.RMQ.Sync;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring RabbitMQ message producers in Brighter
/// </summary>
public static class ProducersExtensions
{
    /// <summary>
    /// Configures RabbitMQ message producers using a fluent builder
    /// </summary>
    /// <param name="builder">Producer registry builder</param>
    /// <param name="configure">Action to configure producer factories and publications</param>
    /// <returns>Configured producer builder</returns>
    /// <example>
    /// builder.AddRabbitMqPublication(factory => factory
    ///     .SetConnection(conn => conn
    ///         .SetAmpq("amqp://localhost")
    ///         .SetExchange(ex => ex.SetName("app.events").DirectType()))
    ///     .AddPublication(pub => pub
    ///         .SetTopic(new RoutingKey("order.created"))
    ///         .CreateTopicIfMissing())
    /// )
    /// </example>
    /// <remarks>
    /// This method configures the entire RabbitMQ producer infrastructure including:
    /// - Connection settings (URI, exchange configuration)
    /// - Publication settings (routing, topic management)
    /// - Multiple publications can be added using AddPublication within the configuration
    /// </remarks>
    public static ProducerBuilder AddRabbitMqPublication(
        this ProducerBuilder builder,
        Action<RmqMessageProducerFactoryBuilder> configure)
    {
        var factoryBuilder = new RmqMessageProducerFactoryBuilder();
        configure(factoryBuilder);
        return builder.AddMessageProducerFactory(factoryBuilder.Build());
    }
}