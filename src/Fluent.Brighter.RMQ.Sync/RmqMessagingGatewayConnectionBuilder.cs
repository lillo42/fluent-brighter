using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Sync;

namespace Fluent.Brighter.RMQ.Sync;
/// <summary>
/// Fluent builder for configuring RabbitMQ gateway connection settings in Brighter
/// </summary>
/// <remarks>
/// Provides a fluent interface to define core RabbitMQ connection parameters including
/// AMQP URI, exchange configurations, and connection resilience settings.
/// </remarks>
public sealed class RmqMessagingGatewayConnectionBuilder
{
    private readonly RmqMessagingGatewayConnection _connection = new();

    /// <summary>
    /// Sets a descriptive name for the connection (optional)
    /// </summary>
    /// <param name="name">Descriptive identifier for the connection</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqMessagingGatewayConnectionBuilder SetName(string name)
    {
        _connection.Name = name;
        return this;
    }
    
    /// <summary>
    /// Sets the AMQP URI specification for the RabbitMQ connection
    /// </summary>
    /// <param name="ampqUri">Complete connection specification including resilience settings</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqMessagingGatewayConnectionBuilder SetAmpq(AmqpUriSpecification ampqUri)
    {
        _connection.AmpqUri = ampqUri;
        return this;
    }
    
    /// <summary>
    /// Sets the primary exchange configuration
    /// </summary>
    /// <param name="exchange">Exchange settings for message publication</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqMessagingGatewayConnectionBuilder SetExchange(Exchange exchange)
    {
        _connection.Exchange = exchange;
        return this;
    }
    
    /// <summary>
    /// Sets the dead letter exchange for handling failed messages
    /// </summary>
    /// <param name="deadLetterExchange">DLX configuration</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqMessagingGatewayConnectionBuilder SetDeadLetterExchange(Exchange deadLetterExchange)
    {
        _connection.DeadLetterExchange = deadLetterExchange;
        return this;
    }

    /// <summary>
    /// Sets the heartbeat interval for connection monitoring
    /// </summary>
    /// <param name="heartbeat">Heartbeat interval in seconds</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqMessagingGatewayConnectionBuilder SetHeartbeat(ushort heartbeat)
    {
        _connection.Heartbeat = heartbeat;
        return this;
    }

    /// <summary>
    /// Configures message persistence (default: non-persistent)
    /// </summary>
    /// <param name="persistMessages">
    /// True to make messages survive broker restarts (persistent mode),
    /// false for transient messages
    /// </param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqMessagingGatewayConnectionBuilder SetPersistMessages(bool persistMessages)
    {
        _connection.PersistMessages = persistMessages;
        return this;
    }

    /// <summary>
    /// Sets the timeout for protocol operations
    /// </summary>
    /// <param name="continuationTimeout">Timeout in seconds for AMQP operations</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqMessagingGatewayConnectionBuilder SetContinuationTimeout(ushort continuationTimeout)
    {
        _connection.ContinuationTimeout = continuationTimeout;
        return this;
    }

    /// <summary>
    /// Constructs the final connection configuration
    /// </summary>
    /// <returns>Validated connection settings</returns>
    /// <exception cref="ConfigurationException">
    /// Thrown if required parameters (AmqpUri, Exchange) are missing
    /// </exception>
    internal RmqMessagingGatewayConnection Build()
    {
        if (_connection.AmpqUri == null)
        {
            throw new ConfigurationException("AMQP URI configuration is required. Use SetAmpq() to configure connection parameters.");
        }
        
        if (_connection.Exchange == null)
        {
            throw new ConfigurationException("Primary exchange configuration is required. Use SetExchange() to define message publication settings.");
        }

        return _connection;
    }
}