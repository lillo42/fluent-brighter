using System;

using Fluent.Brighter.AWS.V4;

using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for SqsAttributesBuilder to provide fluent configuration
/// for various Amazon SQS queue attributes including message delivery, deduplication,
/// queue type, throughput limits, and dead-letter queue policies.
/// </summary>
public static class SqsAttributesBuilderExtensions
{
    #region Raw Nessage Delivery

    /// <summary>
    /// Enables raw message delivery for SQS subscriptions to SNS topics.
    /// When enabled, messages are delivered as the original payload without SNS envelope.
    /// </summary>
    /// <param name="builder">The SQS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsAttributesBuilder EnableRawMessageDelivery(this SqsAttributesBuilder builder)
    {
        return builder.SetRawMessageDelivery(true);
    }

    /// <summary>
    /// Disables raw message delivery for SQS subscriptions to SNS topics.
    /// When disabled, messages are delivered with SNS envelope wrapping the original payload.
    /// </summary>
    /// <param name="builder">The SQS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsAttributesBuilder DisableRawMessageDelivery(this SqsAttributesBuilder builder)
    {
        return builder.SetRawMessageDelivery(false);
    }
    #endregion

    #region Content Based Deduplication

    /// <summary>
    /// Enables content-based deduplication for FIFO queues.
    /// When enabled, SQS uses a SHA-256 hash of the message body for deduplication.
    /// </summary>
    /// <param name="builder">The SQS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsAttributesBuilder EnableContentBasedDeduplication(this SqsAttributesBuilder builder)
    {
        return builder.SetContentBasedDeduplication(true);
    }

    /// <summary>
    /// Disables content-based deduplication for FIFO queues.
    /// When disabled, explicit message deduplication IDs are required for deduplication.
    /// </summary>
    /// <param name="builder">The SQS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsAttributesBuilder DisableContentBasedDeduplication(this SqsAttributesBuilder builder)
    {
        return builder.SetContentBasedDeduplication(false);
    }
    #endregion

    #region Sqs Type

    /// <summary>
    /// Configures the queue to use standard messaging (non-ordered, best-effort delivery).
    /// </summary>
    /// <param name="builder">The SQS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsAttributesBuilder UseStandardQueue(this SqsAttributesBuilder builder)
    {
        return builder.SetType(SqsType.Standard);
    }

    /// <summary>
    /// Configures the queue to use FIFO messaging (ordered, exactly-once processing,
    /// and deduplication capabilities).
    /// </summary>
    /// <param name="builder">The SQS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsAttributesBuilder UseFifoQueue(this SqsAttributesBuilder builder)
    {
        return builder.SetType(SqsType.Standard);
    }
    #endregion

    #region Deduplication Scope

    /// <summary>
    /// Sets the deduplication scope to message group level for FIFO queues.
    /// Deduplication occurs within individual message groups.
    /// </summary>
    /// <param name="builder">The SQS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsAttributesBuilder UseMessageGroupDeduplicationScope(this SqsAttributesBuilder builder)
    {
        return builder.SetDeduplicationScope(DeduplicationScope.MessageGroup);
    }

    /// <summary>
    /// Sets the deduplication scope to queue level for FIFO queues.
    /// Deduplication occurs across all messages in the queue.
    /// </summary>
    /// <param name="builder">The SQS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsAttributesBuilder UseQueueDeduplicationScope(this SqsAttributesBuilder builder)
    {
        return builder.SetDeduplicationScope(DeduplicationScope.Queue);
    }
    #endregion

    #region Fifo Throughput Limit

    /// <summary>
    /// Sets the throughput limit to per queue for FIFO queues.
    /// The throughput limit is applied at the queue level.
    /// </summary>
    /// <param name="builder">The SQS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsAttributesBuilder UsePerQueueThroughputLimit(this SqsAttributesBuilder builder)
    {
        return builder.SetFifoThroughputLimit(FifoThroughputLimit.PerQueue);
    }

    /// <summary>
    /// Sets the throughput limit to per message group for FIFO queues.
    /// The throughput limit is applied at the message group level.
    /// </summary>
    /// <param name="builder">The SQS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsAttributesBuilder UsePerMessageGroupIdThroughputLimit(this SqsAttributesBuilder builder)
    {
        return builder.SetFifoThroughputLimit(FifoThroughputLimit.PerMessageGroupId);
    }
    #endregion

    #region Redrive Policy

    /// <summary>
    /// Sets the redrive policy (dead-letter queue configuration) using a builder pattern.
    /// </summary>
    /// <param name="builder">The SQS attributes builder instance</param>
    /// <param name="configure">Action to configure the redrive policy</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsAttributesBuilder SetRedrivePolicy(this SqsAttributesBuilder builder,
        Action<RedrivePolicyBuilder> configure)
    {
        var redrivePolicyBuilder = new RedrivePolicyBuilder();
        configure(redrivePolicyBuilder);
        return builder.SetRedrivePolicy(redrivePolicyBuilder.Build());
    }

    #endregion
}