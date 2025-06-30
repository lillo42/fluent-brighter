
using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Sync;

namespace Fluent.Brighter.RMQ.Sync;

/// <summary>
/// A fluent builder for creating RmqSubscription instances with optional parameters.
/// </summary>
public class RmqSubscriptionBuilder
{
    private Type? _dataType;

    /// <summary>
    /// Sets the data type.
    /// </summary>
    /// <param name="type">The data type.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public RmqSubscriptionBuilder MessageType(Type type)
    {
        _dataType = type ?? throw new ArgumentNullException(nameof(type));
        return this;
    }
    
    /// <summary>
    /// Sets the data type.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public RmqSubscriptionBuilder MessageType<T>()
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
    public RmqSubscriptionBuilder SubscriptionName(SubscriptionName? subscriptionName)
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
    public RmqSubscriptionBuilder ChannelName(ChannelName? channelName)
    {
        _channelName = channelName;
        return this;
    }

    /// <summary>
    /// Sets the queue name (channel name).
    /// </summary>
    /// <param name="queueName">The name of the queue to use for the subscription.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder Queue(ChannelName? queueName)
        => ChannelName(queueName);
    

    private RoutingKey? _routingKey;
    
    /// <summary>
    /// Sets the routing key (topic name).
    /// </summary>
    /// <param name="routingKey">The routing key to use for message routing.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder RoutingKey(RoutingKey? routingKey)
    {
        _routingKey = routingKey;
        return this;
    }

    /// <summary>
    /// Sets the topic name (routing key).
    /// </summary>
    /// <param name="topicName">The routing key to use for message routing.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder Topic(RoutingKey? topicName) => RoutingKey(topicName);
    
    
    private int _bufferSize = 1;

    /// <summary>
    /// Sets the buffer size.
    /// </summary>
    /// <param name="bufferSize">The number of messages to buffer at once (min 1, max 10).</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder BufferSize(int bufferSize)
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
    public RmqSubscriptionBuilder NoOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }

    /// <summary>
    /// Sets the number of threads for processing messages.
    /// </summary>
    /// <param name="noOfPerformers">The number of threads reading from the channel.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder Concurrency(int noOfPerformers) => NoOfPerformers(noOfPerformers);
    
    private TimeSpan? _timeOut;

    /// <summary>
    /// Sets the timeout for message retrieval.
    /// </summary>
    /// <param name="timeOut">The timeout in milliseconds for retrieving messages.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder TimeOut(TimeSpan? timeOut)
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
    public RmqSubscriptionBuilder RequeueCount(int requeueCount)
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
    public RmqSubscriptionBuilder RequeueDelay(TimeSpan? requeueDelay)
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
    public RmqSubscriptionBuilder UnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit;
        return this;
    }
    
    private bool _isDurable;
    
    /// <summary>
    /// Sets whether the queue should be durable across broker restarts.
    /// </summary>
    /// <param name="isDurable">True if the queue is durable; otherwise, false.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder Durable(bool isDurable)
    {
        _isDurable = isDurable;
        return this;
    }

    /// <summary>
    /// Enable durable queue.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder EnableDurable() => Durable(true);
    
    /// <summary>
    /// Disable durable queue.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder DisableDurable() => Durable(false);
    
    private MessagePumpType _messagePumpType = Paramore.Brighter.MessagePumpType.Reactor;

    /// <summary>
    /// Sets the message pump type (Reactor or Proactor).
    /// </summary>
    /// <param name="messagePumpType">The message pump type to use.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder MessagePump(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }

    /// <summary>
    /// Sets the message pump as <see cref="Paramore.Brighter.MessagePumpType.Proactor"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder AsProactor() => MessagePump(Paramore.Brighter.MessagePumpType.Proactor);
    
    /// <summary>
    /// Sets the message pump as <see cref="Paramore.Brighter.MessagePumpType.Reactor"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder AsReactor() => MessagePump(Paramore.Brighter.MessagePumpType.Reactor);
    
    private IAmAChannelFactory? _channelFactory;

    /// <summary>
    /// Sets the channel factory used for creating channels.
    /// </summary>
    /// <param name="channelFactory">The channel factory to use.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder ChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }
    
    private bool _highAvailability;
    
    /// <summary>
    /// Sets whether the queue should be mirrored across nodes in the cluster.
    /// </summary>
    /// <param name="highAvailability">True if high availability is enabled; otherwise, false.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder HighAvailability(bool highAvailability)
    {
        _highAvailability = highAvailability;
        return this;
    }
    
    /// <summary>
    /// Enable high availability  queue
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder EnableHighAvailability() => HighAvailability(true);
    
    /// <summary>
    /// Disable high availability  queue
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder DisableHighAvailability() => HighAvailability(false);
    
    private ChannelName? _deadLetterChannelName;

    /// <summary>
    /// Sets the dead letter channel name for rejected messages.
    /// </summary>
    /// <param name="deadLetterChannelName">The channel name for dead-lettered messages.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder DeadLetter(ChannelName? deadLetterChannelName)
    {
        _deadLetterChannelName = deadLetterChannelName;
        return this;
    }

    private RoutingKey? _deadLetterRoutingKey;
    
    /// <summary>
    /// Sets the dead letter routing key for rejected messages.
    /// </summary>
    /// <param name="deadLetterRoutingKey">The routing key for dead-lettered messages.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder DeadLetterRoutingKey(RoutingKey? deadLetterRoutingKey)
    {
        _deadLetterRoutingKey = deadLetterRoutingKey;
        return this;
    }

    private TimeSpan? _ttl;
    
    /// <summary>
    /// Sets the time-to-live (TTL) for messages in the queue.
    /// </summary>
    /// <param name="ttl">The time a message can remain in the queue before expiring.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder Ttl(TimeSpan? ttl)
    {
        _ttl = ttl;
        return this;
    }
    
    private OnMissingChannel _makeChannels = OnMissingChannel.Create;
    
    /// <summary>
    /// Sets whether to create channels if they do not exist.
    /// </summary>
    /// <param name="makeChannels">Action to take when the channel is missing.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder MakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }

    /// <summary>
    /// Set the make channels as <see cref="OnMissingChannel.Create"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder CreateIfMissing() => MakeChannels(OnMissingChannel.Create);
    
    /// <summary>
    /// Set the make channels as <see cref="OnMissingChannel.Validate"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder Validate() => MakeChannels(OnMissingChannel.Validate);
    
    /// <summary>
    /// Set the make channels as <see cref="OnMissingChannel.Assume"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder Assume() => MakeChannels(OnMissingChannel.Assume);
    
    private TimeSpan? _emptyChannelDelay;

    /// <summary>
    /// Sets the delay to use when the channel is empty.
    /// </summary>
    /// <param name="emptyChannelDelay">The delay in milliseconds when no messages are available.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder EmptyDelay(TimeSpan? emptyChannelDelay)
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
    public RmqSubscriptionBuilder FailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }

    private int? _maxQueueLength;
    
    /// <summary>
    /// Sets the maximum number of messages allowed in the queue.
    /// </summary>
    /// <param name="maxQueueLength">The maximum number of messages in the queue before rejection.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder MaxQueueLength(int? maxQueueLength)
    {
        _maxQueueLength = maxQueueLength;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured RmqSubscription instance.
    /// </summary>
    /// <returns>The constructed RmqSubscription instance.</returns>
    internal RmqSubscription Build()
    {
        if (_dataType == null)
        {
            throw new ConfigurationException("Missing data type");
        }
        
        return new RmqSubscription(
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
            isDurable: _isDurable,
            messagePumpType: _messagePumpType,
            channelFactory: _channelFactory,
            highAvailability: _highAvailability,
            deadLetterChannelName: _deadLetterChannelName,
            deadLetterRoutingKey: _deadLetterRoutingKey,
            ttl: _ttl,
            makeChannels: _makeChannels,
            emptyChannelDelay: _emptyChannelDelay,
            channelFailureDelay: _channelFailureDelay,
            maxQueueLength: _maxQueueLength
        );
    }
}