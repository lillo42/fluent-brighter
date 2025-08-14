using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter.Kafka;

public sealed class KafkaConfigurator
{
    private KafkaMessagingGatewayConfiguration? _connection;
    private Action<FluentBrighterBuilder> _action = _ => { };

    public KafkaConfigurator SetConnection(Action<KafkaMessagingGatewayConfigurationBuilder> configure)
    {
        var connection = new KafkaMessagingGatewayConfigurationBuilder();
        configure(connection);
        _connection = connection.Build();
        return this;
    }

    public KafkaConfigurator SetConnection(KafkaMessagingGatewayConfiguration connection)
    {
        _connection = connection;
        return this;
    }
    
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
    

    internal void SetFluentBrighter(FluentBrighterBuilder fluentBrighter)
    {
        if (_connection == null)
        {
            throw new ConfigurationException("No MessagingGatewayConnection was set");
        }
        
        _action(fluentBrighter);
    }
}