using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter.Kafka;

/// <summary>
/// Provides a high-level, fluent API for configuring Kafka messaging in a Brighter application,
/// including connection settings, publications, and subscriptions.
/// </summary>
public sealed class KafkaConfigurator
{
    private KafkaMessagingGatewayConfiguration? _connection;
    private Action<FluentBrighterBuilder> _action = _ => { };

    /// <summary>
    /// Configures the Kafka connection using a builder.
    /// </summary>
    /// <param name="configure">An action that configures a <see cref="KafkaMessagingGatewayConfigurationBuilder"/>.</param>
    /// <returns>The current configurator instance for chaining.</returns>
    public KafkaConfigurator SetConnection(Action<KafkaMessagingGatewayConfigurationBuilder> configure)
    {
        var connection = new KafkaMessagingGatewayConfigurationBuilder();
        configure(connection);
        _connection = connection.Build();
        return this;
    }

    /// <summary>
    /// Sets a pre-built Kafka messaging gateway configuration.
    /// </summary>
    /// <param name="connection">The Kafka connection configuration.</param>
    /// <returns>The current configurator instance for chaining.</returns>
    public KafkaConfigurator SetConnection(KafkaMessagingGatewayConfiguration connection)
    {
        _connection = connection;
        return this;
    }
    
    /// <summary>
    /// Configures Kafka message publications (producers) for the Brighter pipeline.
    /// </summary>
    /// <param name="configure">An action that configures a <see cref="KafkaMessageProducerFactoryBuilder"/>.</param>
    /// <returns>The current configurator instance for chaining.</returns>
    public KafkaConfigurator UsePublications(Action<KafkaMessageProducerFactoryBuilder> configure)
    {
        _action += fluent =>
        {
            fluent.Producers(producer => producer
                .AddKafkaPublication(cfg =>
                {
                    cfg.SetConfiguration(_connection!);
                    configure(cfg);
                }));
        };
        return this;
    }
    
    /// <summary>
    /// Configures Kafka message subscriptions (consumers) for the Brighter pipeline.
    /// </summary>
    /// <param name="configure">An action that configures a <see cref="KafkaSubscriptionConfigurator"/>.</param>
    /// <returns>The current configurator instance for chaining.</returns>
    public KafkaConfigurator UseSubscriptions(Action<KafkaSubscriptionConfigurator> configure)
    {
        _action += fluent =>
        {
            fluent.Subscriptions(sub =>
            {
                var channel = new ChannelFactory(new KafkaMessageConsumerFactory(_connection!));
                var configurator = new KafkaSubscriptionConfigurator(channel);
                configure(configurator);

                sub.AddChannelFactory(channel);
                
                foreach (var subscription in configurator.Subscriptions)
                {
                    sub.AddKafkaSubscription(subscription);
                }
            });
        };
        return this;
    }
    
    /// <summary>
    /// Applies the configured Kafka settings to the provided Brighter builder.
    /// </summary>
    /// <param name="fluentBrighter">The Brighter application builder to configure.</param>
    /// <exception cref="ConfigurationException">Thrown if no Kafka connection has been configured.</exception>
    internal void SetFluentBrighter(FluentBrighterBuilder fluentBrighter)
    {
        if (_connection == null)
        {
            throw new ConfigurationException("No MessagingGatewayConnection was set");
        }
        
        _action(fluentBrighter);
    }
}