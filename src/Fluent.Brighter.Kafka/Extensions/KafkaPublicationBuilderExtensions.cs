using Fluent.Brighter.Kafka;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="KafkaPublicationBuilder"/>
/// to simplify common configuration patterns for topic management and idempotence.
/// </summary>
public static class KafkaPublicationBuilderExtensions
{
    #region MakeChannels

    /// <summary>
    /// Configures the publisher to automatically create the Kafka topic if it does not exist.
    /// </summary>
    /// <param name="builder">The publication builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaPublicationBuilder CreateTopicIfMissing(this KafkaPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Create);
    }

    /// <summary>
    /// Configures the publisher to validate that the Kafka topic exists at startup,
    /// throwing an exception if it is missing.
    /// </summary>
    /// <param name="builder">The publication builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaPublicationBuilder ValidIfTopicExists(this KafkaPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Validate);
    }

    /// <summary>
    /// Configures the publisher to assume the Kafka topic already exists,
    /// skipping any validation or creation logic.
    /// </summary>
    /// <param name="builder">The publication builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaPublicationBuilder AssumeTopicExists(this KafkaPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Assume);
    }

    #endregion

    #region Idempotence

    /// <summary>
    /// Enables idempotent message production to prevent duplicate messages in case of retries.
    /// </summary>
    /// <param name="builder">The publication builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaPublicationBuilder EnableIdempotence(this KafkaPublicationBuilder builder)
    {
        return builder.SetIdempotence(true);
    }

    /// <summary>
    /// Disables idempotent message production (not recommended unless required for compatibility).
    /// </summary>
    /// <param name="builder">The publication builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaPublicationBuilder DisableIdempotence(this KafkaPublicationBuilder builder)
    {
        return builder.SetIdempotence(false);
    }

    #endregion
}