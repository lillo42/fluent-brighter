using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter.Kafka;

public sealed class KafkaPublicationBuilder
{
    private Uri? _dataSchema;

    public KafkaPublicationBuilder SetDataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }

    private OnMissingChannel _makeChannels = OnMissingChannel.Create; 

    public KafkaPublicationBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }


    private Type? _requestType;

    public KafkaPublicationBuilder SetRequestType(Type? requestType)
    {
        _requestType = requestType;
        return this;
    }


    private Uri _source = new(MessageHeader.DefaultSource);

    public KafkaPublicationBuilder SetSource(Uri source)
    {
        _source = source;
        return this;
    }

    private string? _subject;

    public KafkaPublicationBuilder SetSubject(string? subject)
    {
        _subject = subject;
        return this;
    }

    private RoutingKey? _topic;

    public KafkaPublicationBuilder SetTopic(RoutingKey? topic)
    {
        _topic = topic;
        return this;
    }

    private CloudEventsType _type = CloudEventsType.Empty;

    public KafkaPublicationBuilder SetType(CloudEventsType type)
    {
        _type = type;
        return this;
    }

    private IDictionary<string, object>? _defaultHeaders;

    public KafkaPublicationBuilder SetDefaultHeaders(IDictionary<string, object>? defaultHeaders)
    {
        _defaultHeaders = defaultHeaders;
        return this;
    }


    private IDictionary<string, object>? _cloudEventsAdditionalProperties;

    public KafkaPublicationBuilder SetCloudEventsAdditionalProperties(
        IDictionary<string, object>? cloudEventsAdditionalProperties)
    {
        _cloudEventsAdditionalProperties = cloudEventsAdditionalProperties;
        return this;
    }

    private string? _replyTo;

    public KafkaPublicationBuilder SetReplyTo(string? replyTo)
    {
        _replyTo = replyTo;
        return this;
    }

    private Acks _replication = Acks.All;
    public KafkaPublicationBuilder SetReplication(Acks replication)
    {
        _replication = replication;
        return this;
    }

    private int _batchNumberMessages = 10000;

    public KafkaPublicationBuilder SetBatchNumberMessages(int batchNumberMessages)
    {
        _batchNumberMessages = batchNumberMessages;
        return this;
    }

    private bool _enableIdempotence = true;

    public KafkaPublicationBuilder SetEnableIdempotence(bool enableIdempotence)
    {
        _enableIdempotence = enableIdempotence;
        return this;
    }
    
    private int _lingerMs = 5;

    public KafkaPublicationBuilder SetLingerMs(int lingerMs)
    {
        _lingerMs = lingerMs;
        return this;
    }

    private int _messageSendMaxRetries = 3;

    public KafkaPublicationBuilder SetMessageSendMaxRetries(int messageSendMaxRetries)
    {
        _messageSendMaxRetries = messageSendMaxRetries;
        return this;
    }

    private int _messageTimeoutMs = 5000;

    public KafkaPublicationBuilder SetMessageTimeoutMs(int messageTimeoutMs)
    {
        _messageTimeoutMs = messageTimeoutMs;
        return this;
    }

    private int _maxInFlightRequestsPerConnection = 1;
    public KafkaPublicationBuilder SetMaxInFlightRequestsPerConnection(int maxInFlightRequestsPerConnection)
    {
        _maxInFlightRequestsPerConnection = maxInFlightRequestsPerConnection;
        return this;
    }

    private int _numPartitions = 1;

    public KafkaPublicationBuilder SetNumPartitions(int numPartitions)
    {
        _numPartitions = numPartitions;
        return this;
    }

    private Partitioner _partitioner = Partitioner.ConsistentRandom;

    public KafkaPublicationBuilder SetPartitioner(Partitioner partitioner)
    {
        _partitioner = partitioner;
        return this;
    }

    private int _queueBufferingMaxMessages = 100000;

    public KafkaPublicationBuilder SetQueueBufferingMaxMessages(int queueBufferingMaxMessages)
    {
        _queueBufferingMaxMessages = queueBufferingMaxMessages;
        return this;
    }

    private int _queueBufferingMaxKbytes = 1048576;

    public KafkaPublicationBuilder SetQueueBufferingMaxKbytes(int queueBufferingMaxKbytes)
    {
        _queueBufferingMaxKbytes = queueBufferingMaxKbytes;
        return this;
    }

    private short _replicationFactor = 1;

    public KafkaPublicationBuilder SetReplicationFactor(short replicationFactor)
    {
        _replicationFactor = replicationFactor;
        return this;
    }

    private int _retryBackoff = 100;

    public KafkaPublicationBuilder SetRetryBackoff(int retryBackoff)
    {
        _retryBackoff = retryBackoff;
        return this;
    }

    private int _requestTimeoutMs = 500;

    public KafkaPublicationBuilder SetRequestTimeoutMs(int requestTimeoutMs)
    {
        _requestTimeoutMs = requestTimeoutMs;
        return this;
    }

    private int _topicFindTimeoutMs = 5000;
    
    public KafkaPublicationBuilder SetTopicFindTimeoutMs(int topicFindTimeoutMs)
    {
        _topicFindTimeoutMs = topicFindTimeoutMs;
        return this;
    }

    private string? _transactionalId = null;
    public KafkaPublicationBuilder SetTransactionalId(string transactionalId)
    {
        _transactionalId = transactionalId;
        return this;
    }

    
    private IKafkaMessageHeaderBuilder _messageHeaderBuilder = KafkaDefaultMessageHeaderBuilder.Instance;
    public KafkaPublicationBuilder SetMessageHeaderBuilder(IKafkaMessageHeaderBuilder messageHeaderBuilder)
    {
        _messageHeaderBuilder = messageHeaderBuilder;
        return this;
    }

    internal KafkaPublication Build()
    {
        return new KafkaPublication
        {
            DataSchema = _dataSchema,
            MakeChannels = _makeChannels,
            RequestType = _requestType,
            Source = _source,
            Subject = _subject,
            Topic = _topic,
            Type = _type,
            DefaultHeaders = _defaultHeaders,
            CloudEventsAdditionalProperties = _cloudEventsAdditionalProperties,
            ReplyTo = _replyTo,
            Replication = _replication,
            BatchNumberMessages = _batchNumberMessages,
            EnableIdempotence = _enableIdempotence,
            LingerMs = _lingerMs,
            MessageSendMaxRetries = _messageSendMaxRetries,
            MessageTimeoutMs = _messageTimeoutMs,
            MaxInFlightRequestsPerConnection = _maxInFlightRequestsPerConnection,
            NumPartitions = _numPartitions,
            Partitioner = _partitioner,
            QueueBufferingMaxMessages = _queueBufferingMaxMessages,
            QueueBufferingMaxKbytes = _queueBufferingMaxKbytes,
            ReplicationFactor = _replicationFactor,
            RetryBackoff = _retryBackoff,
            RequestTimeoutMs = _requestTimeoutMs,
            TopicFindTimeoutMs = _topicFindTimeoutMs,
            TransactionalId = _transactionalId,
            MessageHeaderBuilder = _messageHeaderBuilder
        };
    }
}