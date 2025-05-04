
using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ;

namespace Fluent.Brighter.RMQ;

public class RmqSubscriptionBuilder
{
    private ChannelName? _deadLetterName;
    public RmqSubscriptionBuilder DeadLetter(string deadLetterName)
    {
        return DeadLetter(new ChannelName(deadLetterName));
    }

    public RmqSubscriptionBuilder DeadLetter(ChannelName deadLetterName)
    {
        _deadLetterName = deadLetterName;
        return string.IsNullOrEmpty(_deadLetterRoutingKey) ? DeadLetterRoutingKey(deadLetterName.Value) : this;
    }

    private string? _deadLetterRoutingKey;
    public RmqSubscriptionBuilder DeadLetterRoutingKey(string routingKey)
    {
        _deadLetterRoutingKey = routingKey;
        return this;
    }

    private bool _highAvailability;
    public RmqSubscriptionBuilder EnableHighAvailability()
    {
        return HighAvailability(true);
    }

    public RmqSubscriptionBuilder DisableHighAvailability()
    {
        return HighAvailability(false);
    }

    public RmqSubscriptionBuilder HighAvailability(bool highAvailability)
    {
        _highAvailability = highAvailability;
        return this;
    }

    private bool _durable;
    public RmqSubscriptionBuilder EnableDurable()
    {
        return Durable(true);
    }

    public RmqSubscriptionBuilder DisableDurable()
    {
        return Durable(false);
    }

    public RmqSubscriptionBuilder Durable(bool durable)
    {
        _durable = durable;
        return this;
    }

    private int? _maxQueueLenght;
    public RmqSubscriptionBuilder MaxQueueLenght(int? maxQueueLenght)
    {
        _maxQueueLenght = maxQueueLenght;
        return this;
    }

    private int? _ttl;
    public RmqSubscriptionBuilder TTL(int? ttl)
    {
        _ttl = ttl;
        return this;
    }

    private Type? _dataType;
    public RmqSubscriptionBuilder MessageType<T>()
        where T : class, IRequest
    {
        return MessageType(typeof(T));
    }

    public RmqSubscriptionBuilder MessageType(Type dataType)
    {
        _dataType = dataType;
        if (_channelName == null)
        {
            ChannelName(dataType.FullName!);
        }

        if (_routingKey == null)
        {
            TopicName(dataType.FullName!);
        }

        return this;
    }

    private SubscriptionName? _subscriptionName;
    public RmqSubscriptionBuilder SubscriptionName(string subscriptionName)
    {
        return SubscriptionName(new SubscriptionName(subscriptionName));
    }

    public RmqSubscriptionBuilder SubscriptionName(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }

    private ChannelName? _channelName;
    public RmqSubscriptionBuilder QueueName(string queueName)
    {
        return ChannelName(new ChannelName(queueName));
    }

    public RmqSubscriptionBuilder ChannelName(string channelName)
    {
        return ChannelName(new ChannelName(channelName));
    }

    public RmqSubscriptionBuilder ChannelName(ChannelName channelName)
    {
        _channelName = channelName;
        return this;
    }

    private RoutingKey? _routingKey;
    public RmqSubscriptionBuilder TopicName(string topicName)
    {
        return RoutingKey(new RoutingKey(topicName));
    }

    public RmqSubscriptionBuilder RoutingKey(string routingKey)
    {
        return RoutingKey(new RoutingKey(routingKey));
    }

    public RmqSubscriptionBuilder RoutingKey(RoutingKey routingKey)
    {
        _routingKey = routingKey;
        return this;
    }

    private int _bufferSize = 1;
    public RmqSubscriptionBuilder BufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }

    private int _noOfPerformers = Environment.ProcessorCount;
    public RmqSubscriptionBuilder Concurrency(int concurrency)
    {
        _noOfPerformers = concurrency;
        return this;
    }

    private int _timeoutInMilliseconds = 300;
    public RmqSubscriptionBuilder Timeout(TimeSpan timeout)
    {
        return Timeout(Convert.ToInt32(timeout.TotalMilliseconds));
    }

    public RmqSubscriptionBuilder Timeout(int timeout)
    {
        _timeoutInMilliseconds = timeout;
        return this;
    }

    private int _requeueCount = -1;
    public RmqSubscriptionBuilder RequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }

    private int _requeueDelayInMilliseconds = 0;
    public RmqSubscriptionBuilder RequeueDelay(TimeSpan requeueDelay)
    {
        return RequeueDelayInMilliseconds(Convert.ToInt32(requeueDelay.TotalMilliseconds));
    }

    public RmqSubscriptionBuilder RequeueDelayInMilliseconds(int requeueDelayInMs)
    {
        _requeueDelayInMilliseconds = requeueDelayInMs;
        return this;
    }

    private int _unacceptableMessageLimit = 0;
    public RmqSubscriptionBuilder UnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit;
        return this;
    }

    private bool _runAsync;
    public RmqSubscriptionBuilder MessagePumpReactor()
    {
        return MessagePump(false);
    }

    public RmqSubscriptionBuilder MessagePumpProactor()
    {
        return MessagePump(true);
    }

    public RmqSubscriptionBuilder MessagePump(bool isProactor)
    {
        _runAsync = isProactor;
        return this;
    }

    private IAmAChannelFactory? _channelFactory;
    public RmqSubscriptionBuilder ChannelFactory(IAmAChannelFactory channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }

    private int _emptyChannelInMilliseconds = 500;
    public RmqSubscriptionBuilder EmptyDelay(TimeSpan emptyChannelDelay)
    {
        return EmptyChannelDelayInMilliseconds(Convert.ToInt32(emptyChannelDelay.TotalMilliseconds));
    }

    public RmqSubscriptionBuilder EmptyChannelDelayInMilliseconds(int emptyChannelDelayInMilliseconds)
    {
        _emptyChannelInMilliseconds = emptyChannelDelayInMilliseconds;
        return this;
    }

    private int _channelFailureDelayInMilliseconds = 1_000;
    public RmqSubscriptionBuilder FailureDelay(TimeSpan failureDelay)
    {
        return FailureDelayInMilliseconds(Convert.ToInt32(failureDelay.TotalMilliseconds));
    }

    public RmqSubscriptionBuilder FailureDelayInMilliseconds(int failureDelayInMilliseconds)
    {
        _channelFailureDelayInMilliseconds = failureDelayInMilliseconds;
        return this;
    }

    private OnMissingChannel _makeChannel = OnMissingChannel.Create;
    public RmqSubscriptionBuilder CreateOrOverrideTopicOrQueueIfMissing()
    {
        return MakeTopicOrQueue(OnMissingChannel.Create);
    }

    public RmqSubscriptionBuilder ValidateIfTopicAndQueueExists()
    {
        return MakeTopicOrQueue(OnMissingChannel.Validate);
    }

    public RmqSubscriptionBuilder AssumeTopicAndQueueExists()
    {
        return MakeTopicOrQueue(OnMissingChannel.Assume);
    }

    public RmqSubscriptionBuilder MakeTopicOrQueue(OnMissingChannel makeChannel)
    {
        _makeChannel = makeChannel;
        return this;
    }

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