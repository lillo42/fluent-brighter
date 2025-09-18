using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4;

public sealed class SqsSubscriptionBuilder
{
    private SubscriptionName? _subscriptionName;
    public SqsSubscriptionBuilder SetSubscriptionName(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }

    private ChannelName? _channelName;
    public SqsSubscriptionBuilder SetQueue(ChannelName channelName)
    {
        _channelName = channelName;
        if (RoutingKey.IsNullOrEmpty(_routingKey))
        {
            _routingKey = new RoutingKey(channelName.Value);
        }

        _channelType ??= ChannelType.PointToPoint;
        return this;
    }
    
    private RoutingKey? _routingKey;
    public SqsSubscriptionBuilder SetTopic(RoutingKey routingKey)
    {
        _routingKey = routingKey;
        _channelType = ChannelType.PubSub;
        return this;
    }
    
    private ChannelType? _channelType;
    public SqsSubscriptionBuilder SetChannelType(ChannelType channelType)
    {
        _channelType = channelType;
        return this;
    }


    private Type? _requestType;
    public SqsSubscriptionBuilder SetDataType(Type? requestType)
    {
        _requestType = requestType;
        if (requestType == null)
        {
            return this;
        }
        
        if (_subscriptionName == null)
        {
            _subscriptionName = new SubscriptionName(requestType.Name);
        }
        
        if (ChannelName.IsNullOrEmpty(_channelName))
        {
            _channelName = new ChannelName(requestType.Name);
        }

        if (RoutingKey.IsNullOrEmpty(_routingKey))
        {
            _routingKey = new RoutingKey(requestType.Name);
        }
        
        return this;
    }

    private Func<Message, Type>? _getRequestType;
    public SqsSubscriptionBuilder SetGetRequestType(Func<Message, Type>? getRequestType)
    {
        _getRequestType = getRequestType;
        return this;
    }

    private int _bufferSize = 1;
    
    public SqsSubscriptionBuilder SetBufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }

    private int _noOfPerformers = 1;

    public SqsSubscriptionBuilder SetNoOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }

    private TimeSpan? _timeOut;
    
    public SqsSubscriptionBuilder SetTimeOut(TimeSpan? timeOut)
    {
        _timeOut = timeOut;
        return this;
    }

    private int _requeueCount = -1;
    
    public SqsSubscriptionBuilder SetRequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }

    private TimeSpan? _requeueDelay;
    
    public SqsSubscriptionBuilder SetRequeueDelay(TimeSpan? requeueDelay)
    {
        _requeueDelay = requeueDelay;
        return this;
    }

    private int _unacceptableMessageLimit;
    
    public SqsSubscriptionBuilder SetUnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit;
        return this;
    }

    private MessagePumpType _messagePumpType = MessagePumpType.Proactor;
    
    public SqsSubscriptionBuilder SetMessagePumpType(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }

    private IAmAChannelFactory? _channelFactory;
    
    public SqsSubscriptionBuilder SetChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }

    private TimeSpan? _emptyChannelDelay;
    
    public SqsSubscriptionBuilder SetEmptyChannelDelay(TimeSpan? emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }

    private TimeSpan? _channelFailureDelay;
    
    public SqsSubscriptionBuilder SetChannelFailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }

    private TopicFindBy _findTopicBy = TopicFindBy.Convention;

    public SqsSubscriptionBuilder SetFindTopicBy(TopicFindBy findTopicBy)
    {
        _findTopicBy = findTopicBy;
        return this;
    }

    private QueueFindBy _findQueueBy = QueueFindBy.Name;
    
    public SqsSubscriptionBuilder SetFindQueueBy(QueueFindBy findQueueBy)
    {
        _findQueueBy = findQueueBy;
        return this;
    }

    private SqsAttributes? _queueAttributes;
    
    public SqsSubscriptionBuilder SetQueueAttributes(SqsAttributes? queueAttributes)
    {
        _queueAttributes = queueAttributes;
        return this;
    }

    private SnsAttributes? _topicAttributes;
    
    public SqsSubscriptionBuilder SetTopicAttributes(SnsAttributes? topicAttributes)
    {
        _topicAttributes = topicAttributes;
        return this;
    }

    private OnMissingChannel _makeChannels = OnMissingChannel.Create;
    public SqsSubscriptionBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }

    internal SqsSubscription Build()
    {
        if (_subscriptionName == null)
        {
            throw new ConfigurationException("Subscription name not set");
        }

        if (_channelName == null)
        {
            throw new ConfigurationException("Channel name not set");
        }

        if (_routingKey == null)
        {
            throw new ConfigurationException("Routing key not set");
        }
        
        return new SqsSubscription(
            subscriptionName:_subscriptionName,
            channelName: _channelName,
            channelType: _channelType.GetValueOrDefault(),
            routingKey:_routingKey,
            requestType: _requestType,
            getRequestType: _getRequestType,
            bufferSize: _bufferSize,
            noOfPerformers: _noOfPerformers,
            timeOut: _timeOut,
            requeueCount: _requeueCount,
            requeueDelay: _requeueDelay,
            unacceptableMessageLimit: _unacceptableMessageLimit,
            messagePumpType: _messagePumpType,
            channelFactory: _channelFactory,
            emptyChannelDelay: _emptyChannelDelay,
            channelFailureDelay: _channelFailureDelay,
            findTopicBy: _findTopicBy,
            findQueueBy: _findQueueBy,
            queueAttributes: _queueAttributes,
            topicAttributes: _topicAttributes,
            makeChannels: _makeChannels
        );
    }
}