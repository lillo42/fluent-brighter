using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter.Postgres;

/// <summary>
/// Builder class for configuring and creating a PostgreSQL subscription.
/// A subscription defines how messages are consumed from PostgreSQL, including channel settings,
/// message processing options, retry behavior, and PostgreSQL-specific configurations.
/// </summary>
public sealed class PostgresSubscriptionBuilder
{
    private SubscriptionName? _subscriptionName;

    /// <summary>
    /// Sets the subscription name that uniquely identifies this subscription.
    /// The subscription name is used to track and manage the subscription within the Brighter framework.
    /// </summary>
    /// <param name="subscriptionName">The unique name for this subscription.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetSubscription(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }
    
    private ChannelName? _channelName;
    private RoutingKey? _routingKey;
    
    /// <summary>
    /// Sets the channel name and routing key for the subscription.
    /// The channel represents the PostgreSQL queue/topic from which messages will be consumed.
    /// This method automatically sets the routing key to match the channel name.
    /// </summary>
    /// <param name="channelName">The name of the channel to subscribe to.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetQueue(ChannelName channelName)
    {
        _channelName = channelName;
        _routingKey = new RoutingKey(channelName.Value);
        return this;
    }
    
    private Type? _dataType;

    /// <summary>
    /// Sets the data type for messages in this subscription.
    /// When provided, this method will automatically configure the subscription name, channel name,
    /// and routing key based on the type name if they haven't been explicitly set.
    /// </summary>
    /// <param name="dataType">The .NET type of the messages that will be consumed, or null if not specified.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetDataType(Type? dataType)
    {
        _dataType = dataType;
        if (dataType == null)
        {
            return this;
        }
        
        if (_subscriptionName == null)
        {
            _subscriptionName = new SubscriptionName(dataType.ToString());
        }

        if (ChannelName.IsNullOrEmpty(_channelName))
        {
            _channelName = new ChannelName(dataType.Name);
        }

        if (RoutingKey.IsNullOrEmpty(_routingKey))
        {
            _routingKey = new RoutingKey(dataType.Name);
        }
        
        return this;
    }
    
    private Func<Message, Type>? _getRequestType;

    /// <summary>
    /// Sets a function that determines the request type dynamically based on the message content.
    /// This allows for polymorphic message handling where different message types can be processed by different handlers.
    /// </summary>
    /// <param name="getRequestType">A function that takes a <see cref="Message"/> and returns the corresponding .NET type, or null to use the default behavior.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetGetRequestType(Func<Message, Type>? getRequestType)
    {
        _getRequestType = getRequestType;
        return this;
    }

    private int _bufferSize = 1;

    /// <summary>
    /// Sets the buffer size for the message channel.
    /// The buffer size determines how many messages can be queued in memory before processing,
    /// allowing for better throughput in high-volume scenarios.
    /// </summary>
    /// <param name="bufferSize">The buffer size (number of messages). Default is 1.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetBufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }
    
    private int _noOfPerformers  = 1;

    /// <summary>
    /// Sets the number of performer threads that will process messages concurrently.
    /// Multiple performers allow parallel message processing, improving throughput for I/O-bound operations.
    /// </summary>
    /// <param name="noOfPerformers">The number of concurrent performers. Default is 1.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetNumberOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }
    
    private TimeSpan? _timeOut;
    
    /// <summary>
    /// Sets the timeout duration for message processing operations.
    /// If a message handler takes longer than this timeout, the operation will be cancelled.
    /// </summary>
    /// <param name="timeout">The timeout duration, or null to use the default timeout.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetTimeout(TimeSpan? timeout)
    {
        _timeOut = timeout;
        return this;
    }
    
    private int _requeueCount = -1;
    
    /// <summary>
    /// Sets the maximum number of times a failed message will be requeued for retry.
    /// Messages that fail processing can be retried up to this limit before being moved to a dead letter queue or discarded.
    /// </summary>
    /// <param name="requeueCount">The maximum requeue count. Use -1 for unlimited retries. Default is -1.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetRequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }
    
    private TimeSpan? _requeueDelay;
    
    /// <summary>
    /// Sets the delay before a failed message is requeued for retry.
    /// This provides a backoff period before attempting to process the message again, which can help with transient failures.
    /// </summary>
    /// <param name="timeout">The requeue delay duration, or null to use the default delay.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetRequeueDelay(TimeSpan? timeout)
    {
        _requeueDelay = timeout;
        return this;
    }
    
    private int _unacceptableMessageLimit;
    
    /// <summary>
    /// Sets the limit for the number of unacceptable messages that can be received before the channel stops processing.
    /// Unacceptable messages are those that cannot be deserialized or are malformed. This limit prevents endless processing of bad messages.
    /// </summary>
    /// <param name="unacceptableMessageLimit">The maximum number of unacceptable messages before stopping. Use 0 for unlimited.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetUnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit ;
        return this;
    }
    
    private MessagePumpType _messagePumpType = MessagePumpType.Proactor;
    
    /// <summary>
    /// Sets the message pump type that determines how messages are processed.
    /// Proactor uses async/await patterns for non-blocking I/O, while Reactor uses synchronous processing.
    /// </summary>
    /// <param name="messagePumpType">The <see cref="MessagePumpType"/> to use. Default is <see cref="MessagePumpType.Proactor"/>.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetMessagePumpType(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }

    private IAmAChannelFactory? _channelFactory;

    /// <summary>
    /// Sets a custom channel factory for creating message channels.
    /// Use this when you need to provide custom channel creation logic beyond the default PostgreSQL channel factory.
    /// </summary>
    /// <param name="channelFactory">The custom channel factory implementation, or null to use the default.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }
    
    private OnMissingChannel _onMissingChannel = OnMissingChannel.Create;

    /// <summary>
    /// Sets the behavior for handling missing channels/topics in PostgreSQL.
    /// Determines whether to create, validate, or assume the existence of channels when they are not found.
    /// </summary>
    /// <param name="onMissingChannel">The <see cref="OnMissingChannel"/> behavior to apply. Default is <see cref="OnMissingChannel.Create"/>.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetMakeChannels(OnMissingChannel onMissingChannel)
    {
        _onMissingChannel = onMissingChannel;
        return this;
    }

    private TimeSpan? _emptyChannelDelay;

    /// <summary>
    /// Sets the delay before checking for messages again when the channel is empty.
    /// This prevents tight polling loops and reduces database load when no messages are available.
    /// </summary>
    /// <param name="emptyChannelDelay">The delay duration when the channel is empty, or null to use the default delay.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetEmptyChannelDelay(TimeSpan? emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }

    private TimeSpan? _channelFailureDelay;

    /// <summary>
    /// Sets the delay before retrying channel operations after a failure.
    /// This provides a backoff period when channel errors occur, preventing rapid retry loops that could overwhelm the system.
    /// </summary>
    /// <param name="channelFailureDelay">The delay duration after a channel failure, or null to use the default delay.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetChannelFailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }

    private string? _schemaName;

    /// <summary>
    /// Sets the PostgreSQL database schema name where the message queue tables are located.
    /// This allows you to organize queue tables in a specific database schema.
    /// </summary>
    /// <param name="schemaName">The PostgreSQL schema name, or null to use the default schema.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetSchemaName(string? schemaName)
    {
        _schemaName = schemaName;
        return this;
    }

    private string? _queueStoreTable;

    /// <summary>
    /// Sets the PostgreSQL table name to use for storing messages in the queue.
    /// This allows you to customize the table name where messages are persisted.
    /// </summary>
    /// <param name="queueStoreTable">The PostgreSQL table name for the queue store, or null to use the default table name.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetQueueStoreTable(string? queueStoreTable)
    {
        _queueStoreTable = queueStoreTable;
        return this;
    }

    private TimeSpan? _visibleTimeout;

    /// <summary>
    /// Sets the visibility timeout for messages in the queue.
    /// Messages that are being processed become invisible to other consumers for this duration.
    /// If processing doesn't complete within this timeout, the message becomes visible again for reprocessing.
    /// </summary>
    /// <param name="visibleTimeout">The visibility timeout duration, or null to use the default timeout.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetVisibleTimeout(TimeSpan? visibleTimeout)
    {
        _visibleTimeout = visibleTimeout;
        return this;
    }

    private bool _tableWithLargeMessage;

    /// <summary>
    /// Sets whether the queue table should be configured to handle large messages.
    /// When enabled, the table schema is optimized for storing larger message payloads that exceed normal size limits.
    /// </summary>
    /// <param name="tableWithLargeMessage">True to enable large message support, false otherwise.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetTableWithLargeMessage(bool tableWithLargeMessage)
    {
        _tableWithLargeMessage = tableWithLargeMessage;
        return this;
    }

    private bool? _binaryMessagePayload;

    /// <summary>
    /// Sets whether to store message payloads in binary format in PostgreSQL.
    /// When enabled, message payloads are stored as binary data (bytea) instead of text, which can be more efficient for certain message types.
    /// </summary>
    /// <param name="binaryMessagePayload">True to use binary format, false for text format, or null to use the default setting.</param>
    /// <returns>The current <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public PostgresSubscriptionBuilder SetBinaryMessagePayload(bool? binaryMessagePayload)
    {
        _binaryMessagePayload = binaryMessagePayload;
        return this;
    }
    
    /// <summary>
    /// Builds and returns a configured <see cref="PostgresSubscription"/> instance.
    /// This method is called internally to create the subscription with all the configured settings
    /// including channel configuration, message processing options, and PostgreSQL-specific settings.
    /// </summary>
    /// <returns>A configured <see cref="PostgresSubscription"/> instance.</returns>
    /// <exception cref="ConfigurationException">Thrown when required configuration is missing: subscription name, channel name, or routing key.</exception>
    internal PostgresSubscription Build()
    {
        if (_subscriptionName == null)
        {
            throw new ConfigurationException("SubscriptionName not set");
        }
        
        if (_channelName == null)
        {
            throw new ConfigurationException("ChannelName not set");
        }

        if (_routingKey == null)
        {
            throw new ConfigurationException("RoutingKey not set");
        }
        
        return new PostgresSubscription(
            subscriptionName: _subscriptionName,
            channelName: _channelName,
            routingKey: _routingKey,
            dataType: _dataType,
            getRequestType: _getRequestType,
            bufferSize: _bufferSize,
            noOfPerformers: _noOfPerformers,
            timeOut: _timeOut,
            requeueCount: _requeueCount,
            requeueDelay: _requeueDelay,
            unacceptableMessageLimit: _unacceptableMessageLimit,
            messagePumpType: _messagePumpType,
            channelFactory: _channelFactory,
            makeChannels: _onMissingChannel,
            emptyChannelDelay: _emptyChannelDelay, 
            channelFailureDelay: _channelFailureDelay,
            schemaName: _schemaName,
            queueStoreTable: _queueStoreTable,
            visibleTimeout: _visibleTimeout,
            tableWithLargeMessage: _tableWithLargeMessage,
            binaryMessagePayload: _binaryMessagePayload);
    }
}