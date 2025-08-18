using System;

using Fluent.Brighter.RMQ.Async;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for integrating RabbitMQ with Brighter's fluent configuration
/// </summary>
public static class FluentBrighterExtensions
{
    /// <summary>
    /// Configures RabbitMQ as the message transport for Brighter
    /// </summary>
    /// <param name="builder">Brighter's fluent configuration builder</param>
    /// <param name="configure">Action to configure RabbitMQ connection, publications and subscriptions</param>
    /// <returns>Configured fluent builder</returns>
    /// <example>
    /// .UsingRabbitMq(cfg => cfg
    ///     .SetConnection(conn => conn
    ///         .SetAmpq("amqp://localhost")
    ///         .SetExchange(ex => ex.SetName("app.events").DirectType()))
    ///     .UsePublications(pub => pub
    ///         .AddPublication&lt;OrderEvent&gt;(p => p.SetTopic("order.events"))
    ///     .UseSubscriptions(sub => sub
    ///         .AddSubscription&lt;OrderEvent&gt;(s => s
    ///             .SetQueue("orders")
    ///             .SetTopic("order.*")))
    /// )
    /// </example>
    /// <remarks>
    /// This is the main entry point for RabbitMQ configuration. It enables:
    /// 1. Connection settings
    /// 2. Message publication configuration
    /// 3. Message subscription configuration
    /// 
    /// Must be called after Brighter's base configuration.
    /// </remarks>
    public static FluentBrighterBuilder UsingRabbitMq(
        this FluentBrighterBuilder builder,
        Action<RabbitMqConfigurator> configure)
    {
        var configurator = new RabbitMqConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}