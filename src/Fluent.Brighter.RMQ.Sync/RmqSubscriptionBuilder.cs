using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Sync;

namespace Fluent.Brighter.RMQ.Sync;

/// <summary>
/// Fluent builder for configuring RabbitMQ message subscriptions in Brighter
/// </summary>
/// <remarks>
/// Provides a fluent interface to define consumer parameters including queue binding,
/// message processing behavior, error handling, and queue infrastructure settings.
/// Required parameters: SubscriptionName, ChannelName (queue), and RoutingKey (binding key).
/// </remarks>
public sealed class RmqSubscriptionBuilder
{
    private SubscriptionName? _subscriptionName;

    /// <summary>
    /// Sets the unique identifier for this subscription
    /// </summary>
    /// <param name="subscriptionName">Name identifying the subscription</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetSubscription(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }
    
    private ChannelName? _channelName;
    
    /// <summary>
    /// Sets the queue name to consume from
    /// </summary>
    /// <param name="channelName">Name of the RabbitMQ queue</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetQueue(ChannelName channelName)
    {
        _channelName = channelName;
        return this;
    }
    
    private RoutingKey? _routingKey;
    
    /// <summary>
    /// Sets the binding key for topic-based routing
    /// </summary>
    /// <param name="routingKey">Routing pattern to bind queue to exchange</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetTopic(RoutingKey routingKey)
    {
        _routingKey = routingKey;
        return this;
    }
    
    private Type? _dataType;

    /// <summary>
    /// Sets the expected message type and configures default names if not set
    /// </summary>
    /// <remarks>
    /// When provided (non-null):
    /// - Sets SubscriptionName to type's full name if unset
    /// - Sets ChannelName to type's short name if unset
    /// - Sets RoutingKey to type's short name if unset
    /// </remarks>
    /// <param name="dataType">The .NET type of expected messages</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetDataType(Type? dataType)
    {
        _dataType = dataType;
        if (dataType == null) return this;
        
        _subscriptionName ??= new SubscriptionName(dataType.ToString());
        _channelName = ChannelName.IsNullOrEmpty(_channelName) 
            ? new ChannelName(dataType.Name) 
            : _channelName;
        _routingKey = RoutingKey.IsNullOrEmpty(_routingKey) 
            ? new RoutingKey(dataType.Name) 
            : _routingKey;
        
        return this;
    }
    
    private Func<Message, Type>? _getRequestType;

    /// <summary>
    /// Sets a custom message type resolver
    /// </summary>
    /// <param name="getRequestType">Function to determine message type from headers/body</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetGetRequestType(Func<Message, Type>? getRequestType)
    {
        _getRequestType = getRequestType;
        return this;
    }

    private int _bufferSize = 1;

    /// <summary>
    /// Sets the number of messages to prefetch (default: 1)
    /// </summary>
    /// <param name="bufferSize">Prefetch count (QoS setting)</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetBufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }
    
    private int _noOfPerformers  = 1;

    /// <summary>
    /// Sets the number of concurrent consumers (default: 1)
    /// </summary>
    /// <param name="noOfPerformers">Concurrent message processing threads</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetNumberOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }
    
    private TimeSpan? _timeOut;
    
    /// <summary>
    /// Sets the message processing timeout (optional)
    /// </summary>
    /// <param name="timeout">Max duration for message processing</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetTimeout(TimeSpan? timeout)
    {
        _timeOut = timeout;
        return this;
    }
    
    private int _requeueCount = -1;
    
    /// <summary>
    /// Sets maximum requeue attempts before discarding (default: -1 infinite)
    /// </summary>
    /// <param name="requeueCount">Max retry attempts (-1 = infinite)</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetRequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }
    
    private TimeSpan? _requeueDelay;
    
    /// <summary>
    /// Sets delay before requeueing failed messages (optional)
    /// </summary>
    /// <param name="timeout">Delay duration before retry</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetRequeueDelay(TimeSpan? timeout)
    {
        _requeueDelay = timeout;
        return this;
    }
    
    private int _unacceptableMessageLimit;
    
    /// <summary>
    /// Sets consecutive error threshold before circuit breaking (default: 0 disabled)
    /// </summary>
    /// <param name="unacceptableMessageLimit">Consecutive failure limit</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetUnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit;
        return this;
    }
    
    private MessagePumpType _messagePumpType = MessagePumpType.Proactor;
    
    /// <summary>
    /// Sets message processing model (default: Proactor)
    /// </summary>
    /// <param name="messagePumpType">Threading model for message processing</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetMessagePumpType(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }

    private IAmAChannelFactory? _channelFactory;

    /// <summary>
    /// Sets custom channel factory implementation (optional)
    /// </summary>
    /// <param name="channelFactory">Custom channel creation logic</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }
    
    private OnMissingChannel _onMissingChannel = OnMissingChannel.Create;
    
    /// <summary>
    /// Sets behavior when queues/channels are missing (default: Create)
    /// </summary>
    /// <param name="onMissingChannel">Action to take when infrastructure missing</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetMakeChannels(OnMissingChannel onMissingChannel)
    {
        _onMissingChannel = onMissingChannel;
        return this;
    }

    private TimeSpan? _emptyChannelDelay;
    
    /// <summary>
    /// Sets polling delay when no messages available (optional)
    /// </summary>
    /// <param name="emptyChannelDelay">Delay between empty queue checks</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetEmptyChannelDelay(TimeSpan? emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }

    private TimeSpan? _channelFailureDelay;
    
    /// <summary>
    /// Sets recovery delay after channel failures (optional)
    /// </summary>
    /// <param name="channelFailureDelay">Delay before recreating failed channels</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetChannelFailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }

    private int? _maxQueueLenght;

    /// <summary>
    /// Sets maximum queue depth (optional)
    /// </summary>
    /// <param name="maxQueueLenght">Maximum messages in queue</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetMaxQueueLenght(int? maxQueueLenght)
    {
        _maxQueueLenght = maxQueueLenght;
        return this;
    }

    private bool _isDurable;

    /// <summary>
    /// Configures queue durability (default: non-durable)
    /// </summary>
    /// <param name="isDurable">True to survive broker restarts</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetIsDurable(bool isDurable)
    {
        _isDurable = isDurable;
        return this;
    }
    
    private bool _highAvailability;

    /// <summary>
    /// Enables mirrored queues across cluster (HA) (default: false)
    /// </summary>
    /// <param name="highAvailability">True for cluster-wide queue mirroring</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SethHighAvailability(bool highAvailability)
    {
        _highAvailability = highAvailability;
        return this;
    }

    private ChannelName? _deadLetterChannelName;

    /// <summary>
    /// Sets dead letter queue name (optional)
    /// </summary>
    /// <param name="deadLetterChannelName">Queue for failed/unprocessable messages</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetDeadLetterChannel(ChannelName? deadLetterChannelName)
    {
        _deadLetterChannelName = deadLetterChannelName;
        return this;
    }
    
    private RoutingKey? _deadLetterRoutingKey;
    
    /// <summary>
    /// Sets dead letter routing key (optional)
    /// </summary>
    /// <param name="deadLetterRoutingKey">Routing key for dead letter exchange</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetDeadLetterRoutingKey(RoutingKey? deadLetterRoutingKey)
    {
        _deadLetterRoutingKey = deadLetterRoutingKey;
        return this;
    }
    
    private TimeSpan? _ttl;

    /// <summary>
    /// Sets message time-to-live in queue (optional)
    /// </summary>
    /// <param name="ttl">Maximum time messages stay in queue</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqSubscriptionBuilder SetTTL(TimeSpan? ttl)
    {
        _ttl = ttl;
        return this;
    }
    
    /// <summary>
    /// Constructs the final RmqSubscription configuration
    /// </summary>
    /// <returns>Validated subscription configuration</returns>
    /// <exception cref="ConfigurationException">
    /// Thrown if SubscriptionName, ChannelName, or RoutingKey are not set
    /// </exception>
    internal RmqSubscription Build()
    {
        if (_subscriptionName == null)
        {
            throw new ConfigurationException("Subscription name is required. Set it via SetSubscription() or SetDataType().");
        }
    
        if (_channelName == null)
        {
            throw new ConfigurationException("Queue name (ChannelName) is required. Set it via SetQueue() or SetDataType().");
        }

        if (_routingKey == null)
        {
            throw new ConfigurationException("Routing key (Topic) is required. Set it via SetTopic() or SetDataType().");
        }
        
        return new RmqSubscription(
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
            channelFailureDelay: _channelFailureDelay,
            maxQueueLength: _maxQueueLenght,
            isDurable: _isDurable,
            highAvailability: _highAvailability,
            deadLetterChannelName: _deadLetterChannelName,
            deadLetterRoutingKey: _deadLetterRoutingKey,
            ttl: _ttl);
    }
}