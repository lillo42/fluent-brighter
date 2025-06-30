using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter.Postgres;

/// <summary>
/// A fluent builder for creating instances of <see cref="PostgresSubscription"/>.
/// </summary>
public class PostgresSubscriptionBuilder
{
    private Type? _dataType;

    /// <summary>
    /// Sets the data type.
    /// </summary>
    /// <param name="type">The data type.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public PostgresSubscriptionBuilder MessageType(Type type)
    {
        _dataType = type ?? throw new ArgumentNullException(nameof(type));
        return this;
    }
    
    /// <summary>
    /// Sets the data type.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public PostgresSubscriptionBuilder MessageType<T>()
        where T : class, IRequest
    {
        return MessageType(typeof(T));
    }
    
    private SubscriptionName? _subscriptionName;
    
    /// <summary>
    /// Sets the subscription name.
    /// </summary>
    /// <param name="subscriptionName">The name of the subscription.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder SubscriptionName(SubscriptionName? subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }

    private ChannelName? _channelName;
    
    /// <summary>
    /// Sets the channel name (queue name).
    /// </summary>
    /// <param name="channelName">The name of the channel to use for the subscription.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder ChannelName(ChannelName? channelName)
    {
        _channelName = channelName;
        return this;
    }

    /// <summary>
    /// Sets the queue name (channel name).
    /// </summary>
    /// <param name="queueName">The name of the queue to use for the subscription.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder Queue(ChannelName? queueName)
        => ChannelName(queueName);
    

    private RoutingKey? _routingKey;
    
    /// <summary>
    /// Sets the routing key (topic name).
    /// </summary>
    /// <param name="routingKey">The routing key to use for message routing.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder RoutingKey(RoutingKey? routingKey)
    {
        _routingKey = routingKey;
        return this;
    }

    /// <summary>
    /// Sets the topic name (routing key).
    /// </summary>
    /// <param name="topicName">The routing key to use for message routing.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder Topic(RoutingKey? topicName) => RoutingKey(topicName);
    
    
    private int _bufferSize = 1;

    /// <summary>
    /// Sets the buffer size.
    /// </summary>
    /// <param name="bufferSize">The number of messages to buffer at once (min 1, max 10).</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder BufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }

    private int _noOfPerformers = Environment.ProcessorCount;
    
    /// <summary>
    /// Sets the number of performers (threads) for processing messages.
    /// </summary>
    /// <param name="noOfPerformers">The number of threads reading from the channel.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder NoOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }

    /// <summary>
    /// Sets the number of threads for processing messages.
    /// </summary>
    /// <param name="noOfPerformers">The number of threads reading from the channel.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder Concurrency(int noOfPerformers) => NoOfPerformers(noOfPerformers);
    
    private TimeSpan? _timeOut;

    /// <summary>
    /// Sets the timeout for message retrieval.
    /// </summary>
    /// <param name="timeOut">The timeout in milliseconds for retrieving messages.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder TimeOut(TimeSpan? timeOut)
    {
        _timeOut = timeOut;
        return this;
    }
    
    private int _requeueCount = -1;

    /// <summary>
    /// Sets the number of times to requeue a failed message.
    /// </summary>
    /// <param name="requeueCount">The number of retries before dropping a message.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder RequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }
    
    private TimeSpan? _requeueDelay;

    /// <summary>
    /// Sets the delay between message requeues.
    /// </summary>
    /// <param name="requeueDelay">The delay before retrying a failed message.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder RequeueDelay(TimeSpan? requeueDelay)
    {
        _requeueDelay = requeueDelay;
        return this;
    }
    
    private int _unacceptableMessageLimit;
    
    /// <summary>
    /// Sets the limit for unacceptable messages before stopping the subscription.
    /// </summary>
    /// <param name="unacceptableMessageLimit">The number of unacceptable messages to tolerate.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder UnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit;
        return this;
    }
    
    private MessagePumpType _messagePumpType = Paramore.Brighter.MessagePumpType.Reactor;

    /// <summary>
    /// Sets the message pump type (Reactor or Proactor).
    /// </summary>
    /// <param name="messagePumpType">The message pump type to use.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder MessagePump(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }

    /// <summary>
    /// Sets the message pump as <see cref="Paramore.Brighter.MessagePumpType.Proactor"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder AsProactor() => MessagePump(Paramore.Brighter.MessagePumpType.Proactor);
    
    /// <summary>
    /// Sets the message pump as <see cref="Paramore.Brighter.MessagePumpType.Reactor"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder AsReactor() => MessagePump(Paramore.Brighter.MessagePumpType.Reactor);
    
    private IAmAChannelFactory? _channelFactory;

    /// <summary>
    /// Sets the channel factory used for creating channels.
    /// </summary>
    /// <param name="channelFactory">The channel factory to use.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder ChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }
    
    private OnMissingChannel _makeChannels = OnMissingChannel.Create;
    
    /// <summary>
    /// Sets whether to create channels if they do not exist.
    /// </summary>
    /// <param name="makeChannels">Action to take when the channel is missing.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder MakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }

    /// <summary>
    /// Set the make channels as <see cref="OnMissingChannel.Create"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder CreateIfMissing() => MakeChannels(OnMissingChannel.Create);
    
    /// <summary>
    /// Set the make channels as <see cref="OnMissingChannel.Validate"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder Validate() => MakeChannels(OnMissingChannel.Validate);
    
    /// <summary>
    /// Set the make channels as <see cref="OnMissingChannel.Assume"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder Assume() => MakeChannels(OnMissingChannel.Assume);
    
    private TimeSpan? _emptyChannelDelay;

    /// <summary>
    /// Sets the delay to use when the channel is empty.
    /// </summary>
    /// <param name="emptyChannelDelay">The delay in milliseconds when no messages are available.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder EmptyDelay(TimeSpan? emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }

    private TimeSpan? _channelFailureDelay;
    
    /// <summary>
    /// Sets the delay to use when a channel failure occurs.
    /// </summary>
    /// <param name="channelFailureDelay">The delay in milliseconds when a channel fails.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder FailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }


    private string? _schemaName;
    
    /// <summary>
    /// Sets the schema name where the queue store table resides in the PostgreSQL database.
    /// If not explicitly set, the default schema name configured in the <see cref="PostgresMessagingGatewayConnection"/> will be used [[7]].
    /// </summary>
    /// <param name="schemaName">The schema name for the queue store table.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder SchemaName(string? schemaName)
    {
        _schemaName = schemaName;
        return this;
    }
    
    private string? _queueStoreTable;

    /// <summary>
    /// Sets the name of the queue store table in the PostgreSQL database.
    /// If not explicitly set, the default queue store table name configured in the <see cref="PostgresMessagingGatewayConnection"/> will be used [[7]].
    /// </summary>
    /// <param name="queueStoreTable">The name of the queue store table.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder QueueStoreTable(string? queueStoreTable)
    {
        _queueStoreTable = queueStoreTable;
        return this;
    }

    private bool? _binaryMessagePayload;
    
    /// <summary>
    /// Sets whether the message payload should be stored as binary JSON (JSONB) in the PostgreSQL database.
    /// Using JSONB can offer performance benefits over standard JSON [[9]].
    /// If not explicitly set, the default setting configured in the <see cref="PostgresMessagingGatewayConnection"/> will be used [[7]].
    /// </summary>
    /// <param name="binaryMessagePayload">True to store as JSONB, false to store as UTF-8 string.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder BinaryMessagePayload(bool? binaryMessagePayload)
    {
        _binaryMessagePayload = binaryMessagePayload;
        return this;
    }
    
    /// <summary>
    /// Enable binary message payload
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder EnableBinaryMessagePayload()
        => BinaryMessagePayload(true);
    
    /// <summary>
    /// Disable binary message payload
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder DisableBinaryMessagePayload()
        => BinaryMessagePayload(false);


    private TimeSpan? _visibleTimeout;
    
    /// <summary>
    /// Sets the duration for which a retrieved message is hidden from other consumers.
    /// Defaults to 30 seconds if not explicitly set.
    /// </summary>
    /// <param name="visibleTimeout">The timeout duration for message visibility.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder VisibleTimeout(TimeSpan? visibleTimeout)
    {
        _visibleTimeout = visibleTimeout;
        return this;
    }

    private bool _tableWithLargeMessage;

    /// <summary>
    /// Sets a flag indicating whether the queue table is configured to handle large messages stored as streams.
    /// </summary>
    /// <param name="tableWithLargeMessage">True if the table handles large messages; otherwise, false.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder TableWithLargeMessage(bool tableWithLargeMessage)
    {
        _tableWithLargeMessage = tableWithLargeMessage;
        return this;
    }

    /// <summary>
    /// Sets as true the flag indicating whether the queue table is configured to handle large messages stored as streams.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder EnableTableWithLargeMessage() => TableWithLargeMessage(true);
    
    /// <summary>
    /// Sets as falsethe flag indicating whether the queue table is configured to handle large messages stored as streams.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresSubscriptionBuilder DisableTableWithLargeMessage() => TableWithLargeMessage(false);

    /// <summary>
    /// Create a new instance of <see cref="PostgresSubscription"/> 
    /// </summary>
    /// <returns></returns>
    public PostgresSubscription Build()
    {
        if (_dataType == null)
        {
            throw new ConfigurationException("Missing data type");
        }
        
        return new PostgresSubscription(
            dataType: _dataType,
            subscriptionName: _subscriptionName,
            channelName: _channelName,
            routingKey: _routingKey,
            bufferSize: _bufferSize,
            noOfPerformers: _noOfPerformers,
            timeOut: _timeOut,
            requeueCount: _requeueCount,
            requeueDelay: _requeueDelay,
            unacceptableMessageLimit: _unacceptableMessageLimit,
            messagePumpType: _messagePumpType,
            channelFactory: _channelFactory,
            makeChannels: _makeChannels,
            emptyChannelDelay: _emptyChannelDelay,
            channelFailureDelay: _channelFailureDelay,
            schemaName: _schemaName,
            queueStoreTable: _queueStoreTable,
            binaryMessagePayload: _binaryMessagePayload,
            visibleTimeout: _visibleTimeout,
            tableWithLargeMessage: _tableWithLargeMessage
            );
    }
}
