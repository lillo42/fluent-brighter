
using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

public class SqsSubscriptionBuilder<T>
    where T : class, IRequest
{
    private SubscriptionName? _subscriptionName;

    public SqsSubscriptionBuilder<T> SubscriptionName(string subscriptionName)
    {
        _subscriptionName = new SubscriptionName(subscriptionName);
        return this;
    }

    public SqsSubscriptionBuilder<T> SubscriptionName(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }


    private ChannelName _channelName = new(typeof(T).FullName.ToValidSNSTopicName());
    public SqsSubscriptionBuilder<T> QueueName(string queueName)
    {
        return ChannelName(new ChannelName(queueName));
    }

    public SqsSubscriptionBuilder<T> ChannelName(string channelName)
    {
        return ChannelName(new ChannelName(channelName));
    }

    public SqsSubscriptionBuilder<T> ChannelName(ChannelName channelName)
    {
        _channelName = channelName;
        return this;
    }

    private RoutingKey _routingKey = new(typeof(T).FullName.ToValidSNSTopicName());
    public SqsSubscriptionBuilder<T> TopicName(string topicName)
    {
        return RoutingKey(new RoutingKey(topicName))
            .FindTopicByName();
    }

    public SqsSubscriptionBuilder<T> TopicArn(string topicArn)
    {
        return RoutingKey(new RoutingKey(topicArn))
            .FindTopicByArn();
    }

    public SqsSubscriptionBuilder<T> RoutingKey(string routingKey)
    {
        return RoutingKey(new RoutingKey(routingKey));
    }

    public SqsSubscriptionBuilder<T> RoutingKey(RoutingKey routingKey)
    {
        _routingKey = routingKey;
        return this;
    }

    private int _bufferSize = 1;
    public SqsSubscriptionBuilder<T> BufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }

    private int _noOfPerformers = Environment.ProcessorCount;
    public SqsSubscriptionBuilder<T> Concurrency(int concurrency)
    {
        _noOfPerformers = concurrency;
        return this;
    }

    private int _timeout = 300;
    public SqsSubscriptionBuilder<T> Timeout(TimeSpan timeout)
    {
        return Timeout(Convert.ToInt32(timeout.TotalMilliseconds));
    }

    public SqsSubscriptionBuilder<T> Timeout(int timeout)
    {
        _timeout = timeout;
        return this;
    }

    private int _pollDelayInMs = -1;
    public SqsSubscriptionBuilder<T> PollDelay(TimeSpan pollDelay)
    {
        return PollDelayInMs(Convert.ToInt32(pollDelay.TotalMilliseconds));
    }

    public SqsSubscriptionBuilder<T> PollDelayInMs(int pollDelayInMs)
    {
        _pollDelayInMs = pollDelayInMs;
        return this;
    }

    private int _noWorkPauseInMs = 500;
    public SqsSubscriptionBuilder<T> NoWorkPause(TimeSpan noWorkPause)
    {
        return NoWorkPauseInMs(Convert.ToInt32(noWorkPause.TotalMilliseconds));
    }

    public SqsSubscriptionBuilder<T> NoWorkPauseInMs(int noWorkPauseInMs)
    {
        _noWorkPauseInMs = noWorkPauseInMs;
        return this;
    }

    private int _requeueCount = -1;
    public SqsSubscriptionBuilder<T> RequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }

    private int _requeueDelayInMs = 0;
    public SqsSubscriptionBuilder<T> RequeueDelay(TimeSpan requeueDelay)
    {
        return RequeueDelayInMs(Convert.ToInt32(requeueDelay.TotalMilliseconds));
    }

    public SqsSubscriptionBuilder<T> RequeueDelayInMs(int requeueDelayInMs)
    {
        _requeueDelayInMs = requeueDelayInMs;
        return this;
    }

    private int _unacceptableMessageLimit = 0;
    public SqsSubscriptionBuilder<T> UnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit;
        return this;
    }

    private bool _runAsync;
    public SqsSubscriptionBuilder<T> Reactor()
    {
        return RunAsync(false);
    }

    public SqsSubscriptionBuilder<T> Proactor()
    {
        return RunAsync(true);
    }

    public SqsSubscriptionBuilder<T> RunAsync(bool runAsync)
    {
        _runAsync = runAsync;
        return this;
    }

    private IAmAChannelFactory? _channelFactory;
    public SqsSubscriptionBuilder<T> ChannelFactory(IAmAChannelFactory channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }

    private int _lockTimeout = 10;
    public SqsSubscriptionBuilder<T> LockTimeout(int lockTimeout)
    {
        _lockTimeout = lockTimeout;
        return this;
    }

    private int _delayInSeconds;
    public SqsSubscriptionBuilder<T> DelayInSeconds(int delay)
    {
        _delayInSeconds = delay;
        return this;
    }

    private int _messageRetentionPeriod = 345600;
    public SqsSubscriptionBuilder<T> MessageRetentionPeriod(int messageRetentionPeriod)
    {
        _messageRetentionPeriod = messageRetentionPeriod;
        return this;
    }

    private TopicFindBy _findTopicBy = TopicFindBy.Name;
    public SqsSubscriptionBuilder<T> FindTopicByName()
    {
        _findTopicBy = TopicFindBy.Name;
        return this;
    }

    public SqsSubscriptionBuilder<T> FindTopicByArn()
    {
        _findTopicBy = TopicFindBy.Arn;
        return this;
    }

    public SqsSubscriptionBuilder<T> FindTopicByConvention()
    {
        _findTopicBy = TopicFindBy.Convention;
        return this;
    }

    public SqsSubscriptionBuilder<T> FindTopicBy(TopicFindBy findBy)
    {
        _findTopicBy = findBy;
        return this;
    }

    private string? _iAmPolicy;
    public SqsSubscriptionBuilder<T> IAmPolicy(string iAmPolicy)
    {
        _iAmPolicy = iAmPolicy;
        return this;
    }

    private RedrivePolicy? _redrivePolicy;
    public SqsSubscriptionBuilder<T> RedrivePolicy(Action<RedrivePolicyBuilder> configure)
    {
        var builder = new RedrivePolicyBuilder();
        configure(builder);
        _redrivePolicy = builder.Build();
        return this;
    }

    public SqsSubscriptionBuilder<T> RedrivePolicy(RedrivePolicy redrivePolicy)
    {
        _redrivePolicy = redrivePolicy;
        return this;
    }

    private SnsAttributes? _snsAttributes;
    public SqsSubscriptionBuilder<T> SnsAttributes(Action<SnsAttributesBuilder> configure)
    {
        var builder = new SnsAttributesBuilder();
        configure(builder);
        _snsAttributes = builder.Build();
        return this;
    }

    public SqsSubscriptionBuilder<T> SnsAttributes(SnsAttributes attributes)
    {
        _snsAttributes = attributes;
        return this;
    }

    private Dictionary<string, string>? _tags;
    public SqsSubscriptionBuilder<T> Tags(string key, string value)
    {
        _tags ??= [];
        _tags[key] = value;
        return this;
    }

    public SqsSubscriptionBuilder<T> Tags(IEnumerable<KeyValuePair<string, string>> tags)
    {
        _tags ??= [];
        foreach (var item in tags)
        {
            _tags[item.Key] = item.Value;
        }

        return this;
    }

    private OnMissingChannel _makeChannel = OnMissingChannel.Create;
    public SqsSubscriptionBuilder<T> CreateOrOverrideIfSnsOrSqsIsMissing()
    {
        _makeChannel = OnMissingChannel.Create;
        return this;
    }

    public SqsSubscriptionBuilder<T> ValidateSnsAndSqsExists()
    {
        _makeChannel = OnMissingChannel.Validate;
        return this;
    }

    public SqsSubscriptionBuilder<T> AssumeSnsAndSqsExists()
    {
        _makeChannel = OnMissingChannel.Assume;
        return this;
    }

    public SqsSubscriptionBuilder<T> MakeChannels(OnMissingChannel missingChannel)
    {
        _makeChannel = missingChannel;
        return this;
    }

    private bool _rawMessageDelivery = true;

    public SqsSubscriptionBuilder<T> EnableRawMessageDelivery()
    {
        _rawMessageDelivery = true;
        return this;
    }

    public SqsSubscriptionBuilder<T> DisableRawMessageDelivery()
    {
        _rawMessageDelivery = false;
        return this;
    }

    public SqsSubscriptionBuilder<T> RawMessageDelivery(bool rawMessageDelivery)
    {
        _rawMessageDelivery = rawMessageDelivery;
        return this;
    }

    private int _emptyChannelDelay = 500;
    public SqsSubscriptionBuilder<T> EmptyDelay(TimeSpan emptyChannelDelay)
    {
        return EmptyDelayInMs(Convert.ToInt32(emptyChannelDelay.TotalMilliseconds));
    }

    public SqsSubscriptionBuilder<T> EmptyDelayInMs(int emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }

    private int _channelFailureDelay = 1_000;
    public SqsSubscriptionBuilder<T> FailureDelay(TimeSpan failureDelay)
    {
        return FailureDelayInMs(Convert.ToInt32(failureDelay.TotalMilliseconds));
    }

    public SqsSubscriptionBuilder<T> FailureDelayInMs(int failureDelay)
    {
        _channelFailureDelay = failureDelay;
        return this;
    }

    internal SqsSubscription<T> Build()
    {
        return new SqsSubscription<T>(
               name: _subscriptionName,
               channelName: _channelName,
               routingKey: _routingKey,
               bufferSize: _bufferSize,
               noOfPerformers: _noOfPerformers,
               timeoutInMs: _timeout,
               pollDelayInMs: _pollDelayInMs,
               noWorkPauseInMs: _noWorkPauseInMs,
               requeueCount: _requeueCount,
               requeueDelayInMs: _requeueDelayInMs,
               unacceptableMessageLimit: _unacceptableMessageLimit,
               runAsync: _runAsync,
               channelFactory: _channelFactory,
               lockTimeout: _lockTimeout,
               delaySeconds: _delayInSeconds,
               messageRetentionPeriod: _messageRetentionPeriod,
               findTopicBy: _findTopicBy,
               iAmPolicy: _iAmPolicy,
               redrivePolicy: _redrivePolicy,
               snsAttributes: _snsAttributes,
               tags: _tags,
               makeChannels: _makeChannel,
               rawMessageDelivery: _rawMessageDelivery,
               emptyChannelDelay: _emptyChannelDelay,
               channelFailureDelay: _channelFailureDelay);
    }
}