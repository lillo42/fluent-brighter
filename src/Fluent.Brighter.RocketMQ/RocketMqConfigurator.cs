using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RocketMQ;

namespace Fluent.Brighter.RocketMQ;

/// <summary>
/// Central configuration class for RocketMQ integration with Brighter
/// </summary>
/// <remarks>
/// Coordinates setup of both producer (publication) and consumer (subscription) configurations
/// for RocketMQ. Requires a connection configuration before setting up publications or subscriptions.
/// </remarks>
public class RocketMqConfigurator
{
    private RocketMessagingGatewayConnection? _connection;
    private Action<FluentBrighterBuilder> _action = _ => { };

    /// <summary>
    /// Configures RocketMQ connection settings using a fluent builder
    /// </summary>
    /// <param name="configure">Action to configure connection parameters</param>
    /// <returns>This configurator for method chaining</returns>
    /// <example>
    /// SetConnection(conn => conn
    ///     .SetAmpq("amqp://localhost")
    ///     .SetExchange(ex => ex.SetName("app.events").DirectType())
    /// )
    /// </example>
    public RocketMqConfigurator SetConnection(Action<RocketMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new RocketMessagingGatewayConnectionBuilder();
        configure(connection);
        _connection = connection.Build();
        return this;
    }

    /// <summary>
    /// Sets pre-configured RocketMQ connection settings
    /// </summary>
    /// <param name="connection">Pre-built connection configuration</param>
    /// <returns>This configurator for method chaining</returns>
    public RocketMqConfigurator SetConnection(RocketMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }
    
    /// <summary>
    /// Configures message publications (producers)
    /// </summary>
    /// <param name="configure">Action to configure publications</param>
    /// <returns>This configurator for method chaining</returns>
    /// <example>
    /// UsePublications(pub => pub
    ///     .AddPublication&lt;OrderEvent&gt;(cfg => cfg
    ///         .SetTopic(new RoutingKey("order.created"))
    ///     .AddPublication&lt;PaymentEvent&gt;(cfg => cfg
    ///         .SetTopic(new RoutingKey("payment.processed")))
    /// )
    /// </example>
    public RocketMqConfigurator UsePublications(Action<RocketMessageProducerFactoryBuilder> configure)
    {
        _action += fluent =>
        {
            fluent.Producers(producer => producer
                .AddRocketMqPublication(cfg =>
                {
                    cfg.SetConnection(_connection!);
                    configure(cfg);
                }));
        };
        return this;
    }

    /// <summary>
    /// Configures message subscriptions (consumers)
    /// </summary>
    /// <param name="configure">Action to configure subscriptions</param>
    /// <returns>This configurator for method chaining</returns>
    /// <example>
    /// UseSubscriptions(subs => subs
    ///     .AddSubscription&lt;OrderEvent&gt;(cfg => cfg
    ///         .SetQueue(new ChannelName("orders"))
    ///         .SetTopic(new RoutingKey("order.*")))
    ///     .AddSubscription&lt;PaymentEvent&gt;(cfg => cfg
    ///         .SetQueue(new ChannelName("payments"))
    ///         .SetTopic(new RoutingKey("payment.*")))
    /// )
    /// </example>
    public RocketMqConfigurator UseSubscriptions(Action<RocketMqSubscriptionConfigurator> configure)
    {
        _action += fluent =>
        {
            fluent.Subscriptions(sub =>
            {
                var channel = new RocketMqChannelFactory(new RocketMessageConsumerFactory(_connection!));
                
                var configurator = new RocketMqSubscriptionConfigurator(channel);
                configure(configurator);

                sub.AddChannelFactory(channel);
                
                foreach (var subscription in configurator.Subscriptions)
                {
                    sub.AddRocketMqSubscription(subscription);
                }
            });
        };
        return this;
    }
    
    /// <summary>
    /// Applies the configuration to Brighter's fluent builder (internal)
    /// </summary>
    /// <exception cref="ConfigurationException">
    /// Thrown if connection configuration is missing
    /// </exception>
    internal void SetFluentBrighter(FluentBrighterBuilder fluentBrighter)
    {
        if (_connection == null)
        {
            throw new ConfigurationException("RocketMQ connection configuration is required. Use SetConnection() to configure connection settings.");
        }
        
        _action(fluentBrighter);
    }
}