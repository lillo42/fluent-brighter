using Fluent.Brighter.Redis;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="RedisPublicationBuilder"/> to simplify configuration with convenient helper methods.
/// These extensions provide readable, intention-revealing methods for configuring channel creation behavior.
/// </summary>
public static class RedisPublicationBuilderExtensions
{
    #region MakeChannels
    /// <summary>
    /// Configures the publication to create Redis topics/channels if they don't exist.
    /// When a publication attempts to send to a non-existent topic, it will be automatically created.
    /// </summary>
    /// <param name="builder">The <see cref="RedisPublicationBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="RedisPublicationBuilder"/> instance for method chaining.</returns>
    public static RedisPublicationBuilder CreateTopicIfMissing(this RedisPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Create);
    }

    /// <summary>
    /// Configures the publication to validate that Redis topics/channels exist before publishing.
    /// If a topic doesn't exist, an error will be raised rather than creating it automatically.
    /// </summary>
    /// <param name="builder">The <see cref="RedisPublicationBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="RedisPublicationBuilder"/> instance for method chaining.</returns>
    public static RedisPublicationBuilder ValidIfTopicExists(this RedisPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Validate);
    }

    /// <summary>
    /// Configures the publication to assume Redis topics/channels exist without validation.
    /// No checks will be performed, and the publication will attempt to use the topic regardless.
    /// This is the most performant option but requires topics to be pre-created.
    /// </summary>
    /// <param name="builder">The <see cref="RedisPublicationBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="RedisPublicationBuilder"/> instance for method chaining.</returns>
    public static RedisPublicationBuilder AssumeTopicExists(this RedisPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Assume);
    }

    #endregion
}