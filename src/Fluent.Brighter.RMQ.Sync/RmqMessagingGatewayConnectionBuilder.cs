using Paramore.Brighter.MessagingGateway.RMQ.Sync;

namespace Fluent.Brighter.RMQ.Sync;

public sealed class RmqMessagingGatewayConnectionBuilder
{
    private readonly RmqMessagingGatewayConnection _connection = new();

    public RmqMessagingGatewayConnectionBuilder SetName(string name)
    {
        _connection.Name = name;
        return this;
    }
    
    public RmqMessagingGatewayConnectionBuilder SetAmpq(AmqpUriSpecification ampqUri)
    {
        _connection.AmpqUri = ampqUri;
        return this;
    }
    

    public RmqMessagingGatewayConnectionBuilder SetExchange(Exchange exchange)
    {
        _connection.Exchange = exchange;
        return this;
    }
    
    public RmqMessagingGatewayConnectionBuilder SetDeadLetterExchange(Exchange deadLetterExchange)
    {
        _connection.DeadLetterExchange = deadLetterExchange;
        return this;
    }

    public RmqMessagingGatewayConnectionBuilder SetHeartbeat(ushort heartbeat)
    {
        _connection.Heartbeat = heartbeat;
        return this;
    }

    public RmqMessagingGatewayConnectionBuilder SetPersistMessages(bool persistMessages)
    {
        _connection.PersistMessages = persistMessages;
        return this;
    }

    public RmqMessagingGatewayConnectionBuilder SetContinuationTimeout(ushort continuationTimeout)
    {
        _connection.ContinuationTimeout = continuationTimeout;
        return this;
    }

    internal RmqMessagingGatewayConnection Build()
    {
        return _connection;
    }
}