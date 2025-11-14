using System;

using Fluent.Brighter.Redis.Extensions;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Redis;

namespace Fluent.Brighter.Redis;

/// <summary>
/// Provides a fluent API for configuring Redis integration with Fluent Brighter.
/// This configurator allows you to set up Redis-based messaging, including message subscriptions and publications.
/// </summary>
public sealed class RedisConfigurator
{
    private RedisMessagingGatewayConfiguration? _connection;
    private Action<FluentBrighterBuilder> _action = _ => { };

    /// <summary>
    /// Sets the Redis messaging gateway connection using a fluent configuration builder.
    /// </summary>
    /// <param name="configure">An action that configures the <see cref="RedisMessagingGatewayConfigurationBuilder"/> with connection details and settings.</param>
    /// <returns>The current <see cref="RedisConfigurator"/> instance for method chaining.</returns>
    public RedisConfigurator SetConnection(Action<RedisMessagingGatewayConfigurationBuilder> configure)
    {
        var connection = new RedisMessagingGatewayConfigurationBuilder();
        configure(connection);
        _connection = connection.Build();
        return this;
    }

    /// <summary>
    /// Sets the Redis messaging gateway connection using a pre-configured <see cref="RedisMessagingGatewayConfiguration"/> object.
    /// </summary>
    /// <param name="connection">The Redis messaging gateway configuration containing connection details.</param>
    /// <returns>The current <see cref="RedisConfigurator"/> instance for method chaining.</returns>
    public RedisConfigurator SetConnection(RedisMessagingGatewayConfiguration connection)
    {
        _connection = connection;
        return this;
    }
    
    /// <summary>
    /// Configures Redis-based message publications.
    /// This method allows you to define how messages are published through Redis messaging gateway,
    /// including topic mappings and producer settings.
    /// </summary>
    /// <param name="configure">An action that configures the <see cref="RedisMessageProducerFactoryBuilder"/> for publications.</param>
    /// <returns>The current <see cref="RedisConfigurator"/> instance for method chaining.</returns>
    public RedisConfigurator UsePublications(Action<RedisMessageProducerFactoryBuilder> configure)
    {
        _action += fluent =>
        {
            fluent.Producers(producer => producer
                .AddRedisPublication(cfg =>
                {
                    cfg.SetConfiguration(_connection!);
                    configure(cfg);
                }));
        };
        return this;
    }
    
    /// <summary>
    /// Configures Redis-based message subscriptions.
    /// This method allows you to define how messages are consumed from Redis messaging gateway,
    /// including channel configurations and subscription mappings for different message types.
    /// </summary>
    /// <param name="configure">An action that configures the <see cref="RedisSubscriptionConfigurator"/> for subscriptions.</param>
    /// <returns>The current <see cref="RedisConfigurator"/> instance for method chaining.</returns>
    public RedisConfigurator UseSubscriptions(Action<RedisSubscriptionConfigurator> configure)
    {
        _action += fluent =>
        {
            fluent.Subscriptions(sub =>
            {
                var channel = new ChannelFactory(new RedisMessageConsumerFactory(_connection!));
                var configurator = new RedisSubscriptionConfigurator(channel);
                configure(configurator);

                sub.AddChannelFactory(channel);
                
                foreach (var subscription in configurator.Subscriptions)
                {
                    sub.AddRedisSubscription(subscription);
                }
            });
        };
        return this;
    }
    
    /// <summary>
    /// Applies the configured Redis settings to the Fluent Brighter builder.
    /// This method is called internally to set up all the configured Redis features.
    /// </summary>
    /// <param name="fluentBrighter">The Fluent Brighter builder instance to configure.</param>
    /// <exception cref="ConfigurationException">Thrown when no Redis connection has been configured via <see cref="SetConnection(Action{RedisMessagingGatewayConfigurationBuilder})"/> or <see cref="SetConnection(RedisMessagingGatewayConfiguration)"/>.</exception>
    internal void SetFluentBrighter(FluentBrighterBuilder fluentBrighter)
    {
        if (_connection == null)
        {
            throw new ConfigurationException("No MessagingGatewayConnection was set");
        }
        
        _action(fluentBrighter);
    }
}