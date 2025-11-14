
using Fluent.Brighter.Redis;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="RedisSubscriptionBuilder"/> to simplify configuration with convenient helper methods.
/// These extensions provide readable, intention-revealing methods for configuring message pump types and channel creation behavior.
/// </summary>
public static class RedisSubscriptionBuilderExtensions
{
    #region Message Pump

    /// <summary>
    /// Configures the subscription to use the Proactor message pump pattern.
    /// The Proactor pattern uses async/await for non-blocking I/O operations, making it ideal for
    /// high-throughput scenarios and when handling many concurrent operations.
    /// </summary>
    /// <param name="builder">The <see cref="RedisSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public static RedisSubscriptionBuilder UseProactorMode(this RedisSubscriptionBuilder builder)
    {
        return builder.SetMessagePumpType(MessagePumpType.Proactor);
    }

    /// <summary>
    /// Configures the subscription to use the Reactor message pump pattern.
    /// The Reactor pattern uses synchronous processing, which can be simpler to reason about
    /// and may be preferred for CPU-bound operations or simpler workflows.
    /// </summary>
    /// <param name="builder">The <see cref="RedisSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public static RedisSubscriptionBuilder UseReactorMode(this RedisSubscriptionBuilder builder)
    {
        return builder.SetMessagePumpType(MessagePumpType.Reactor);
    }

    #endregion
    
    #region MakeChannels

    /// <summary>
    /// Configures the subscription to create Redis infrastructure (topics/channels) if it doesn't exist.
    /// When a subscription attempts to consume from non-existent infrastructure, it will be automatically created.
    /// </summary>
    /// <param name="builder">The <see cref="RedisSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public static RedisSubscriptionBuilder CreateInfrastructureIfMissing(this RedisSubscriptionBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Create);
    }

    /// <summary>
    /// Configures the subscription to validate that Redis infrastructure (topics/channels) exists before consuming.
    /// If infrastructure doesn't exist, an error will be raised rather than creating it automatically.
    /// </summary>
    /// <param name="builder">The <see cref="RedisSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public static RedisSubscriptionBuilder ValidIfInfrastructureExists(this RedisSubscriptionBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Validate);
    }

    /// <summary>
    /// Configures the subscription to assume Redis infrastructure (topics/channels) exists without validation.
    /// No checks will be performed, and the subscription will attempt to consume regardless.
    /// This is the most performant option but requires infrastructure to be pre-created.
    /// </summary>
    /// <param name="builder">The <see cref="RedisSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="RedisSubscriptionBuilder"/> instance for method chaining.</returns>
    public static RedisSubscriptionBuilder AssumeInfrastructureExists(this RedisSubscriptionBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Assume);
    }

    #endregion
}