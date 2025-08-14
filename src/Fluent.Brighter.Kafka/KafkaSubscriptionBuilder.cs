using System;

using Confluent.Kafka;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter.Kafka;

public sealed class KafkaSubscriptionBuilder
{
    private SubscriptionName? _subscriptionName;

    public KafkaSubscriptionBuilder SetSubscription(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }
    
    private ChannelName? _channelName;
    private RoutingKey? _routingKey;
    public KafkaSubscriptionBuilder SetTopic(RoutingKey routingKey)
    {
        _routingKey = routingKey;
        _channelName = new ChannelName(routingKey.Value);
        return this;
    }
    
    private Type? _dataType;

    public KafkaSubscriptionBuilder SetDataType(Type? dataType)
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

    public KafkaSubscriptionBuilder SetGetRequestType(Func<Message, Type>? getRequestType)
    {
        _getRequestType = getRequestType;
        return this;
    }

    private int _bufferSize = 1;

    public KafkaSubscriptionBuilder SetBufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }
    
    private int _noOfPerformers  = 1;

    public KafkaSubscriptionBuilder SetNumberOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }
    
    private TimeSpan? _timeOut;
    public KafkaSubscriptionBuilder SetTimeout(TimeSpan? timeout)
    {
        _timeOut = timeout;
        return this;
    }
    
    private int _requeueCount = -1;
    public KafkaSubscriptionBuilder SetRequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }
    
    private TimeSpan? _requeueDelay;
    public KafkaSubscriptionBuilder SetRequeueDelay(TimeSpan? timeout)
    {
        _requeueDelay = timeout;
        return this;
    }
    
    private int _unacceptableMessageLimit;
    public KafkaSubscriptionBuilder SetUnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit ;
        return this;
    }
    
    private MessagePumpType _messagePumpType = MessagePumpType.Proactor;
    public KafkaSubscriptionBuilder SetMessagePumpType(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }

    private IAmAChannelFactory? _channelFactory;

    public KafkaSubscriptionBuilder SetChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }
    
    private OnMissingChannel _onMissingChannel = OnMissingChannel.Create;
    public KafkaSubscriptionBuilder SetMakeChannels(OnMissingChannel onMissingChannel)
    {
        _onMissingChannel = onMissingChannel;
        return this;
    }

    private TimeSpan? _emptyChannelDelay;
    public KafkaSubscriptionBuilder SetEmptyChannelDelay(TimeSpan? emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }

    private TimeSpan? _channelFailureDelay;
    public KafkaSubscriptionBuilder SetChannelFailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }

    private long _commitBatchSize = 10;
    
    public KafkaSubscriptionBuilder SetCommitBatchSize(long commitBatchSize)
    {
        _commitBatchSize = commitBatchSize;
        return this;
    }

    private Action<ConsumerConfig>? _configHook = null;
    public KafkaSubscriptionBuilder SetConfigHook(Action<ConsumerConfig> configHook)
    {
        _configHook = configHook;
        return this;
    }
    
    private string? _groupId = null;
    
    public KafkaSubscriptionBuilder SetGroupId(string groupId)
    {
        _groupId = groupId;
        return this;
    }
    
    private IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;
    public KafkaSubscriptionBuilder SetIsolationLevel(IsolationLevel isolationLevel)
    {
        _isolationLevel = isolationLevel;
        return this;
    }
    
    private TimeSpan _maxPollInterval = TimeSpan.FromMilliseconds(300000);
    public KafkaSubscriptionBuilder SetMaxPollInterval(TimeSpan maxPollInterval)
    {
        _maxPollInterval = maxPollInterval;
        return this;
    }
    
    private int _numPartitions = 1;
    public KafkaSubscriptionBuilder SetNumPartitions(int numPartitions)
    {
        _numPartitions = numPartitions;
        return this;
    }

    
    private AutoOffsetReset _offsetDefault = AutoOffsetReset.Earliest;

    public KafkaSubscriptionBuilder SetOffsetDefault(AutoOffsetReset offsetDefault)
    {
        _offsetDefault = offsetDefault;
        return this;
    }


    private PartitionAssignmentStrategy _partitionAssignmentStrategy = PartitionAssignmentStrategy.RoundRobin; // Default matches KafkaSubscription constructor

    public KafkaSubscriptionBuilder SetPartitionAssignmentStrategy(PartitionAssignmentStrategy partitionAssignmentStrategy)
    {
        _partitionAssignmentStrategy = partitionAssignmentStrategy;
        return this;
    }

    
    private TimeSpan _readCommittedOffsetsTimeOut = TimeSpan.FromMilliseconds(5000);

    public KafkaSubscriptionBuilder SetReadCommittedOffsetsTimeOut(TimeSpan readCommittedOffsetsTimeOut)
    {
        _readCommittedOffsetsTimeOut = readCommittedOffsetsTimeOut;
        return this;
    }

    
    private short _replicationFactor = 1;

    public KafkaSubscriptionBuilder SetReplicationFactor(short replicationFactor)
    {
        _replicationFactor = replicationFactor;
        return this;
    }


    private TimeSpan _sessionTimeout = TimeSpan.FromMilliseconds(10000);

    public KafkaSubscriptionBuilder SetSessionTimeout(TimeSpan sessionTimeout)
    {
        _sessionTimeout = sessionTimeout;
        return this;
    }


    private TimeSpan _sweepUncommittedOffsetsInterval = TimeSpan.FromMilliseconds(30000);

    public KafkaSubscriptionBuilder SetSweepUncommittedOffsetsInterval(TimeSpan sweepUncommittedOffsetsInterval)
    {
        _sweepUncommittedOffsetsInterval = sweepUncommittedOffsetsInterval;
        return this;
    }

    
    private TimeSpan _topicFindTimeout = TimeSpan.FromMilliseconds(5000);

    public KafkaSubscriptionBuilder SetTopicFindTimeout(TimeSpan topicFindTimeout)
    {
        _topicFindTimeout = topicFindTimeout;
        return this;
    }
    
    internal KafkaSubscription Build()
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
        
        return new KafkaSubscription(
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
            commitBatchSize: _commitBatchSize,
            configHook: _configHook,
            groupId: _groupId,
            isolationLevel: _isolationLevel,
            maxPollInterval: _maxPollInterval,
            numOfPartitions: _numPartitions,
            offsetDefault: _offsetDefault,
            partitionAssignmentStrategy: _partitionAssignmentStrategy,
            replicationFactor: _replicationFactor,
            sessionTimeout: _sessionTimeout,
            sweepUncommittedOffsetsInterval: _sweepUncommittedOffsetsInterval)
        {
            ReadCommittedOffsetsTimeOut = _readCommittedOffsetsTimeOut,
            TopicFindTimeout = _topicFindTimeout
        };
    }
}