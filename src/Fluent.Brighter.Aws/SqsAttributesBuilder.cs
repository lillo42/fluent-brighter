using System;
using System.Collections.Generic;

using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

/// <summary>
/// Builder class for fluently configuring Amazon SQS queue attributes.
/// Provides methods to set various queue properties including timeout settings,
/// message delivery options, retention policies, and FIFO-specific configurations.
/// </summary>
public sealed class SqsAttributesBuilder
{
    private TimeSpan? _lockTimeout;
    
    /// <summary>
    /// Sets the visibility timeout for messages in the queue, which determines
    /// how long a message is invisible to other consumers after being retrieved.
    /// </summary>
    /// <param name="lockTimeout">The visibility timeout duration</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsAttributesBuilder SetLockTimeout(TimeSpan? lockTimeout)
    {
        _lockTimeout = lockTimeout;
        return this;
    }

    private TimeSpan? _timeOut;
    
    /// <summary>
    /// Sets the timeout for receive message requests to the SQS queue.
    /// </summary>
    /// <param name="timeOut">The receive message wait timeout</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsAttributesBuilder SetTimeOut(TimeSpan? timeOut)
    {
        _timeOut = timeOut;
        return this;
    }

    private TimeSpan? _delaySeconds;

    /// <summary>
    /// Sets the initial delay for messages when they are added to the queue.
    /// Messages will not be available for consumption until after this delay.
    /// </summary>
    /// <param name="delaySeconds">The delay duration for new messages</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsAttributesBuilder SetDelaySeconds(TimeSpan? delaySeconds)
    {
        _delaySeconds = delaySeconds;
        return this;
    }

    private TimeSpan? _messageRetentionPeriod;
    
    /// <summary>
    /// Sets how long Amazon SQS retains messages that are not deleted.
    /// </summary>
    /// <param name="messageRetentionPeriod">The message retention period</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsAttributesBuilder SetMessageRetentionPeriod(TimeSpan? messageRetentionPeriod)
    {
        _messageRetentionPeriod = messageRetentionPeriod;
        return this;
    }

    private string? _iamPolicy;

    /// <summary>
    /// Sets the IAM policy that defines who can access the queue and what actions they can perform.
    /// </summary>
    /// <param name="iamPolicy">JSON string representing the IAM policy</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsAttributesBuilder SetIamPolicy(string? iamPolicy)
    {
        _iamPolicy = iamPolicy;
        return this;
    }

    private bool _rawMessageDelivery = true;
    
    /// <summary>
    /// Enables or disables raw message delivery for SQS subscriptions to SNS topics.
    /// When enabled, messages are delivered as the original payload without SNS envelope.
    /// </summary>
    /// <param name="rawMessageDelivery">True to enable raw message delivery</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsAttributesBuilder SetRawMessageDelivery(bool rawMessageDelivery)
    {
        _rawMessageDelivery = rawMessageDelivery;
        return this;
    }

    private RedrivePolicy? _redrivePolicy;

    /// <summary>
    /// Sets the redrive policy for dead-letter queue configuration, which specifies
    /// when and how messages are moved to a dead-letter queue after failed processing.
    /// </summary>
    /// <param name="redrivePolicy">The redrive policy configuration</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsAttributesBuilder SetRedrivePolicy(RedrivePolicy? redrivePolicy)
    {
        _redrivePolicy = redrivePolicy;
        return this;
    }

    private Dictionary<string, string>? _tags;
    
    /// <summary>
    /// Sets tags for the SQS queue, which are key-value pairs for organizing and
    /// categorizing AWS resources.
    /// </summary>
    /// <param name="tags">Dictionary of tag key-value pairs</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsAttributesBuilder SetTags(Dictionary<string, string>? tags)
    {
        _tags = tags;
        return this;
    }

    /// <summary>
    /// Adds a single tag to the SQS queue.
    /// </summary>
    /// <param name="key">The tag key</param>
    /// <param name="value">The tag value</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsAttributesBuilder AddTag(string key, string value)
    {
        _tags ??= new Dictionary<string, string>();
        _tags.Add(key, value);
        return this;
    }

    private SqsType _type = SqsType.Standard;

    /// <summary>
    /// Sets the type of SQS queue (Standard or FIFO).
    /// </summary>
    /// <param name="type">The SQS queue type</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsAttributesBuilder SetType(SqsType type)
    {
        _type = type;
        return this;
    }
    
    private bool _contentBasedDeduplication = true;

    /// <summary>
    /// Enables or disables content-based deduplication for FIFO queues.
    /// When enabled, SQS uses a SHA-256 hash of the message body for deduplication.
    /// </summary>
    /// <param name="contentBasedDeduplication">True to enable content-based deduplication</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsAttributesBuilder SetContentBasedDeduplication(bool contentBasedDeduplication)
    {
        _contentBasedDeduplication = contentBasedDeduplication;
        return this;
    }
    
    private DeduplicationScope? _deduplicationScope;
    
    /// <summary>
    /// Sets the deduplication scope for FIFO queues, which determines whether
    /// deduplication occurs at the message group level or queue level.
    /// </summary>
    /// <param name="deduplicationScope">The deduplication scope</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsAttributesBuilder SetDeduplicationScope(DeduplicationScope? deduplicationScope)
    {
        _deduplicationScope = deduplicationScope;
        return this;
    }

    private FifoThroughputLimit? _fifoThroughputLimit;
    
    /// <summary>
    /// Sets the throughput limit for FIFO queues, which controls how many
    /// messages per second the queue can process.
    /// </summary>
    /// <param name="fifoThroughputLimit">The throughput limit setting</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsAttributesBuilder SetFifoThroughputLimit(FifoThroughputLimit? fifoThroughputLimit)
    {
        _fifoThroughputLimit = fifoThroughputLimit;
        return this;
    }

    internal SqsAttributes Build()
    {
        return new SqsAttributes(
            _lockTimeout,
            _delaySeconds,
            _timeOut,
            _messageRetentionPeriod,
            _iamPolicy,
            _rawMessageDelivery,
            _redrivePolicy,
            _tags,
            _type,
            _contentBasedDeduplication,
            _deduplicationScope,
            _fifoThroughputLimit
        );
    }
}