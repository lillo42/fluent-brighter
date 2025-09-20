using Fluent.Brighter.Aws;

using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for SnsAttributesBuilder to provide fluent configuration
/// for content-based deduplication and SNS topic type settings.
/// </summary>
public static class SnsAttributesBuilderExtensions
{
    #region Content Based Deduplication
    /// <summary>
    /// Enables content-based deduplication for FIFO topics, which uses a SHA-256 hash
    /// of the message body for deduplication instead of requiring a message deduplication ID.
    /// </summary>
    /// <param name="builder">The SNS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SnsAttributesBuilder EnableContentBasedDeduplication(this SnsAttributesBuilder builder)
        => builder.SetContentBasedDeduplication(true);

    /// <summary>
    /// Disables content-based deduplication for FIFO topics, requiring explicit
    /// message deduplication IDs for deduplication.
    /// </summary>
    /// <param name="builder">The SNS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SnsAttributesBuilder DisableContentBasedDeduplication(this SnsAttributesBuilder builder)
        => builder.SetContentBasedDeduplication(false);
    #endregion

    #region Set Type

    /// <summary>
    /// Configures the SNS topic to use standard messaging (non-ordered, best-effort delivery).
    /// </summary>
    /// <param name="builder">The SNS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SnsAttributesBuilder UseStandardSns(this SnsAttributesBuilder builder)
        => builder.SetType(SqsType.Standard);
    
    /// <summary>
    /// Configures the SNS topic to use FIFO messaging (ordered, exactly-once processing,
    /// and deduplication capabilities).
    /// </summary>
    /// <param name="builder">The SNS attributes builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SnsAttributesBuilder UseFifoSns(this SnsAttributesBuilder builder)
        => builder.SetType(SqsType.Fifo);
    #endregion
}