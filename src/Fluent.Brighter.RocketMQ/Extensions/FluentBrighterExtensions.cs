using System;

using Fluent.Brighter.RocketMQ;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for integrating RocketMQ with Brighter's fluent configuration
/// </summary>
public static class FluentBrighterExtensions
{
    /// <summary>
    /// Configures RocketMQ as the message transport for Brighter
    /// </summary>
    /// <param name="builder">Brighter's fluent configuration builder</param>
    /// <param name="configure">Action to configure RocketMQ connection, publications and subscriptions</param>
    /// <returns>Configured fluent builder</returns>
    /// <example>
    /// .UsingRocketMQ(cfg => cfg
    ///     .SetConnection(conn => conn
    ///         .SetClient(...))
    ///     .UsePublications(pub => pub
    ///         .AddPublication&lt;OrderEvent&gt;(p => p.SetTopic("order.events"))
    ///     .UseSubscriptions(sub => sub
    ///         .AddSubscription&lt;OrderEvent&gt;(s => s
    ///             .SetQueue("orders")
    ///             .SetTopic("order.*")))
    /// )
    /// </example>
    /// <remarks>
    /// This is the main entry point for RocketMQ configuration. It enables:
    /// 1. Connection settings
    /// 2. Message publication configuration
    /// 3. Message subscription configuration
    /// 
    /// Must be called after Brighter's base configuration.
    /// </remarks>
    public static FluentBrighterBuilder UsingRocketMq(
        this FluentBrighterBuilder builder,
        Action<RocketMqConfigurator> configure)
    {
        var configurator = new RocketMqConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}