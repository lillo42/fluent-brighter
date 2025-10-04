using Fluent.Brighter.Kafka;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="KafkaSubscriptionBuilder"/>
/// to simplify configuration of message pump modes and infrastructure (topic) management policies.
/// </summary>
public static class KafkaSubscriptionBuilderExtensions
{
    #region Message Pump

    /// <summary>
    /// Configures the subscription to use the Proactor message pump pattern,
    /// where message handling is decoupled from I/O polling (default and recommended for most scenarios).
    /// </summary>
    /// <param name="builder">The subscription builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaSubscriptionBuilder UseProactorMode(this KafkaSubscriptionBuilder builder)
    {
        return builder.SetMessagePumpType(MessagePumpType.Proactor);
    }

    /// <summary>
    /// Configures the subscription to use the Reactor message pump pattern,
    /// where message handling occurs on the same thread as I/O polling.
    /// </summary>
    /// <param name="builder">The subscription builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaSubscriptionBuilder UseReactorMode(this KafkaSubscriptionBuilder builder)
    {
        return builder.SetMessagePumpType(MessagePumpType.Reactor);
    }

    #endregion
    
    #region MakeChannels

    /// <summary>
    /// Configures the consumer to automatically create the Kafka topic and related infrastructure
    /// if it does not already exist.
    /// </summary>
    /// <param name="builder">The subscription builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaSubscriptionBuilder CreateInfrastructureIfMissing(this KafkaSubscriptionBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Create);
    }

    /// <summary>
    /// Configures the consumer to validate that the required Kafka topic exists at startup,
    /// throwing an exception if it is missing.
    /// </summary>
    /// <param name="builder">The subscription builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaSubscriptionBuilder ValidIfInfrastructureExists(this KafkaSubscriptionBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Validate);
    }

    /// <summary>
    /// Configures the consumer to assume that the Kafka topic and infrastructure already exist,
    /// skipping any validation or creation logic.
    /// </summary>
    /// <param name="builder">The subscription builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaSubscriptionBuilder AssumeInfrastructureExists(this KafkaSubscriptionBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Assume);
    }

    #endregion
}