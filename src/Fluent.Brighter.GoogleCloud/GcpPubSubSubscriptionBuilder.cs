using System;

using Google.Cloud.PubSub.V1;
using Google.Protobuf.Collections;
using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.GcpPubSub;

using DeadLetterPolicy = Paramore.Brighter.MessagingGateway.GcpPubSub.DeadLetterPolicy;
using SubscriptionName = Paramore.Brighter.SubscriptionName;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for fluently configuring a Google Cloud Pub/Sub subscription in Paramore.Brighter.
/// Provides methods to set various properties for message consumption from Google Cloud Pub/Sub,
/// including subscription naming, channel configuration, message handling behavior,
/// error handling, and GCP-specific features like dead letter policies and message ordering.
/// </summary>
public sealed class GcpPubSubSubscriptionBuilder
{
    private SubscriptionName? _subscriptionName;

    /// <summary>
    /// Sets the name of the subscription, which uniquely identifies this consumer.
    /// </summary>
    /// <param name="subscriptionName">The name of the subscription</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetSubscriptionName(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        if (ChannelName.IsNullOrEmpty(_channelName))
        {
            _channelName = new ChannelName(_subscriptionName.Value);
        }
        
        return this;
    }

    private ChannelName? _channelName;

    /// <summary>
    /// Sets the channel name for the subscription.
    /// In GCP Pub/Sub, this represents the subscription name in the platform.
    /// </summary>
    /// <param name="channelName">The name of the channel</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetSubscription(ChannelName channelName)
    {
        _channelName = channelName;
        if (_subscriptionName == null)
        {
            _subscriptionName = new SubscriptionName(channelName.Value);
        }
        
        return this;
    }

    private RoutingKey? _routingKey;

    /// <summary>
    /// Sets the topic/routing key for the subscription.
    /// This is the Pub/Sub topic that the subscription will consume messages from.
    /// </summary>
    /// <param name="routingKey">The topic/routing key</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetTopic(RoutingKey routingKey)
    {
        _routingKey = routingKey;
        return this;
    }

    private Type? _requestType;

    /// <summary>
    /// Sets the .NET type of the request message being consumed and automatically
    /// derives subscription name, channel name, and routing key if not explicitly set.
    /// </summary>
    /// <param name="requestType">The type of the message request</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetDataType(Type? requestType)
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

    /// <summary>
    /// Sets a function to dynamically determine the request type from a message,
    /// useful for polymorphic message handling.
    /// </summary>
    /// <param name="getRequestType">Function to extract the request type from a message</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetGetRequestType(Func<Message, Type>? getRequestType)
    {
        _getRequestType = getRequestType;
        return this;
    }

    private int _bufferSize = 1;

    /// <summary>
    /// Sets the buffer size for the message pump, which controls how many messages
    /// are pre-fetched from the subscription.
    /// </summary>
    /// <param name="bufferSize">The number of messages to buffer</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetBufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }

    private int _noOfPerformers = 1;

    /// <summary>
    /// Sets the number of concurrent performers (threads/processes) that will
    /// process messages from this subscription.
    /// </summary>
    /// <param name="noOfPerformers">The number of concurrent performers</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetNoOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }

    private TimeSpan? _timeOut;

    /// <summary>
    /// Sets the timeout for receive message operations from the Pub/Sub subscription.
    /// </summary>
    /// <param name="timeOut">The receive message timeout</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetTimeOut(TimeSpan? timeOut)
    {
        _timeOut = timeOut;
        return this;
    }

    private int _requeueCount = -1;

    /// <summary>
    /// Sets the maximum number of times a message can be requeued after failed processing.
    /// A value of -1 indicates unlimited requeues.
    /// </summary>
    /// <param name="requeueCount">The maximum number of requeue attempts</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetRequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }

    private TimeSpan? _requeueDelay;

    /// <summary>
    /// Sets the minimum delay before a requeued message becomes visible again for processing.
    /// This works in conjunction with MaxRequeueDelay for exponential backoff.
    /// </summary>
    /// <param name="requeueDelay">The minimum delay before requeued messages become visible</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetRequeueDelay(TimeSpan? requeueDelay)
    {
        _requeueDelay = requeueDelay;
        return this;
    }

    private int _unacceptableMessageLimit;

    /// <summary>
    /// Sets the limit for consecutive unacceptable messages before the channel is terminated.
    /// </summary>
    /// <param name="unacceptableMessageLimit">The maximum number of unacceptable messages</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetUnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit;
        return this;
    }

    private MessagePumpType _messagePumpType = MessagePumpType.Proactor;

    /// <summary>
    /// Sets the message pump type (Proactor for async or Reactor for sync) which determines
    /// how messages are processed.
    /// </summary>
    /// <param name="messagePumpType">The message pump type</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetMessagePumpType(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }

    private IAmAChannelFactory? _channelFactory;

    /// <summary>
    /// Sets a custom channel factory for creating channels to the message queue.
    /// </summary>
    /// <param name="channelFactory">The channel factory implementation</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }

    private OnMissingChannel _makeChannels = OnMissingChannel.Create;

    /// <summary>
    /// Sets the channel creation behavior when a subscription/topic doesn't exist.
    /// </summary>
    /// <param name="makeChannels">Policy for channel creation (validate, create, or assume)</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }

    private TimeSpan? _emptyChannelDelay;

    /// <summary>
    /// Sets the delay when an empty channel is encountered before checking for new messages.
    /// </summary>
    /// <param name="emptyChannelDelay">The delay when channel is empty</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetEmptyChannelDelay(TimeSpan? emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }

    private TimeSpan? _channelFailureDelay;

    /// <summary>
    /// Sets the delay after a channel failure before attempting to reconnect.
    /// </summary>
    /// <param name="channelFailureDelay">The delay after channel failure</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetChannelFailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }

    private string? _projectId;

    /// <summary>
    /// Sets the Google Cloud Project ID where the subscription and its topic reside.
    /// If null, the default project ID from the connection will be used.
    /// </summary>
    /// <param name="projectId">The Google Cloud Project ID</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetProjectId(string? projectId)
    {
        _projectId = projectId;
        return this;
    }

    private TopicAttributes? _topicAttributes;

    /// <summary>
    /// Sets the attributes for the associated Google Cloud Pub/Sub Topic.
    /// This is used for Topic creation and configuration during infrastructure setup.
    /// </summary>
    /// <param name="topicAttributes">The topic attributes configuration</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetTopicAttributes(TopicAttributes? topicAttributes)
    {
        _topicAttributes = topicAttributes;
        return this;
    }

    private int _ackDeadlineSeconds = 30;

    /// <summary>
    /// Sets the acknowledgment deadline in seconds for the subscription.
    /// This is the time the subscriber has to acknowledge a message before Pub/Sub redelivers it.
    /// </summary>
    /// <param name="ackDeadlineSeconds">The acknowledgment deadline in seconds</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetAckDeadlineSeconds(int ackDeadlineSeconds)
    {
        _ackDeadlineSeconds = ackDeadlineSeconds;
        return this;
    }

    private bool _retainAckedMessages;

    /// <summary>
    /// Sets whether Pub/Sub should retain acknowledged messages.
    /// This is typically used for replay functionality.
    /// </summary>
    /// <param name="retainAckedMessages">True to retain acknowledged messages</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetRetainAckedMessages(bool retainAckedMessages)
    {
        _retainAckedMessages = retainAckedMessages;
        return this;
    }

    private TimeSpan? _messageRetentionDuration;

    /// <summary>
    /// Sets the duration for which Pub/Sub retains messages published to the topic.
    /// This setting is applied to the subscription during creation/update.
    /// </summary>
    /// <param name="messageRetentionDuration">The message retention duration</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetMessageRetentionDuration(TimeSpan? messageRetentionDuration)
    {
        _messageRetentionDuration = messageRetentionDuration;
        return this;
    }

    private MapField<string, string>? _labels;

    /// <summary>
    /// Sets a collection of key-value pairs that are attached to the subscription as labels.
    /// Labels are used for organization, billing, and resource management.
    /// </summary>
    /// <param name="labels">Dictionary of label key-value pairs</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetLabels(MapField<string, string>? labels)
    {
        _labels = labels;
        return this;
    }

    /// <summary>
    /// Adds a single label to the subscription.
    /// </summary>
    /// <param name="key">The label key</param>
    /// <param name="value">The label value</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder AddLabel(string key, string value)
    {
        _labels ??= new MapField<string, string>();
        _labels[key] = value;
        return this;
    }

    private bool _enableMessageOrdering;

    /// <summary>
    /// Sets whether messages published to the topic are delivered in the order they were published,
    /// provided they were published with an ordering key.
    /// </summary>
    /// <param name="enableMessageOrdering">True to enable message ordering</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetEnableMessageOrdering(bool enableMessageOrdering)
    {
        _enableMessageOrdering = enableMessageOrdering;
        return this;
    }

    private bool _enableExactlyOnceDelivery;

    /// <summary>
    /// Sets whether exactly-once delivery is enabled for the subscription.
    /// </summary>
    /// <param name="enableExactlyOnceDelivery">True to enable exactly-once delivery</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetEnableExactlyOnceDelivery(bool enableExactlyOnceDelivery)
    {
        _enableExactlyOnceDelivery = enableExactlyOnceDelivery;
        return this;
    }

    private CloudStorageConfig? _storage;

    /// <summary>
    /// Sets the configuration for forwarding message snapshots to a Google Cloud Storage bucket.
    /// This is used for data export/backup functionality.
    /// </summary>
    /// <param name="storage">The cloud storage configuration</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetStorage(CloudStorageConfig? storage)
    {
        _storage = storage;
        return this;
    }

    private ExpirationPolicy? _expirationPolicy;

    /// <summary>
    /// Sets the subscription's expiration policy.
    /// If set, the subscription will automatically be deleted after a period of inactivity.
    /// </summary>
    /// <param name="expirationPolicy">The expiration policy</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetExpirationPolicy(ExpirationPolicy? expirationPolicy)
    {
        _expirationPolicy = expirationPolicy;
        return this;
    }

    private DeadLetterPolicy? _deadLetter;

    /// <summary>
    /// Sets the configuration for the Dead Letter Policy (DLQ).
    /// If set, messages that fail processing will be forwarded to a specified dead letter topic.
    /// </summary>
    /// <param name="deadLetter">The dead letter policy</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetDeadLetter(DeadLetterPolicy? deadLetter)
    {
        _deadLetter = deadLetter;
        return this;
    }

    private TimeSpan? _maxRequeueDelay;

    /// <summary>
    /// Sets the maximum delay time for exponential backoff retry policy when a message is requeued.
    /// This works in conjunction with RequeueDelay which is the minimum backoff.
    /// </summary>
    /// <param name="maxRequeueDelay">The maximum requeue delay</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetMaxRequeueDelay(TimeSpan? maxRequeueDelay)
    {
        _maxRequeueDelay = maxRequeueDelay;
        return this;
    }

    private TimeProvider? _timeProvider;

    /// <summary>
    /// Sets the time provider used for time-related operations (e.g., purging).
    /// </summary>
    /// <param name="timeProvider">The time provider</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetTimeProvider(TimeProvider? timeProvider)
    {
        _timeProvider = timeProvider;
        return this;
    }

    private SubscriptionMode _subscriptionMode = SubscriptionMode.Stream;

    /// <summary>
    /// Sets the message consumption mode: Stream (default) or Pull.
    /// </summary>
    /// <param name="subscriptionMode">The subscription mode</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetSubscriptionMode(SubscriptionMode subscriptionMode)
    {
        _subscriptionMode = subscriptionMode;
        return this;
    }

    private Action<SubscriberClientBuilder>? _streamingConfiguration;

    /// <summary>
    /// Sets an action to configure the SubscriberClientBuilder used for the streaming consumer.
    /// This allows for advanced customization of the underlying streaming client configuration.
    /// </summary>
    /// <param name="streamingConfiguration">Action to configure the SubscriberClientBuilder</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPubSubSubscriptionBuilder SetStreamingConfiguration(Action<SubscriberClientBuilder>? streamingConfiguration)
    {
        _streamingConfiguration = streamingConfiguration;
        return this;
    }

    /// <summary>
    /// Builds a GcpPubSubSubscription instance with the configured values.
    /// </summary>
    /// <returns>A configured GcpPubSubSubscription instance</returns>
    /// <exception cref="ConfigurationException">Thrown when required configuration is missing</exception>
    internal GcpPubSubSubscription Build()
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

        return new GcpPubSubSubscription(
            subscriptionName: _subscriptionName,
            channelName: _channelName,
            routingKey: _routingKey,
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
            makeChannels: _makeChannels,
            emptyChannelDelay: _emptyChannelDelay,
            channelFailureDelay: _channelFailureDelay,
            projectId: _projectId,
            topicAttributes: _topicAttributes,
            ackDeadlineSeconds: _ackDeadlineSeconds,
            retainAckedMessages: _retainAckedMessages,
            messageRetentionDuration: _messageRetentionDuration,
            labels: _labels,
            enableMessageOrdering: _enableMessageOrdering,
            enableExactlyOnceDelivery: _enableExactlyOnceDelivery,
            storage: _storage,
            expirationPolicy: _expirationPolicy,
            deadLetter: _deadLetter,
            maxRequeueDelay: _maxRequeueDelay,
            timeProvider: _timeProvider,
            subscriptionMode: _subscriptionMode,
            streamingConfiguration: _streamingConfiguration
        );
    }
}