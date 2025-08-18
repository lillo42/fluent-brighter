using Fluent.Brighter.RMQ.Sync;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides fluent extension methods for configuring RabbitMQ publication channel creation behavior
/// </summary>
/// <remarks>
/// Simplifies configuration of how producers should handle missing exchanges/topics when publishing messages.
/// </remarks>
public static class RmqPublicationBuilderExtensions
{
    #region MakeChannels
    /// <summary>
    /// Configures the producer to create required exchanges/topics if missing
    /// </summary>
    /// <remarks>
    /// Default behavior. Ensures required infrastructure exists before publishing.
    /// Creates exchanges and bindings if they don't exist.
    /// </remarks>
    /// <param name="builder">Publication builder</param>
    /// <returns>Configured publication builder</returns>
    public static RmqPublicationBuilder CreateTopicIfMissing(this RmqPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Create);
    
    /// <summary>
    /// Configures the producer to validate exchange/topic existence before publishing
    /// </summary>
    /// <remarks>
    /// Validates infrastructure exists at startup. Throws exception if exchanges/topics are missing.
    /// Recommended for production environments to prevent runtime publishing failures.
    /// </remarks>
    /// <param name="builder">Publication builder</param>
    /// <returns>Configured publication builder</returns>
    public static RmqPublicationBuilder ValidIfTopicExists(this RmqPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Validate);
    
    /// <summary>
    /// Configures the producer to assume exchanges/topics exist
    /// </summary>
    /// <remarks>
    /// Skips infrastructure checks. May cause message loss if infrastructure is missing.
    /// Use only when maximum publish performance is required and infrastructure is guaranteed.
    /// </remarks>
    /// <param name="builder">Publication builder</param>
    /// <returns>Configured publication builder</returns>
    public static RmqPublicationBuilder AssumeTopicExists(this RmqPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Assume);
    #endregion
}