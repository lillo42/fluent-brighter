
using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ;

namespace Fluent.Brighter.RMQ;

/// <summary>
/// Fluent builder for configuring RabbitMQ subscription settings.
/// Provides a chainable API to define queue, exchange, and message handling behavior 
/// before creating the final <see cref="RmqSubscription"/> instance.
/// </summary>
public class RmqSubscriptionBuilder
{
    private ChannelName? _deadLetterName;

    /// <summary>
    /// Sets the dead letter queue name for undeliverable messages.
    /// </summary>
    /// <param name="deadLetterName">Name of the dead letter queue.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="deadLetterName"/> is <see langword="null"/> or empty.</exception>
    public RmqSubscriptionBuilder DeadLetter(string deadLetterName)
    {
        return string.IsNullOrEmpty(deadLetterName)
            ? throw new ArgumentException("DeadLetter can't be null or empty", nameof(deadLetterName))
            : DeadLetter(new ChannelName(deadLetterName));
    }

    /// <summary>
    /// Sets the dead letter queue using a <see cref="Paramore.Brighter.ChannelName"/> object.
    /// </summary>
    /// <param name="deadLetterName">Pre-constructed channel name for dead letter queue.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder DeadLetter(ChannelName deadLetterName)
    {
        _deadLetterName = deadLetterName ?? throw new ArgumentNullException(nameof(deadLetterName));
        return string.IsNullOrEmpty(_deadLetterRoutingKey) ? DeadLetterRoutingKey(deadLetterName.Value) : this;
    }

    private string? _deadLetterRoutingKey;

    /// <summary>
    /// Sets the routing key for the dead letter queue.
    /// </summary>
    /// <param name="routingKey">Routing key to use for dead letter messages.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="routingKey"/> is <see langword="null"/> or empty.</exception>
    public RmqSubscriptionBuilder DeadLetterRoutingKey(string routingKey)
    {
        if (string.IsNullOrEmpty(routingKey))
        {
            throw new ArgumentException("Routing key can't be null or empty", nameof(routingKey));
        }

        _deadLetterRoutingKey = routingKey;
        return this;
    }

    private bool _highAvailability;
    /// <summary>
    /// Enables high availability mode for the queue.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder EnableHighAvailability()
    {
        return HighAvailability(true);
    }

    /// <summary>
    /// Disables high availability mode for the queue.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder DisableHighAvailability()
    {
        return HighAvailability(false);
    }

    /// <summary>
    /// Sets whether the queue should be highly available.
    /// </summary>
    /// <param name="highAvailability">True to enable HA, false to disable.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder HighAvailability(bool highAvailability)
    {
        _highAvailability = highAvailability;
        return this;
    }

    private bool _durable;

    /// <summary>
    /// Enables durable queue configuration (survives broker restarts).
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder EnableDurable()
    {
        return Durable(true);
    }

    /// <summary>
    /// Disables durable queue configuration (non-persistent).
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder DisableDurable()
    {
        return Durable(false);
    }

    /// <summary>
    /// Sets whether the queue should be durable.
    /// </summary>
    /// <param name="durable">True for durable, <see langword="false"/> for transient.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder Durable(bool durable)
    {
        _durable = durable;
        return this;
    }

    private int? _maxQueueLenght;

    /// <summary>
    /// Sets the maximum queue length (message backlog limit).
    /// </summary>
    /// <param name="maxQueueLenght">Maximum number of messages allowed in the queue (<see langword="null"/> for unlimited).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder MaxQueueLenght(int? maxQueueLenght)
    {
        _maxQueueLenght = maxQueueLenght;
        return this;
    }

    private int? _ttl;
    /// <summary>
    /// Sets the time-to-live (TTL) for messages in the queue.
    /// </summary>
    /// <param name="ttl">Time in milliseconds that messages are kept in the queue (<see langword="null"/> for unlimited).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder Ttl(int? ttl)
    {
        _ttl = ttl;
        return this;
    }

    private Type? _dataType;

    /// <summary>
    /// Sets the message type for the subscription using a generic type parameter.
    /// </summary>
    /// <typeparam name="T">The request type that this subscription will handle.</typeparam>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder MessageType<T>()
        where T : class, IRequest
    {
        return MessageType(typeof(T));
    }

    /// <summary>
    /// Sets the message type for the subscription using a Type object.
    /// </summary>
    /// <param name="dataType">The request type that this subscription will handle.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="dataType"/> does not implement <see cref="IRequest"/>.</exception>
    public RmqSubscriptionBuilder MessageType(Type dataType)
    {
        _dataType = dataType ?? throw new ArgumentNullException(nameof(dataType));
        if (_channelName == null)
        {
            _ = ChannelName(dataType.FullName!);
        }

        if (_routingKey == null)
        {
            _ = TopicName(dataType.FullName!);
        }

        return this;
    }

    private SubscriptionName? _subscriptionName;

    /// <summary>
    /// Sets the subscription name.
    /// </summary>
    /// <param name="subscriptionName">Name for the subscription.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="subscriptionName"/> is <see langword="null"/> or empty.</exception>
    public RmqSubscriptionBuilder SubscriptionName(string subscriptionName)
    {
        return string.IsNullOrEmpty(subscriptionName)
            ? throw new ArgumentException("Subscription name can't be null or empty", nameof(subscriptionName))
            : SubscriptionName(new SubscriptionName(subscriptionName));
    }

    /// <summary>
    /// Sets the subscription name using a <see cref="Paramore.Brighter.SubscriptionName"/> object.
    /// </summary>
    /// <param name="subscriptionName">Pre-constructed subscription name.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="subscriptionName"/> is null.</exception>
    public RmqSubscriptionBuilder SubscriptionName(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName ?? throw new ArgumentNullException(nameof(subscriptionName));
        return this;
    }

    private ChannelName? _channelName;

    /// <summary>
    /// Sets the queue name using a string.
    /// </summary>
    /// <param name="queueName">Name of the queue to bind to.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="queueName"/> is null or empty.</exception>
    public RmqSubscriptionBuilder QueueName(string queueName)
    {
        return ChannelName(queueName);
    }

    /// <summary>
    /// Sets the channel name using a string.
    /// </summary>
    /// <param name="channelName">Name of the channel to bind to.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="channelName"/> is <see langword="null"/> or empty.</exception>
    public RmqSubscriptionBuilder ChannelName(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            throw new ArgumentException("Channel name can't be null or empty", nameof(channelName));
        }
        
        return ChannelName(new ChannelName(channelName));
    }

    /// <summary>
    /// Sets the channel name using a <see cref="Paramore.Brighter.ChannelName"/> object.
    /// </summary>
    /// <param name="channelName">Pre-constructed channel name.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="channelName"/> is null.</exception>
    public RmqSubscriptionBuilder ChannelName(ChannelName channelName)
    {
        _channelName = channelName ?? throw new ArgumentNullException(nameof(channelName));
        return this;
    }

    private RoutingKey? _routingKey;

    /// <summary>
    /// Sets the topic name for routing messages.
    /// </summary>
    /// <param name="topicName">Topic name for message routing.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="topicName"/> is <see langword="null"/> or empty.</exception>
    public RmqSubscriptionBuilder TopicName(string topicName)
    {
        return RoutingKey(new RoutingKey(topicName));
    }

    /// <summary>
    /// Sets the routing key for message routing using a string.
    /// </summary>
    /// <param name="routingKey">Routing key for message routing.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="routingKey"/> is <see langword="null"/> or empty.</exception>
    public RmqSubscriptionBuilder RoutingKey(string routingKey)
    {
        if (string.IsNullOrEmpty(routingKey))
        {
            throw new ArgumentException("RoutingKey can't be null or empty", nameof(routingKey));
        }
        return RoutingKey(new RoutingKey(routingKey));
    }

    /// <summary>
    /// Sets the routing key for message routing using a <see cref="Paramore.Brighter.RoutingKey"/> object.
    /// </summary>
    /// <param name="routingKey">Pre-constructed routing key.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="routingKey"/> is <see langword="null"/>.</exception>
    public RmqSubscriptionBuilder RoutingKey(RoutingKey routingKey)
    {
        _routingKey = routingKey ?? throw new ArgumentNullException(nameof(routingKey));
        return this;
    }

    private int _bufferSize = 1;

    /// <summary>
    /// Sets the buffer size for message processing.
    /// </summary>
    /// <param name="bufferSize">Number of messages to buffer (default: 1).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="bufferSize"/> is less than 1.</exception>
    public RmqSubscriptionBuilder BufferSize(int bufferSize)
    {
        if (bufferSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(bufferSize), "Buffer size must be at least 1");
        }

        _bufferSize = bufferSize;
        return this;
    }

    private int _noOfPerformers = Environment.ProcessorCount;

    /// <summary>
    /// Sets the number of concurrent performers for message processing.
    /// </summary>
    /// <param name="concurrency">Number of concurrent workers (default: processor count).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="concurrency"/> is less than 1.</exception>
    public RmqSubscriptionBuilder Concurrency(int concurrency)
    {
        if (concurrency < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(concurrency), "Concurrency must be at least 1");
        }

        _noOfPerformers = concurrency;
        return this;
    }

    private int _timeoutInMilliseconds = 300;

    /// <summary>
    /// Sets the timeout for message processing in milliseconds.
    /// </summary>
    /// <param name="timeout">Time span representing the timeout duration.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="timeout"/> is negative.</exception>
    public RmqSubscriptionBuilder Timeout(TimeSpan timeout)
    {
        return TimeoutInMilliseconds(Convert.ToInt32(timeout.TotalMilliseconds));
    }

    /// <summary>
    /// Sets the timeout for message processing in milliseconds.
    /// </summary>
    /// <param name="timeoutInMilliseconds">Timeout in milliseconds (default: 300).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="timeoutInMilliseconds"/> is negative.</exception>
    public RmqSubscriptionBuilder TimeoutInMilliseconds(int timeoutInMilliseconds)
    {
        if (timeoutInMilliseconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timeoutInMilliseconds), "Timeout must be non-negative");
        }

        _timeoutInMilliseconds = timeoutInMilliseconds;
        return this;
    }

    private int _requeueCount = -1;

    /// <summary>
    /// Sets the number of times to requeue failed messages.
    /// </summary>
    /// <param name="requeueCount">Number of requeue attempts (-1 for unlimited).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="requeueCount"/> is less than -1.</exception>
    public RmqSubscriptionBuilder RequeueCount(int requeueCount)
    {
        if (requeueCount < -1)
        {
            throw new ArgumentOutOfRangeException(nameof(requeueCount), "Requeue count must be -1 or greater");
        }

        _requeueCount = requeueCount;
        return this;
    }

    private int _requeueDelayInMilliseconds;

    /// <summary>
    /// Sets the delay between message requeues using a time span.
    /// </summary>
    /// <param name="requeueDelay">Time span representing the delay between requeue attempts.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="requeueDelay"/> is negative.</exception>
    public RmqSubscriptionBuilder RequeueDelay(TimeSpan requeueDelay)
    {
        return RequeueDelayInMilliseconds(Convert.ToInt32(requeueDelay.TotalMilliseconds));
    }

    /// <summary>
    /// Sets the delay between message requeues in milliseconds.
    /// </summary>
    /// <param name="requeueDelayInMilliseconds">Delay in milliseconds (default: 0).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="requeueDelayInMilliseconds"/> is negative.</exception>
    public RmqSubscriptionBuilder RequeueDelayInMilliseconds(int requeueDelayInMilliseconds)
    {
        if (requeueDelayInMilliseconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(requeueDelayInMilliseconds), "Requeue delay must be non-negative");
        }

        _requeueDelayInMilliseconds = requeueDelayInMilliseconds;
        return this;
    }

    private int _unacceptableMessageLimit;

    /// <summary>
    /// Sets the limit for unacceptable messages before stopping the subscription.
    /// </summary>
    /// <param name="unacceptableMessageLimit">Number of failed messages allowed before shutdown.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="unacceptableMessageLimit"/> is negative.</exception>
    public RmqSubscriptionBuilder UnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        if (unacceptableMessageLimit < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unacceptableMessageLimit), "Limit must be non-negative");
        }

        _unacceptableMessageLimit = unacceptableMessageLimit;
        return this;
    }

    private bool _runAsync;

    /// <summary>
    /// Uses a reactor pattern for message processing (synchronous).
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder MessagePumpReactor()
    {
        return MessagePump(false);
    }

    /// <summary>
    /// Uses a proactor pattern for message processing (asynchronous).
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder MessagePumpProactor()
    {
        return MessagePump(true);
    }

    /// <summary>
    /// Sets the message pump implementation pattern.
    /// </summary>
    /// <param name="isProactor">True for asynchronous proactor, false for synchronous reactor.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder MessagePump(bool isProactor)
    {
        _runAsync = isProactor;
        return this;
    }

    private IAmAChannelFactory? _channelFactory;

    /// <summary>
    /// Sets a custom channel factory for the subscription.
    /// </summary>
    /// <param name="channelFactory">The channel factory to use.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="channelFactory"/> is null.</exception>
    public RmqSubscriptionBuilder ChannelFactory(IAmAChannelFactory channelFactory)
    {
        _channelFactory = channelFactory ?? throw new ArgumentNullException(nameof(channelFactory));
        return this;
    }

    private int _emptyChannelInMilliseconds = 500;

    /// <summary>
    /// Sets the delay between channel emptiness checks using a time span.
    /// </summary>
    /// <param name="emptyChannelDelay">Time span representing the delay between checks.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="emptyChannelDelay"/> is negative.</exception>
    public RmqSubscriptionBuilder EmptyDelay(TimeSpan emptyChannelDelay)
    {
        return EmptyChannelDelayInMilliseconds(Convert.ToInt32(emptyChannelDelay.TotalMilliseconds));
    }

    /// <summary>
    /// Sets the delay between channel emptiness checks in milliseconds.
    /// </summary>
    /// <param name="emptyChannelDelayInMilliseconds">Delay in milliseconds (default: 500).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="emptyChannelDelayInMilliseconds"/> is negative.</exception>
    public RmqSubscriptionBuilder EmptyChannelDelayInMilliseconds(int emptyChannelDelayInMilliseconds)
    {
        if (emptyChannelDelayInMilliseconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(emptyChannelDelayInMilliseconds), "Empty channel delay must be non-negative");
        }

        _emptyChannelInMilliseconds = emptyChannelDelayInMilliseconds;
        return this;
    }

    private int _channelFailureDelayInMilliseconds = 1_000;

    /// <summary>
    /// Sets the delay between channel failure recovery attempts using a time span.
    /// </summary>
    /// <param name="failureDelay">Time span representing the delay between recovery attempts.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="failureDelay"/> is negative.</exception>
    public RmqSubscriptionBuilder FailureDelay(TimeSpan failureDelay)
    {
        return FailureDelayInMilliseconds(Convert.ToInt32(failureDelay.TotalMilliseconds));
    }

    /// <summary>
    /// Sets the delay between channel failure recovery attempts in milliseconds.
    /// </summary>
    /// <param name="failureDelayInMilliseconds">Delay in milliseconds (default: 1000).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="failureDelayInMilliseconds"/> is negative.</exception>
    public RmqSubscriptionBuilder FailureDelayInMilliseconds(int failureDelayInMilliseconds)
    {
        if (failureDelayInMilliseconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(failureDelayInMilliseconds), "Failure delay must be non-negative");
        }

        _channelFailureDelayInMilliseconds = failureDelayInMilliseconds;
        return this;
    }

    private OnMissingChannel _makeChannel = OnMissingChannel.Create;

    /// <summary>
    /// Configures the subscription to create or override topics/queues if they don't exist.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder CreateOrOverrideTopicOrQueueIfMissing()
    {
        return MakeTopicOrQueue(OnMissingChannel.Create);
    }

    /// <summary>
    /// Validates that topics and queues exist before starting consumption.
    /// Throws an exception if they are missing.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder ValidateIfTopicAndQueueExists()
    {
        return MakeTopicOrQueue(OnMissingChannel.Validate);
    }

    /// <summary>
    /// Assumes topics and queues exist without validation.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder AssumeTopicAndQueueExists()
    {
        return MakeTopicOrQueue(OnMissingChannel.Assume);
    }

    /// <summary>
    /// Sets the behavior when topics or queues are missing.
    /// </summary>
    /// <param name="makeChannel">The <see cref="OnMissingChannel"/> mode to use.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqSubscriptionBuilder MakeTopicOrQueue(OnMissingChannel makeChannel)
    {
        _makeChannel = makeChannel;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured <see cref="RmqSubscription"/> instance.
    /// </summary>
    /// <returns>A new <see cref="RmqSubscription"/> with the specified configuration.</returns>
    internal RmqSubscription Build()
    {
        return new RmqSubscription(
                deadLetterChannelName: _deadLetterName,
                deadLetterRoutingKey: _deadLetterRoutingKey,
                highAvailability: _highAvailability,
                isDurable: _durable,
                ttl: _ttl,
                dataType: _dataType,
                name: _subscriptionName,
                channelName: _channelName,
                routingKey: _routingKey,
                bufferSize: _bufferSize,
                noOfPerformers: _noOfPerformers,
                timeoutInMilliseconds: _timeoutInMilliseconds,
                requeueCount: _requeueCount,
                requeueDelayInMilliseconds: _requeueDelayInMilliseconds,
                unacceptableMessageLimit: _unacceptableMessageLimit,
                runAsync: _runAsync,
                channelFactory: _channelFactory,
                makeChannels: _makeChannel,
                emptyChannelDelay: _emptyChannelInMilliseconds,
                channelFailureDelay: _channelFailureDelayInMilliseconds);
    }
}