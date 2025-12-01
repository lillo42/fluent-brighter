using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.GcpPubSub;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for fluently configuring a Google Cloud Pub/Sub Dead Letter Policy (DLQ).
/// Provides methods to set the dead letter topic, subscription, publisher/subscriber members,
/// acknowledgment deadline, and maximum delivery attempts.
/// </summary>
public sealed class DeadLetterPolicyBuilder
{
    private RoutingKey? _topicName;

    /// <summary>
    /// Sets the routing key for the Dead Letter Topic (DLT) where messages that exceed the max delivery attempts are sent.
    /// This topic must exist on Google Cloud Pub/Sub.
    /// </summary>
    /// <param name="topicName">The routing key for the dead letter topic</param>
    /// <returns>The builder instance for method chaining</returns>
    public DeadLetterPolicyBuilder SetTopicName(RoutingKey topicName)
    {
        _topicName = topicName;
        return this;
    }

    private ChannelName? _subscription;

    /// <summary>
    /// Sets the channel name of the main subscription that this dead letter policy applies to.
    /// </summary>
    /// <param name="subscription">The subscription channel name</param>
    /// <returns>The builder instance for method chaining</returns>
    public DeadLetterPolicyBuilder SetSubscription(ChannelName subscription)
    {
        _subscription = subscription;
        return this;
    }

    private string? _publisherMember;

    /// <summary>
    /// Sets the member field in the main message that identifies the publisher.
    /// This is often used for logging or tracing purposes.
    /// </summary>
    /// <param name="publisherMember">The publisher member field name</param>
    /// <returns>The builder instance for method chaining</returns>
    public DeadLetterPolicyBuilder SetPublisherMember(string? publisherMember)
    {
        _publisherMember = publisherMember;
        return this;
    }

    private string? _subscriberMember;

    /// <summary>
    /// Sets the member field in the main message that identifies the subscriber.
    /// This is often used for logging or tracing purposes.
    /// </summary>
    /// <param name="subscriberMember">The subscriber member field name</param>
    /// <returns>The builder instance for method chaining</returns>
    public DeadLetterPolicyBuilder SetSubscriberMember(string? subscriberMember)
    {
        _subscriberMember = subscriberMember;
        return this;
    }

    private int _ackDeadlineSeconds = 60;

    /// <summary>
    /// Sets the number of seconds the subscriber has to acknowledge a message before Pub/Sub redelivers it.
    /// This value is applied to the dead-letter subscription (the subscription on the DLT) if one is created by Brighter.
    /// The default is 60 seconds, which is the Pub/Sub default.
    /// </summary>
    /// <param name="ackDeadlineSeconds">The acknowledgment deadline in seconds</param>
    /// <returns>The builder instance for method chaining</returns>
    public DeadLetterPolicyBuilder SetAckDeadlineSeconds(int ackDeadlineSeconds)
    {
        _ackDeadlineSeconds = ackDeadlineSeconds;
        return this;
    }

    private int _maxDeliveryAttempts = 10;

    /// <summary>
    /// Sets the maximum number of times Pub/Sub attempts to deliver a message before
    /// sending it to the Dead Letter Topic.
    /// The value must be between 5 and 100. The default is 10.
    /// </summary>
    /// <param name="maxDeliveryAttempts">The maximum number of delivery attempts</param>
    /// <returns>The builder instance for method chaining</returns>
    public DeadLetterPolicyBuilder SetMaxDeliveryAttempts(int maxDeliveryAttempts)
    {
        _maxDeliveryAttempts = maxDeliveryAttempts;
        return this;
    }

    /// <summary>
    /// Builds a DeadLetterPolicy instance with the configured values.
    /// </summary>
    /// <returns>A configured DeadLetterPolicy instance</returns>
    /// <exception cref="ConfigurationException">Thrown when required configuration is missing</exception>
    internal DeadLetterPolicy Build()
    {
        if (_topicName == null)
        {
            throw new ConfigurationException("Dead letter topic name not set");
        }

        if (_subscription == null)
        {
            throw new ConfigurationException("Subscription name not set");
        }

        return new DeadLetterPolicy(_topicName, _subscription)
        {
            PublisherMember = _publisherMember,
            SubscriberMember = _subscriberMember,
            AckDeadlineSeconds = _ackDeadlineSeconds,
            MaxDeliveryAttempts = _maxDeliveryAttempts
        };
    }
}