using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

/// <summary>
/// Builder class for fluently configuring an SQS subscription in Paramore.Brighter.
/// Provides methods to set various properties for message consumption from Amazon SQS queues,
/// including subscription naming, channel configuration, message handling behavior,
/// error handling, and resource management.
/// </summary>
public sealed class SqsSubscriptionBuilder
{
    private SubscriptionName? _subscriptionName;
    
    /// <summary>
    /// Sets the name of the subscription, which uniquely identifies this consumer.
    /// </summary>
    /// <param name="subscriptionName">The name of the subscription</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsSubscriptionBuilder SetSubscriptionName(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }

    private ChannelName? _channelName;
    
    /// <summary>
    /// Sets the SQS queue name and automatically configures routing key and channel type
    /// for point-to-point messaging if not already set.
    /// </summary>
    /// <param name="channelName">The name of the SQS queue/channel</param>
    /// <returns>The builder instance for method chaining</returns>
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
    
    /// <summary>
    /// Sets the topic/routing key for the subscription and configures the channel type
    /// for publish-subscribe messaging.
    /// </summary>
    /// <param name="routingKey">The topic/routing key</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsSubscriptionBuilder SetTopic(RoutingKey routingKey)
    {
        _routingKey = routingKey;
        _channelType = ChannelType.PubSub;
        return this;
    }
    
    private ChannelType? _channelType;
    
    /// <summary>
    /// Sets the channel type explicitly (Point-to-Point or Publish-Subscribe).
    /// </summary>
    /// <param name="channelType">The channel type</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsSubscriptionBuilder SetChannelType(ChannelType channelType)
    {
        _channelType = channelType;
        return this;
    }


    private Type? _requestType;
    
    /// <summary>
    /// Sets the .NET type of the request message being consumed and automatically
    /// derives subscription name, channel name, and routing key if not explicitly set.
    /// </summary>
    /// <param name="requestType">The type of the message request</param>
    /// <returns>The builder instance for method chaining</returns>
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
    
    /// <summary>
    /// Sets a function to dynamically determine the request type from a message,
    /// useful for polymorphic message handling.
    /// </summary>
    /// <param name="getRequestType">Function to extract the request type from a message</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsSubscriptionBuilder SetGetRequestType(Func<Message, Type>? getRequestType)
    {
        _getRequestType = getRequestType;
        return this;
    }

    private int _bufferSize = 1;
    
    /// <summary>
    /// Sets the buffer size for the message pump, which controls how many messages
    /// are pre-fetched from the queue.
    /// </summary>
    /// <param name="bufferSize">The number of messages to buffer</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsSubscriptionBuilder SetBufferSize(int bufferSize)
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
    public SqsSubscriptionBuilder SetNoOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }

    private TimeSpan? _timeOut;
    
    /// <summary>
    /// Sets the timeout for receive message operations from the SQS queue.
    /// </summary>
    /// <param name="timeOut">The receive message timeout</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsSubscriptionBuilder SetTimeOut(TimeSpan? timeOut)
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
    public SqsSubscriptionBuilder SetRequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }

    private TimeSpan? _requeueDelay;
    
    /// <summary>
    /// Sets the delay before a requeued message becomes visible again for processing.
    /// </summary>
    /// <param name="requeueDelay">The delay before requeued messages become visible</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsSubscriptionBuilder SetRequeueDelay(TimeSpan? requeueDelay)
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
    public SqsSubscriptionBuilder SetUnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit;
        return this;
    }

    private MessagePumpType _messagePumpType = MessagePumpType.Proactor;
    
    /// <summary>
    /// Sets the message pump type (Proactor or Task Queue) which determines
    /// how messages are processed.
    /// </summary>
    /// <param name="messagePumpType">The message pump type</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsSubscriptionBuilder SetMessagePumpType(MessagePumpType messagePumpType)
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
    public SqsSubscriptionBuilder SetChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }

    private TimeSpan? _emptyChannelDelay;
    
    /// <summary>
    /// Sets the delay when an empty channel is encountered before checking for new messages.
    /// </summary>
    /// <param name="emptyChannelDelay">The delay when channel is empty</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsSubscriptionBuilder SetEmptyChannelDelay(TimeSpan? emptyChannelDelay)
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
    public SqsSubscriptionBuilder SetChannelFailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }

    private TopicFindBy _findTopicBy = TopicFindBy.Convention;

    /// <summary>
    /// Sets the method for finding the target topic (by convention, name, or ARN).
    /// </summary>
    /// <param name="findTopicBy">The topic lookup strategy</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsSubscriptionBuilder SetFindTopicBy(TopicFindBy findTopicBy)
    {
        _findTopicBy = findTopicBy;
        return this;
    }

    private QueueFindBy _findQueueBy = QueueFindBy.Name;
    
    /// <summary>
    /// Sets the method for finding the target queue (by name or ARN).
    /// </summary>
    /// <param name="findQueueBy">The queue lookup strategy</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsSubscriptionBuilder SetFindQueueBy(QueueFindBy findQueueBy)
    {
        _findQueueBy = findQueueBy;
        return this;
    }

    private SqsAttributes? _queueAttributes;
    
    /// <summary>
    /// Sets custom attributes for the SQS queue when creating a new queue.
    /// </summary>
    /// <param name="queueAttributes">SQS queue attributes</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsSubscriptionBuilder SetQueueAttributes(SqsAttributes? queueAttributes)
    {
        _queueAttributes = queueAttributes;
        return this;
    }

    private SnsAttributes? _topicAttributes;
    
    /// <summary>
    /// Sets custom attributes for the SNS topic when creating a new topic.
    /// </summary>
    /// <param name="topicAttributes">SNS topic attributes</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsSubscriptionBuilder SetTopicAttributes(SnsAttributes? topicAttributes)
    {
        _topicAttributes = topicAttributes;
        return this;
    }

    private OnMissingChannel _makeChannels = OnMissingChannel.Create;
    
    /// <summary>
    /// Sets the channel creation behavior when a queue/topic doesn't exist.
    /// </summary>
    /// <param name="makeChannels">Policy for channel creation</param>
    /// <returns>The builder instance for method chaining</returns>
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