using System;

using Paramore.Brighter.MessagingGateway.RMQ;

namespace Fluent.Brighter.RMQ;

public class RmqConnectionBuilder
{
    private string _name = Environment.MachineName;
    public RmqConnectionBuilder Name(string name)
    {
        _name = name;
        return this;
    }

    private AmqpUriSpecification? _amqpUri;
    public RmqConnectionBuilder AmqpUriSpecification(Action<AmqpUriSpecificationBuilder> configure)
    {
        var builder = new AmqpUriSpecificationBuilder();
        configure(builder);
        return AmqpUriSpecification(builder.Build());
    }

    public RmqConnectionBuilder AmqpUriSpecification(AmqpUriSpecification specification)
    {
        _amqpUri = specification;
        return this;
    }


    private Exchange? _exchange;
    public RmqConnectionBuilder Exchange(Action<ExchangeBuilder> configure)
    {
        var builder = new ExchangeBuilder();
        configure(builder);
        return Exchange(builder.Build());
    }

    public RmqConnectionBuilder Exchange(Exchange exchange)
    {
        _exchange = exchange;
        return this;
    }

    private Exchange? _deadLetterExchange;
    public RmqConnectionBuilder DeadLetterExchange(Action<ExchangeBuilder> configure)
    {
        var builder = new ExchangeBuilder();
        configure(builder);
        return DeadLetterExchange(builder.Build());
    }

    public RmqConnectionBuilder DeadLetterExchange(Exchange exchange)
    {
        _deadLetterExchange = exchange;
        return this;
    }

    private ushort _heartbeat = 20;
    public RmqConnectionBuilder HeartBeat(ushort heartbeat)
    {
        _heartbeat = heartbeat;
        return this;
    }

    private ushort _continuationTimeout = 20;
    public RmqConnectionBuilder ContinuationTimeout(ushort continuationTimeout)
    {
        _continuationTimeout = continuationTimeout;
        return this;
    }

    private bool _persistMessages;

    public RmqConnectionBuilder EnablePersistMessage()
    {
        return PersistMessage(true);
    }

    public RmqConnectionBuilder DisablePersistMessage()
    {
        return PersistMessage(false);
    }

    public RmqConnectionBuilder PersistMessage(bool persistMessage)
    {
        _persistMessages = persistMessage;
        return this;
    }

    internal RmqMessagingGatewayConnection Build()
    {
        return new RmqMessagingGatewayConnection
        {
            Name = _name,
            AmpqUri = _amqpUri,
            Exchange = _exchange,
            DeadLetterExchange = _deadLetterExchange,
            Heartbeat = _heartbeat,
            PersistMessages = _persistMessages,
            ContinuationTimeout = _continuationTimeout
        };
    }
}