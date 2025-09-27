using Fluent.Brighter.RocketMQ;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides fluent extension methods for configuring RocketMQ publication channel creation behavior
/// </summary>
/// <remarks>
/// Simplifies configuration of how producers should handle missing exchanges/topics when publishing messages.
/// </remarks>
public static class RocketMqPublicationBuilderExtensions
{
    #region MakeChannels
    /// <summary>
    /// Configures the producer to validate exchange/topic existence before publishing
    /// </summary>
    /// <remarks>
    /// Validates infrastructure exists at startup. Throws exception if exchanges/topics are missing.
    /// Recommended for production environments to prevent runtime publishing failures.
    /// </remarks>
    /// <param name="builder">Publication builder</param>
    /// <returns>Configured publication builder</returns>
    public static RocketMqPublicationBuilder ValidIfTopicExists(this RocketMqPublicationBuilder builder)
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
    public static RocketMqPublicationBuilder AssumeTopicExists(this RocketMqPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Assume);
    #endregion
}