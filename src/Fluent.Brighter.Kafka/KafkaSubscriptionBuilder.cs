using System;

using Confluent.Kafka;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter.Kafka;

/// <summary>
/// Provides a fluent builder for configuring Kafka subscription settings
/// in a Brighter integration with Apache Kafka, supporting consumer tuning,
/// error handling, and CloudEvents deserialization.
/// </summary>
public sealed class KafkaSubscriptionBuilder
{
    private SubscriptionName? _subscriptionName;

    /// <summary>
    /// Sets the logical name of the subscription, used to identify the consumer group or handler.
    /// </summary>
    /// <param name="subscriptionName">The subscription name.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetSubscription(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }
    
    private ChannelName? _channelName;
    private RoutingKey? _routingKey;

    /// <summary>
    /// Specifies the Kafka topic (as a routing key) to subscribe to.
    /// The channel name is automatically derived from the topic name.
    /// </summary>
    /// <param name="routingKey">The Kafka topic name wrapped as a routing key.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetTopic(RoutingKey routingKey)
    {
        _routingKey = routingKey;
        _channelName = new ChannelName(routingKey.Value);
        return this;
    }
    
    private Type? _dataType;

    /// <summary>
    /// Sets the .NET type of the expected message payload.
    /// If not already set, this also auto-configures the subscription name, channel name, and topic
    /// based on the type's name.
    /// </summary>
    /// <param name="dataType">The message payload type, or null if dynamic typing is used.</param>
    /// <returns>The current builder instance for chaining.</returns>
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

    /// <summary>
    /// Configures a function to dynamically determine the request type from an incoming raw message.
    /// Used when message types are not known at configuration time.
    /// </summary>
    /// <param name="getRequestType">A function that maps a <see cref="Message"/> to its <see cref="Type"/>, or null.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetGetRequestType(Func<Message, Type>? getRequestType)
    {
        _getRequestType = getRequestType;
        return this;
    }

    private int _bufferSize = 1;

    /// <summary>
    /// Sets the size of the internal buffer used by the message pump.
    /// </summary>
    /// <param name="bufferSize">The number of messages the pump can hold in memory.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetBufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }
    
    private int _noOfPerformers  = 1;

    /// <summary>
    /// Configures the number of parallel performers (handlers) processing messages from the channel.
    /// </summary>
    /// <param name="noOfPerformers">The number of concurrent message processors.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetNumberOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }
    
    private TimeSpan? _timeOut;

    /// <summary>
    /// Sets the timeout for message processing before considering it failed.
    /// </summary>
    /// <param name="timeout">The processing timeout, or null for no timeout.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetTimeout(TimeSpan? timeout)
    {
        _timeOut = timeout;
        return this;
    }
    
    private int _requeueCount = -1;

    /// <summary>
    /// Configures how many times a failed message should be requeued before being dead-lettered.
    /// A value of -1 means infinite retries.
    /// </summary>
    /// <param name="requeueCount">The maximum requeue count.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetRequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }
    
    private TimeSpan? _requeueDelay;

    /// <summary>
    /// Sets the delay before a failed message is requeued for retry.
    /// </summary>
    /// <param name="timeout">The delay duration, or null for immediate retry.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetRequeueDelay(TimeSpan? timeout)
    {
        _requeueDelay = timeout;
        return this;
    }
    
    private int _unacceptableMessageLimit;

    /// <summary>
    /// Sets the maximum number of consecutive unacceptable (e.g., unprocessable) messages
    /// allowed before the channel is considered faulty.
    /// </summary>
    /// <param name="unacceptableMessageLimit">The error tolerance threshold.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetUnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit ;
        return this;
    }
    
    private MessagePumpType _messagePumpType = MessagePumpType.Proactor;

    /// <summary>
    /// Configures the message pump architecture (Proactor or Reactor).
    /// </summary>
    /// <param name="messagePumpType">The pump type.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetMessagePumpType(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }

    private IAmAChannelFactory? _channelFactory;

    /// <summary>
    /// Sets a custom channel factory for creating the underlying messaging channel.
    /// </summary>
    /// <param name="channelFactory">The channel factory implementation, or null to use default.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }
    
    private OnMissingChannel _onMissingChannel = OnMissingChannel.Create;

    /// <summary>
    /// Configures the behavior when the subscribed Kafka topic does not exist.
    /// </summary>
    /// <param name="onMissingChannel">The policy (e.g., Create, Assume, Validate).</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetMakeChannels(OnMissingChannel onMissingChannel)
    {
        _onMissingChannel = onMissingChannel;
        return this;
    }

    private TimeSpan? _emptyChannelDelay;

    /// <summary>
    /// Sets the delay before polling again when the Kafka topic has no messages.
    /// </summary>
    /// <param name="emptyChannelDelay">The backoff duration, or null for default behavior.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetEmptyChannelDelay(TimeSpan? emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }

    private TimeSpan? _channelFailureDelay;

    /// <summary>
    /// Configures the delay before retrying after a channel failure (e.g., connection loss).
    /// </summary>
    /// <param name="channelFailureDelay">The recovery delay, or null for default.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetChannelFailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }

    private long _commitBatchSize = 10;
    
    /// <summary>
    /// Sets the number of messages processed before committing offsets to Kafka.
    /// </summary>
    /// <param name="commitBatchSize">The offset commit batch size.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetCommitBatchSize(long commitBatchSize)
    {
        _commitBatchSize = commitBatchSize;
        return this;
    }

    private Action<ConsumerConfig>? _configHook = null;

    /// <summary>
    /// Allows direct customization of the underlying Kafka consumer configuration.
    /// </summary>
    /// <param name="configHook">An action that modifies the <see cref="ConsumerConfig"/> instance.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetConfigHook(Action<ConsumerConfig> configHook)
    {
        _configHook = configHook;
        return this;
    }
    
    private string? _groupId = null;
    
    /// <summary>
    /// Sets the Kafka consumer group ID. If not provided, Brighter may generate one.
    /// </summary>
    /// <param name="groupId">The consumer group identifier.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetConsumerGroupId(string groupId)
    {
        _groupId = groupId;
        return this;
    }
    
    private IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

    /// <summary>
    /// Configures the transactional isolation level for reading messages.
    /// </summary>
    /// <param name="isolationLevel">The isolation level (e.g., ReadCommitted, ReadUncommitted).</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetIsolationLevel(IsolationLevel isolationLevel)
    {
        _isolationLevel = isolationLevel;
        return this;
    }
    
    private TimeSpan _maxPollInterval = TimeSpan.FromMilliseconds(300000);

    /// <summary>
    /// Sets the maximum delay between poll calls before the consumer is considered dead.
    /// </summary>
    /// <param name="maxPollInterval">The maximum poll interval duration.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetMaxPollInterval(TimeSpan maxPollInterval)
    {
        _maxPollInterval = maxPollInterval;
        return this;
    }
    
    private int _numPartitions = 1;

    /// <summary>
    /// Sets the number of partitions to use if the topic is auto-created.
    /// </summary>
    /// <param name="numPartitions">The number of partitions.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetNumPartitions(int numPartitions)
    {
        _numPartitions = numPartitions;
        return this;
    }

    private AutoOffsetReset _offsetDefault = AutoOffsetReset.Earliest;

    /// <summary>
    /// Configures the offset reset strategy when no committed offset is found.
    /// </summary>
    /// <param name="offsetDefault">The reset policy (e.g., Earliest, Latest).</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetOffsetDefault(AutoOffsetReset offsetDefault)
    {
        _offsetDefault = offsetDefault;
        return this;
    }

    private PartitionAssignmentStrategy _partitionAssignmentStrategy = PartitionAssignmentStrategy.RoundRobin;

    /// <summary>
    /// Sets the strategy used to assign topic partitions to consumer instances in a group.
    /// </summary>
    /// <param name="partitionAssignmentStrategy">The assignment strategy (e.g., RoundRobin, Range).</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetPartitionAssignmentStrategy(PartitionAssignmentStrategy partitionAssignmentStrategy)
    {
        _partitionAssignmentStrategy = partitionAssignmentStrategy;
        return this;
    }

    private TimeSpan _readCommittedOffsetsTimeOut = TimeSpan.FromMilliseconds(5000);

    /// <summary>
    /// Sets the timeout for reading committed offsets during consumer initialization.
    /// </summary>
    /// <param name="readCommittedOffsetsTimeOut">The timeout duration.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetReadCommittedOffsetsTimeOut(TimeSpan readCommittedOffsetsTimeOut)
    {
        _readCommittedOffsetsTimeOut = readCommittedOffsetsTimeOut;
        return this;
    }

    private short _replicationFactor = 1;

    /// <summary>
    /// Sets the replication factor to use if the topic is auto-created.
    /// </summary>
    /// <param name="replicationFactor">The number of replicas.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetReplicationFactor(short replicationFactor)
    {
        _replicationFactor = replicationFactor;
        return this;
    }

    private TimeSpan _sessionTimeout = TimeSpan.FromMilliseconds(10000);

    /// <summary>
    /// Configures the session timeout for detecting consumer failures in the group.
    /// </summary>
    /// <param name="sessionTimeout">The session timeout duration.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetSessionTimeout(TimeSpan sessionTimeout)
    {
        _sessionTimeout = sessionTimeout;
        return this;
    }

    private TimeSpan _sweepUncommittedOffsetsInterval = TimeSpan.FromMilliseconds(30000);

    /// <summary>
    /// Sets the interval at which uncommitted offsets are swept and potentially cleaned up.
    /// </summary>
    /// <param name="sweepUncommittedOffsetsInterval">The sweep interval duration.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetSweepUncommittedOffsetsInterval(TimeSpan sweepUncommittedOffsetsInterval)
    {
        _sweepUncommittedOffsetsInterval = sweepUncommittedOffsetsInterval;
        return this;
    }

    private TimeSpan _topicFindTimeout = TimeSpan.FromMilliseconds(5000);

    /// <summary>
    /// Sets the timeout for operations that require topic metadata lookup (e.g., validation or creation).
    /// </summary>
    /// <param name="topicFindTimeout">The topic lookup timeout duration.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaSubscriptionBuilder SetTopicFindTimeout(TimeSpan topicFindTimeout)
    {
        _topicFindTimeout = topicFindTimeout;
        return this;
    }
    
    /// <summary>
    /// Builds a new instance of <see cref="KafkaSubscription"/> with the configured settings.
    /// </summary>
    /// <returns>A fully configured <see cref="KafkaSubscription"/> instance.</returns>
    /// <exception cref="ConfigurationException">Thrown if required properties (subscription name, channel name, or routing key) are not set.</exception>
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
