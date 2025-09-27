using System;

using Org.Apache.Rocketmq;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RocketMQ;

using Message = Paramore.Brighter.Message;

namespace Fluent.Brighter.RocketMQ;

/// <summary>
/// Fluent builder for configuring RocketMQ subscriptions in Brighter pipeline.
/// Implements RocketMQ's consumer group model and message visibility controls through a fluent interface.
/// </summary>
public sealed class RocketSubscriptionBuilder
{
    private SubscriptionName? _subscriptionName;
    
    /// <summary>
    /// Sets the unique identifier for this subscription
    /// </summary>
    /// <param name="subscriptionName">Name identifying the subscription</param>
    /// <returns>This builder for fluent chaining</returns>
    public RocketSubscriptionBuilder SetSubscriptionName(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }
    
    private RoutingKey? _routingKey;
    
    /// <summary>
    /// Sets the binding key for topic-based routing
    /// </summary>
    /// <param name="routingKey">Routing pattern to bind queue to exchange</param>
    /// <returns>This builder for fluent chaining</returns>
    public RocketSubscriptionBuilder SetTopic(RoutingKey routingKey)
    {
        _routingKey = routingKey;
        return this;
    }
    
    private Type? _requestType;

    /// <summary>
    /// Sets the request type for message handling.
    /// </summary>
    /// <param name="requestType">The type of request this subscription handles.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetRequestType(Type requestType)
    {
        _requestType = requestType ?? throw new ArgumentNullException(nameof(requestType));
        return this;
    }
    
    private Func<Message, Type>? _getRequestType;

    /// <summary>
    /// Sets the function for mapping messages to request types.
    /// </summary>
    /// <param name="getRequestType">The mapping function.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetRequestTypeMapper(Func<Message, Type> getRequestType)
    {
        _getRequestType = getRequestType ?? throw new ArgumentNullException(nameof(getRequestType));
        return this;
    }
    
    private string? _consumerGroup;
    private ChannelName? _channelName;
    
    /// <summary>
    /// Sets the consumer group for RocketMQ message consumption.
    /// Identifies a group of consumers working as a cluster for parallel processing.
    /// </summary>
    /// <param name="consumerGroup">The consumer group name.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetConsumerGroup(string consumerGroup)
    {
        _consumerGroup = consumerGroup ?? throw new ArgumentNullException(nameof(consumerGroup));
        _channelName = new  ChannelName(consumerGroup);
        return this;
    }
    
    private int _bufferSize = 1;

    /// <summary>
    /// Sets the buffer size for message batching.
    /// </summary>
    /// <param name="bufferSize">The number of messages to buffer (min 1, max 10).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetBufferSize(int bufferSize)
    {
        if (bufferSize < 1 || bufferSize > 10)
            throw new ArgumentException("Buffer size must be between 1 and 10", nameof(bufferSize));
        
        _bufferSize = bufferSize;
        return this;
    }
    
    private int _noOfPerformers = 1;

    /// <summary>
    /// Sets the number of performer threads for parallel message processing.
    /// </summary>
    /// <param name="noOfPerformers">The number of threads.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetNoOfPerformers(int noOfPerformers)
    {
        if (noOfPerformers < 1)
            throw new ArgumentException("Number of performers must be at least 1", nameof(noOfPerformers));
        
        _noOfPerformers = noOfPerformers;
        return this;
    }
    
    private TimeSpan? _timeOut;

    /// <summary>
    /// Sets the operation timeout for message processing.
    /// </summary>
    /// <param name="timeOut">The timeout duration.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetTimeout(TimeSpan timeOut)
    {
        _timeOut = timeOut;
        return this;
    }
    
    private int _requeueCount = -1;

    /// <summary>
    /// Sets the maximum number of requeue attempts for failed messages.
    /// </summary>
    /// <param name="requeueCount">The maximum requeue count.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetRequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }
    
    private TimeSpan? _requeueDelay;

    /// <summary>
    /// Sets the delay before requeuing failed messages.
    /// </summary>
    /// <param name="requeueDelay">The delay duration.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetRequeueDelay(TimeSpan requeueDelay)
    {
        _requeueDelay = requeueDelay;
        return this;
    }
    
    private int _unacceptableMessageLimit = 0;

    /// <summary>
    /// Sets the limit for unacceptable messages before stopping consumption.
    /// </summary>
    /// <param name="unacceptableMessageLimit">The message limit.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetUnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        if (unacceptableMessageLimit < 0)
            throw new ArgumentException("Unacceptable message limit cannot be negative", nameof(unacceptableMessageLimit));
        
        _unacceptableMessageLimit = unacceptableMessageLimit;
        return this;
    }
    
    private MessagePumpType _messagePumpType = MessagePumpType.Unknown;

    /// <summary>
    /// Sets the message pump type for asynchronous processing.
    /// </summary>
    /// <param name="messagePumpType">The pump type.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetMessagePumpType(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }
    
    private IAmAChannelFactory? _channelFactory;
    
    /// <summary>
    /// Sets the channel factory for creating message channels.
    /// </summary>
    /// <param name="channelFactory">The channel factory instance.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetChannelFactory(IAmAChannelFactory channelFactory)
    {
        _channelFactory = channelFactory ?? throw new ArgumentNullException(nameof(channelFactory));
        return this;
    }
    
    private OnMissingChannel _makeChannels = OnMissingChannel.Create;

    /// <summary>
    /// Sets the behavior for missing channels.
    /// </summary>
    /// <param name="makeChannels">The missing channel behavior.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }
    
    private FilterExpression? _filter;
    
    /// <summary>
    /// Sets the message filter expression for RocketMQ topic filtering.
    /// Supports RocketMQ's tag-based or SQL-style message filtering.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetFilter(FilterExpression filter)
    {
        _filter = filter ?? throw new ArgumentNullException(nameof(filter));
        return this;
    }
    
    private TimeSpan? _emptyChannelDelay;
    
    /// <summary>
    /// Sets the delay when a channel is empty.
    /// </summary>
    /// <param name="emptyChannelDelay">The delay duration.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetEmptyChannelDelay(TimeSpan emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }
    
    private TimeSpan? _channelFailureDelay;

    /// <summary>
    /// Sets the delay when a channel failure occurs.
    /// </summary>
    /// <param name="channelFailureDelay">The delay duration.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetChannelFailureDelay(TimeSpan channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }

    private TimeSpan? _receiveMessageTimeout;
    
    /// <summary>
    /// Sets the timeout for receiving messages from RocketMQ brokers.
    /// Controls the maximum time to wait for new messages during polling.
    /// </summary>
    /// <param name="receiveMessageTimeout">The receive timeout.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetReceiveMessageTimeout(TimeSpan receiveMessageTimeout)
    {
        _receiveMessageTimeout = receiveMessageTimeout;
        return this;
    }

    private TimeSpan? _invisibilityTimeout;
    
    /// <summary>
    /// Sets the invisibility timeout for consumed messages.
    /// Determines how long messages remain invisible after being received.
    /// </summary>
    /// <param name="invisibilityTimeout">The invisibility timeout.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketSubscriptionBuilder SetInvisibilityTimeout(TimeSpan invisibilityTimeout)
    {
        _invisibilityTimeout = invisibilityTimeout;
        return this;
    }

    /// <summary>
    /// Builds the final RocketSubscription instance with the configured settings.
    /// </summary>
    /// <returns>A configured instance of RocketSubscription.</returns>
    internal RocketSubscription Build()
    {
        if (_subscriptionName == null)
        {
            throw new ConfigurationException("Subscription name not set");
        }

        if (ChannelName.IsNullOrEmpty(_channelName))
        {
            throw new ConfigurationException("Channel name not set");
        }

        if (RoutingKey.IsNullOrEmpty(_routingKey))
        {
            throw new ConfigurationException("Routing key not set");
        }
        
        return new RocketSubscription(
            _subscriptionName,
            _channelName,
            _routingKey,
            _requestType,
            _getRequestType,
            _consumerGroup,
            _bufferSize,
            _noOfPerformers,
            _timeOut,
            _requeueCount,
            _requeueDelay,
            _unacceptableMessageLimit,
            _messagePumpType,
            _channelFactory,
            _makeChannels,
            _filter,
            _emptyChannelDelay,
            _channelFailureDelay,
            _receiveMessageTimeout,
            _invisibilityTimeout
        );
    }
}