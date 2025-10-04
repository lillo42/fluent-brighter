using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter.Kafka;

/// <summary>
/// Provides a fluent builder for configuring Kafka publication settings
/// in a Brighter integration with Apache Kafka, supporting CloudEvents and producer tuning.
/// </summary>
public sealed class KafkaPublicationBuilder
{
    private Uri? _dataSchema;

    /// <summary>
    /// Sets the URI of the schema that describes the event data format (e.g., JSON Schema, Avro).
    /// Used in CloudEvents 'dataSchema' attribute.
    /// </summary>
    /// <param name="dataSchema">The schema URI, or null if not applicable.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetDataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }

    private OnMissingChannel _makeChannels = OnMissingChannel.Create;

    /// <summary>
    /// Configures the behavior when publishing to a Kafka topic that does not exist.
    /// </summary>
    /// <param name="makeChannels">The policy for handling missing topics (e.g., Create, Assume, Validate).</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }

    private Type? _requestType;

    /// <summary>
    /// Specifies the .NET type of the command or event being published.
    /// Used for routing and serialization.
    /// </summary>
    /// <param name="requestType">The request type, or null if not specified.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetRequestType(Type? requestType)
    {
        _requestType = requestType;
        return this;
    }

    private Uri _source = new(MessageHeader.DefaultSource);

    /// <summary>
    /// Sets the source URI of the event, identifying the context in which the event occurred.
    /// Maps to the CloudEvents 'source' attribute.
    /// </summary>
    /// <param name="source">The event source URI.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetSource(Uri source)
    {
        _source = source;
        return this;
    }

    private string? _subject;

    /// <summary>
    /// Sets an optional subject that describes the specific entity or resource related to the event.
    /// Maps to the CloudEvents 'subject' attribute.
    /// </summary>
    /// <param name="subject">The event subject, or null if not applicable.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetSubject(string? subject)
    {
        _subject = subject;
        return this;
    }

    private RoutingKey? _topic;

    /// <summary>
    /// Specifies the Kafka topic (routing key) to publish messages to.
    /// </summary>
    /// <param name="topic">The routing key representing the Kafka topic name.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetTopic(RoutingKey? topic)
    {
        _topic = topic;
        return this;
    }

    private CloudEventsType _type = CloudEventsType.Empty;

    /// <summary>
    /// Sets the CloudEvents 'type' attribute, which describes the nature of the event.
    /// </summary>
    /// <param name="type">The event type (e.g., "com.example.order.created").</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetType(CloudEventsType type)
    {
        _type = type;
        return this;
    }

    private IDictionary<string, object>? _defaultHeaders;

    /// <summary>
    /// Configures default headers to be included with every published message.
    /// </summary>
    /// <param name="defaultHeaders">A dictionary of header key-value pairs, or null.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetDefaultHeaders(IDictionary<string, object>? defaultHeaders)
    {
        _defaultHeaders = defaultHeaders;
        return this;
    }

    private IDictionary<string, object>? _cloudEventsAdditionalProperties;

    /// <summary>
    /// Sets additional custom properties to include in the CloudEvent envelope.
    /// </summary>
    /// <param name="cloudEventsAdditionalProperties">Custom CloudEvents extension attributes.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetCloudEventsAdditionalProperties(
        IDictionary<string, object>? cloudEventsAdditionalProperties)
    {
        _cloudEventsAdditionalProperties = cloudEventsAdditionalProperties;
        return this;
    }

    private string? _replyTo;

    /// <summary>
    /// Sets the reply-to topic or address for request-reply patterns.
    /// </summary>
    /// <param name="replyTo">The reply destination, or null if not used.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetReplyTo(string? replyTo)
    {
        _replyTo = replyTo;
        return this;
    }

    private Acks _replication = Acks.All;

    /// <summary>
    /// Configures the number of acknowledgments the Kafka producer requires from brokers.
    /// </summary>
    /// <param name="replication">The acknowledgment mode (e.g., All, Leader, None).</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetReplication(Acks replication)
    {
        _replication = replication;
        return this;
    }

    private int _batchNumberMessages = 10000;

    /// <summary>
    /// Sets the maximum number of messages to batch before sending to Kafka.
    /// </summary>
    /// <param name="batchNumberMessages">The batch size in number of messages.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetBatchNumberMessages(int batchNumberMessages)
    {
        _batchNumberMessages = batchNumberMessages;
        return this;
    }

    private bool _enableIdempotence = true;

    /// <summary>
    /// Enables or disables idempotent producer behavior to prevent duplicate messages.
    /// </summary>
    /// <param name="enableIdempotence">True to enable idempotence; otherwise, false.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetIdempotence(bool enableIdempotence)
    {
        _enableIdempotence = enableIdempotence;
        return this;
    }
    
    private int _lingerMs = 5;

    /// <summary>
    /// Sets the time in milliseconds the producer waits to allow more messages to accumulate in a batch.
    /// </summary>
    /// <param name="lingerMs">The linger time in milliseconds.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetLingerMs(int lingerMs)
    {
        _lingerMs = lingerMs;
        return this;
    }

    private int _messageSendMaxRetries = 3;

    /// <summary>
    /// Configures the maximum number of retries for failed message sends.
    /// </summary>
    /// <param name="messageSendMaxRetries">The retry count.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetMessageSendMaxRetries(int messageSendMaxRetries)
    {
        _messageSendMaxRetries = messageSendMaxRetries;
        return this;
    }

    private int _messageTimeoutMs = 5000;

    /// <summary>
    /// Sets the total time in milliseconds the producer will attempt to send a message before timing out.
    /// </summary>
    /// <param name="messageTimeoutMs">The message timeout in milliseconds.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetMessageTimeoutMs(int messageTimeoutMs)
    {
        _messageTimeoutMs = messageTimeoutMs;
        return this;
    }

    private int _maxInFlightRequestsPerConnection = 1;

    /// <summary>
    /// Configures the maximum number of unacknowledged requests the producer will send on a single connection.
    /// Lower values improve ordering guarantees when retries are enabled.
    /// </summary>
    /// <param name="maxInFlightRequestsPerConnection">The maximum in-flight requests per connection.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetMaxInFlightRequestsPerConnection(int maxInFlightRequestsPerConnection)
    {
        _maxInFlightRequestsPerConnection = maxInFlightRequestsPerConnection;
        return this;
    }

    private int _numPartitions = 1;

    /// <summary>
    /// Sets the number of partitions to use when auto-creating a Kafka topic.
    /// </summary>
    /// <param name="numPartitions">The number of partitions.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetNumPartitions(int numPartitions)
    {
        _numPartitions = numPartitions;
        return this;
    }

    private Partitioner _partitioner = Partitioner.ConsistentRandom;

    /// <summary>
    /// Specifies the partitioning strategy used to assign messages to Kafka partitions.
    /// </summary>
    /// <param name="partitioner">The partitioner algorithm (e.g., ConsistentRandom, Murmur2Random).</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetPartitioner(Partitioner partitioner)
    {
        _partitioner = partitioner;
        return this;
    }

    private int _queueBufferingMaxMessages = 100000;

    /// <summary>
    /// Sets the maximum number of messages that can be buffered in the producer queue.
    /// </summary>
    /// <param name="queueBufferingMaxMessages">The maximum message count in the queue.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetQueueBufferingMaxMessages(int queueBufferingMaxMessages)
    {
        _queueBufferingMaxMessages = queueBufferingMaxMessages;
        return this;
    }

    private int _queueBufferingMaxKbytes = 1048576;

    /// <summary>
    /// Sets the maximum total size (in kilobytes) of messages that can be buffered in the producer queue.
    /// </summary>
    /// <param name="queueBufferingMaxKbytes">The maximum queue size in KB.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetQueueBufferingMaxKbytes(int queueBufferingMaxKbytes)
    {
        _queueBufferingMaxKbytes = queueBufferingMaxKbytes;
        return this;
    }

    private short _replicationFactor = 1;

    /// <summary>
    /// Sets the replication factor to use when auto-creating a Kafka topic.
    /// </summary>
    /// <param name="replicationFactor">The number of replicas for the topic.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetReplicationFactor(short replicationFactor)
    {
        _replicationFactor = replicationFactor;
        return this;
    }

    private int _retryBackoff = 100;

    /// <summary>
    /// Configures the backoff time in milliseconds before retrying a failed message send.
    /// </summary>
    /// <param name="retryBackoff">The retry backoff duration in milliseconds.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetRetryBackoff(int retryBackoff)
    {
        _retryBackoff = retryBackoff;
        return this;
    }

    private int _requestTimeoutMs = 500;

    /// <summary>
    /// Sets the time in milliseconds the producer waits for a response from the broker before timing out.
    /// </summary>
    /// <param name="requestTimeoutMs">The request timeout in milliseconds.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetRequestTimeoutMs(int requestTimeoutMs)
    {
        _requestTimeoutMs = requestTimeoutMs;
        return this;
    }

    private int _topicFindTimeoutMs = 5000;
    
    /// <summary>
    /// Sets the timeout in milliseconds for operations that require topic metadata lookup (e.g., validation).
    /// </summary>
    /// <param name="topicFindTimeoutMs">The topic lookup timeout in milliseconds.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetTopicFindTimeoutMs(int topicFindTimeoutMs)
    {
        _topicFindTimeoutMs = topicFindTimeoutMs;
        return this;
    }

    private string? _transactionalId = null;

    /// <summary>
    /// Sets the transactional ID for enabling exactly-once semantics via Kafka transactions.
    /// </summary>
    /// <param name="transactionalId">The unique transactional identifier, or null to disable transactions.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetTransactionalId(string transactionalId)
    {
        _transactionalId = transactionalId;
        return this;
    }

    private IKafkaMessageHeaderBuilder _messageHeaderBuilder = KafkaDefaultMessageHeaderBuilder.Instance;

    /// <summary>
    /// Configures the strategy used to build message headers for outgoing Kafka messages.
    /// </summary>
    /// <param name="messageHeaderBuilder">The header builder implementation.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaPublicationBuilder SetMessageHeaderBuilder(IKafkaMessageHeaderBuilder messageHeaderBuilder)
    {
        _messageHeaderBuilder = messageHeaderBuilder;
        return this;
    }

    /// <summary>
    /// Builds a new instance of <see cref="KafkaPublication"/> with the configured settings.
    /// </summary>
    /// <returns>A fully configured <see cref="KafkaPublication"/> instance.</returns>
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