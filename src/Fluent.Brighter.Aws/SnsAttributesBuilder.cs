using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

/// <summary>
/// Builder class for fluently configuring Amazon SNS topic attributes.
/// Provides methods to set delivery policies, access policies, queue types,
/// and deduplication settings for SNS topics.
/// </summary>
public sealed class SnsAttributesBuilder
{
    private string? _deliveryPolicy;

    /// <summary>
    /// Sets the delivery policy for the SNS topic, which defines how messages
    /// are retried when they cannot be delivered to a subscriber.
    /// </summary>
    /// <param name="deliveryPolicy">JSON string representing the delivery policy</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsAttributesBuilder SetDeliveryPolicy(string? deliveryPolicy)
    {
        _deliveryPolicy = deliveryPolicy;
        return this;
    }

    private string? _policy;
    
    /// <summary>
    /// Sets the access policy for the SNS topic, which controls permissions
    /// for who can publish and subscribe to the topic.
    /// </summary>
    /// <param name="policy">JSON string representing the access policy</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsAttributesBuilder SetPolicy(string? policy)
    {
        _policy = policy;
        return this;
    }

    private SqsType _type = SqsType.Standard;
    
    /// <summary>
    /// Sets the type of SQS queue that will subscribe to this SNS topic.
    /// </summary>
    /// <param name="type">The SQS queue type (Standard or FIFO)</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsAttributesBuilder SetType(SqsType type)
    {
        _type = type;
        return this;
    }

    private bool _contentBasedDeduplication = true;
    
    /// <summary>
    /// Enables or disables content-based deduplication for FIFO topics.
    /// When enabled, SNS will use the message content to identify duplicates.
    /// </summary>
    /// <param name="contentBasedDeduplication">True to enable content-based deduplication</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsAttributesBuilder SetContentBasedDeduplication(bool contentBasedDeduplication)
    {
        _contentBasedDeduplication = contentBasedDeduplication;
        return this;
    }

    internal SnsAttributes Build()
    {
        return new SnsAttributes(_deliveryPolicy, _policy, _type, _contentBasedDeduplication);
    }
}