using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Async;

namespace Fluent.Brighter.RMQ.Async;

public sealed class RmqSubscriptionBuilder
{
    private SubscriptionName? _subscriptionName;

    public RmqSubscriptionBuilder SetSubscription(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }
    
    private ChannelName? _channelName;
    public RmqSubscriptionBuilder SetQueue(ChannelName channelName)
    {
        _channelName = channelName;
        return this;
    }
    
    private RoutingKey? _routingKey;
    public RmqSubscriptionBuilder SetTopic(RoutingKey routingKey)
    {
        _routingKey = routingKey;
        return this;
    }
    
    private Type? _dataType;

    public RmqSubscriptionBuilder SetDataType(Type? dataType)
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

    public RmqSubscriptionBuilder SetGetRequestType(Func<Message, Type>? getRequestType)
    {
        _getRequestType = getRequestType;
        return this;
    }

    private int _bufferSize = 1;

    public RmqSubscriptionBuilder SetBufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }
    
    private int _noOfPerformers  = 1;

    public RmqSubscriptionBuilder SetNumberOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }
    
    private TimeSpan? _timeOut;
    public RmqSubscriptionBuilder SetTimeout(TimeSpan? timeout)
    {
        _timeOut = timeout;
        return this;
    }
    
    private int _requeueCount = -1;
    public RmqSubscriptionBuilder SetRequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }
    
    private TimeSpan? _requeueDelay;
    public RmqSubscriptionBuilder SetRequeueDelay(TimeSpan? timeout)
    {
        _requeueDelay = timeout;
        return this;
    }
    
    private int _unacceptableMessageLimit;
    public RmqSubscriptionBuilder SetUnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit ;
        return this;
    }
    
    private MessagePumpType _messagePumpType = MessagePumpType.Proactor;
    public RmqSubscriptionBuilder SetMessagePumpType(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }

    private IAmAChannelFactory? _channelFactory;

    public RmqSubscriptionBuilder SetChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }
    
    private OnMissingChannel _onMissingChannel = OnMissingChannel.Create;
    public RmqSubscriptionBuilder SetMakeChannels(OnMissingChannel onMissingChannel)
    {
        _onMissingChannel = onMissingChannel;
        return this;
    }

    private TimeSpan? _emptyChannelDelay;
    public RmqSubscriptionBuilder SetEmptyChannelDelay(TimeSpan? emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }

    private TimeSpan? _channelFailureDelay;
    public RmqSubscriptionBuilder SetChannelFailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }

    private int? _maxQueueLenght;

    public RmqSubscriptionBuilder SetMaxQueueLenght(int? maxQueueLenght)
    {
        _maxQueueLenght = maxQueueLenght;
        return this;
    }

    private QueueType _queueType = QueueType.Classic;

    public RmqSubscriptionBuilder SetQueueType(QueueType type)
    {
        _queueType = type;
        return this;
    }

    private bool _isDurable;

    public RmqSubscriptionBuilder SetIsDurable(bool isDurable)
    {
        _isDurable = isDurable;
        return this;
    }
    
    private bool _highAvailability;

    public RmqSubscriptionBuilder SethHighAvailability(bool highAvailability)
    {
        _highAvailability = highAvailability;
        return this;
    }

    private ChannelName? _deadLetterChannelName;

    public RmqSubscriptionBuilder SetDeadLetterChannel(ChannelName? deadLetterChannelName)
    {
        _deadLetterChannelName = deadLetterChannelName;
        return this;
    }
    
    private RoutingKey? _deadLetterRoutingKey;
    public RmqSubscriptionBuilder SetDeadLetterRoutingKey(RoutingKey? deadLetterRoutingKey)
    {
        _deadLetterRoutingKey = deadLetterRoutingKey;
        return this;
    }
    
    private TimeSpan? _ttl;

    public RmqSubscriptionBuilder SetTTL(TimeSpan? ttl)
    {
        _ttl = ttl;
        return this;
    }
    
    internal RmqSubscription Build()
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
            queueType: _queueType,
            isDurable: _isDurable,
            highAvailability: _highAvailability,
            deadLetterChannelName: _deadLetterChannelName,
            deadLetterRoutingKey: _deadLetterRoutingKey,
            ttl: _ttl);
    }
}