using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Redis;

namespace Fluent.Brighter.Redis;

/// <summary>
/// Builder class for configuring and creating a Redis subscription.
/// A subscription defines how messages are consumed from Redis, including channel settings,
/// message processing options, retry behavior, and Redis-specific configurations.
/// </summary>
public sealed class RedisSubscriptionBuilder
{
    private SubscriptionName _subscriptionName = new(Uuid.New().ToString("N"));
    
    /// <summary>
    /// Sets the subscription name that uniquely identifies this subscription.
    /// The subscription name is used to track and manage the subscription within the Brighter framework.
    /// </summary>
    /// <param name="subscriptionName">The unique name for this subscription.</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetSubscription(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }
    
    private ChannelName? _channelName;
    private RoutingKey? _routingKey;
    
    /// <summary>
    /// Sets the channel name and routing key for the subscription.
    /// The channel represents the Redis queue/topic from which messages will be consumed.
    /// This method automatically sets the routing key to match the channel name.
    /// </summary>
    /// <param name="channelName">The name of the channel to subscribe to.</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetQueue(ChannelName channelName)
    {
        _channelName = channelName;
        return this;
    }
    
    /// <summary>
    /// Sets the routing key (topic) for the subscription.
    /// This determines which messages will be routed to this subscription based on the topic pattern.
    /// </summary>
    /// <param name="routingKey">The routing key identifying the Redis topic pattern.</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetTopic(RoutingKey routingKey)
    {
        _routingKey = routingKey;
        return this;
    }
    
    private Type? _dataType;
    
    /// <summary>
    /// Sets the data type for messages in this subscription.
    /// When provided, this method will automatically configure the subscription name, channel name,
    /// and routing key based on the type name if they haven't been explicitly set.
    /// </summary>
    /// <param name="dataType">The .NET type of the messages that will be consumed, or null if not specified.</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetDataType(Type? dataType)
    {
        _dataType = dataType;
        
        if (dataType != null)
        {
            var typeName = dataType.FullName!;
            _subscriptionName ??= new SubscriptionName(typeName);
            _channelName ??= new ChannelName(typeName);
            _routingKey ??= new RoutingKey(typeName);
        }
        
        return this;
    }
    
    private Func<Message, Type>? _getRequestType;
    
    /// <summary>
    /// Sets a function that determines the request type dynamically based on the message content.
    /// This allows for polymorphic message handling where different message types can be processed by different handlers.
    /// </summary>
    /// <param name="getRequestType">A function that takes a <see cref="Message"/> and returns the corresponding .NET type, or null to use the default behavior.</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetGetRequestType(Func<Message, Type>? getRequestType)
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
    /// <param name="bufferSize">The buffer size (number of messages). Must be between 1 and 10. Default is 1.</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetBufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }
    
    private int _noOfPerformers = 1;
    
    /// <summary>
    /// Sets the number of performer threads that will process messages concurrently.
    /// Multiple performers allow parallel message processing, improving throughput for I/O-bound operations.
    /// </summary>
    /// <param name="noOfPerformers">The number of concurrent performers. Default is 1.</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetNumberOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }
    
    private TimeSpan? _timeOut;
    
    /// <summary>
    /// Sets the timeout duration for message processing operations.
    /// If a message handler takes longer than this timeout, the operation will be cancelled.
    /// </summary>
    /// <param name="timeout">The timeout duration, or null to use the default timeout (300ms).</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetTimeout(TimeSpan? timeout)
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
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetRequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }
    
    private TimeSpan? _requeueDelay;
    
    /// <summary>
    /// Sets the delay before a failed message is requeued for retry.
    /// This provides a backoff period before attempting to process the message again, which can help with transient failures.
    /// </summary>
    /// <param name="requeueDelay">The requeue delay duration, or null to use the default delay (zero).</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetRequeueDelay(TimeSpan? requeueDelay)
    {
        _requeueDelay = requeueDelay;
        return this;
    }
    
    private int _unacceptableMessageLimit;
    
    /// <summary>
    /// Sets the limit for the number of unacceptable messages that can be received before the channel stops processing.
    /// Unacceptable messages are those that cannot be deserialized or are malformed. This limit prevents endless processing of bad messages.
    /// </summary>
    /// <param name="unacceptableMessageLimit">The maximum number of unacceptable messages before stopping. Use 0 for unlimited.</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetUnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit;
        return this;
    }
    
    private MessagePumpType _messagePumpType = MessagePumpType.Proactor;
    
    /// <summary>
    /// Sets the message pump type that determines how messages are processed.
    /// Proactor uses async/await patterns for non-blocking I/O, while Reactor uses synchronous processing.
    /// </summary>
    /// <param name="messagePumpType">The <see cref="MessagePumpType"/> to use. Default is <see cref="MessagePumpType.Proactor"/>.</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetMessagePumpType(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }
    
    private IAmAChannelFactory? _channelFactory;
    
    /// <summary>
    /// Sets a custom channel factory for creating message channels.
    /// Use this when you need to provide custom channel creation logic beyond the default Redis channel factory.
    /// </summary>
    /// <param name="channelFactory">The custom channel factory implementation, or null to use the default.</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }
    
    private OnMissingChannel _onMissingChannel = OnMissingChannel.Create;
    
    /// <summary>
    /// Sets the behavior for handling missing channels/topics in Redis.
    /// Determines whether to create, validate, or assume the existence of channels when they are not found.
    /// </summary>
    /// <param name="onMissingChannel">The <see cref="OnMissingChannel"/> behavior to apply. Default is <see cref="OnMissingChannel.Create"/>.</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetMakeChannels(OnMissingChannel onMissingChannel)
    {
        _onMissingChannel = onMissingChannel;
        return this;
    }
    
    private TimeSpan? _emptyChannelDelay;
    
    /// <summary>
    /// Sets the delay before checking for messages again when the channel is empty.
    /// This prevents tight polling loops and reduces load on Redis when no messages are available.
    /// </summary>
    /// <param name="emptyChannelDelay">The delay duration when the channel is empty, or null to use the default delay (500ms).</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetEmptyChannelDelay(TimeSpan? emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }
    
    private TimeSpan? _channelFailureDelay;
    
    /// <summary>
    /// Sets the delay before retrying channel operations after a failure.
    /// This provides a backoff period when channel errors occur, preventing rapid retry loops that could overwhelm the system.
    /// </summary>
    /// <param name="channelFailureDelay">The delay duration after a channel failure, or null to use the default delay (1000ms).</param>
    /// <returns>The current <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public RedisSubscriptionBuilder SetChannelFailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }
    
    /// <summary>
    /// Builds and returns a configured <see cref="RedisSubscription"/> instance.
    /// This method is called internally to create the subscription with all the configured settings
    /// including channel configuration, message processing options, and Redis-specific settings.
    /// </summary>
    /// <returns>A configured <see cref="RedisSubscription"/> instance.</returns>
    /// <exception cref="ConfigurationException">Thrown when required configuration is missing: subscription name, channel name, or routing key.</exception>
    internal RedisSubscription Build()
    {
        if (_subscriptionName == null)
        {
            throw new ConfigurationException("Subscription name is required");
        }
        
        if (_channelName == null)
        {
            throw new ConfigurationException("Channel name is required");
        }
        
        if (_routingKey == null)
        {
            throw new ConfigurationException("Routing key is required");
        }
        
        return new TmpRedisSubscription(
            subscriptionName: _subscriptionName,
            channelName: _channelName,
            routingKey: _routingKey,
            requestType: _dataType,
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
            channelFailureDelay: _channelFailureDelay
        );
    }

    private class TmpRedisSubscription(
        SubscriptionName subscriptionName,
        ChannelName channelName,
        RoutingKey routingKey,
        Type? requestType = null,
        Func<Message, Type>? getRequestType = null,
        int bufferSize = 1,
        int noOfPerformers = 1,
        TimeSpan? timeOut = null,
        int requeueCount = -1,
        TimeSpan? requeueDelay = null,
        int unacceptableMessageLimit = 0,
        MessagePumpType messagePumpType = MessagePumpType.Proactor,
        IAmAChannelFactory? channelFactory = null,
        OnMissingChannel makeChannels = OnMissingChannel.Create,
        TimeSpan? emptyChannelDelay = null,
        TimeSpan? channelFailureDelay = null) : RedisSubscription(
        subscriptionName, 
        channelName, 
        routingKey, 
        requestType, 
        getRequestType, 
        bufferSize, 
        noOfPerformers, 
        timeOut, 
        requeueCount, 
        requeueDelay, 
        unacceptableMessageLimit, 
        messagePumpType, 
        channelFactory, 
        makeChannels, 
        emptyChannelDelay, 
        channelFailureDelay 
        );
}