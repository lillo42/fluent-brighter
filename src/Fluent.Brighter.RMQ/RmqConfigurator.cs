
using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ;

namespace Fluent.Brighter.RMQ;

public class RmqConfigurator
{
    private readonly List<RmqPublication> _publications = [];
    private readonly List<Subscription> _subscriptions = [];
    private RmqMessagingGatewayConnection? _connection;

    public RmqConfigurator Connection(Action<RmqConnectionBuilder> configure)
    {
        var builder = new RmqConnectionBuilder();
        configure(builder);
        _connection = builder.Build();
        return this;
    }


    public RmqConfigurator AddSubscription(Action<RmqSubscriptionBuilder> configure)
    {
        var builder = new RmqSubscriptionBuilder();
        configure(builder);
        _subscriptions.Add(builder.Build());
        return this;
    }

    public RmqConfigurator AddSubscription<T>(Action<RmqSubscriptionBuilder> configure)
        where T : class, IRequest
    {
        var builder = new RmqSubscriptionBuilder()
            .MessageType<T>();
        configure(builder);
        _subscriptions.Add(builder.Build());
        return this;
    }

    public RmqConfigurator AddPublication(Action<RmqPublicationBuilder> configure)
    {
        var builder = new RmqPublicationBuilder();
        configure(builder);
        _publications.Add(builder.Build());
        return this;
    }

    internal IBrighterConfigurator AddRabbitMq(IBrighterConfigurator register)
    {
        if (_connection == null)
        {
            throw new InvalidOperationException("no connection setup");
        }

        _ = register
            .AddExternalBus(new RmqMessageProducerFactory(_connection, _publications))
            .AddChannelFactory(new ChannelFactory(new RmqMessageConsumerFactory(_connection)), _subscriptions);
        return register;
    }
}