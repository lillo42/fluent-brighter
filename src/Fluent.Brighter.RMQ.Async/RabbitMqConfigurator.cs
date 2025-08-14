using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Async;

namespace Fluent.Brighter.RMQ.Async;

public class RabbitMqConfigurator
{
    private RmqMessagingGatewayConnection? _connection;
    private Action<FluentBrighterBuilder> _action = _ => { };

    public RabbitMqConfigurator SetConnection(Action<RmqMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new RmqMessagingGatewayConnectionBuilder();
        configure(connection);
        _connection = connection.Build();
        return this;
    }

    public RabbitMqConfigurator SetConnection(RmqMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }
    
    public RabbitMqConfigurator UsePublications(Action<RmqMessageProducerFactoryBuilder> configure)
    {
        _action += fluent =>
        {
            fluent.Producers(producer => producer
                .AddRabbitMqPublication(cfg =>
                {
                    cfg.SetConnection(_connection!);
                    configure(cfg);
                }));
        };
        return this;
    }

    public RabbitMqConfigurator UseSubscriptions(Action<RabbitMqSubscriptionConfigurator> configure)
    {
        _action += fluent =>
        {
            fluent.Subscriptions(sub =>
            {
                var channel = new ChannelFactory(new RmqMessageConsumerFactory(_connection!));
                
                var configurator = new RabbitMqSubscriptionConfigurator(channel);
                configure(configurator);

                sub.AddChannelFactory(channel);
                
                foreach (var subscription in configurator.Subscriptions)
                {
                    sub.AddRabbitMqSubscription(subscription);
                }
            });
        };
        return this;
    }
    
    internal void SetFluentBrighter(FluentBrighterBuilder fluentBrighter)
    {
        if (_connection == null)
        {
            throw new ConfigurationException("No RmqMessagingGatewayConnection was set");
        }
        
        _action(fluentBrighter);
    }
}