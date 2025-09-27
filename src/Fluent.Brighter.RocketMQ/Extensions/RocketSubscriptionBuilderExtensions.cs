using Fluent.Brighter.RocketMQ;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides fluent extension methods for configuring Rocket subscriptions in Brighter
/// </summary>
/// <remarks>
/// Simplifies common configuration scenarios with semantic methods that replace enum parameters.
/// Grouped by configuration area: Message Pump, Infrastructure Creation, Queue Type and etc
/// </remarks>
public static class RocketSubscriptionBuilderExtensions
{
    #region Message Pump
    /// <summary>
    /// Configures the subscription to use Proactor message pump mode (default)
    /// </summary>
    /// <remarks>
    /// Proactor mode uses a dedicated thread per consumer. Recommended for most scenarios.
    /// </remarks>
    /// <param name="builder">Subscription builder</param>
    /// <returns>Configured subscription builder</returns>
    public static RocketSubscriptionBuilder UseProactorMode(this RocketSubscriptionBuilder builder)
        => builder.SetMessagePumpType(MessagePumpType.Proactor);
    
    /// <summary>
    /// Configures the subscription to use Reactor message pump mode
    /// </summary>
    /// <remarks>
    /// Reactor mode uses a shared thread model. Use for high-throughput scenarios 
    /// where thread efficiency is critical.
    /// </remarks>
    /// <param name="builder">Subscription builder</param>
    /// <returns>Configured subscription builder</returns>
    public static RocketSubscriptionBuilder UseReactorMode(this RocketSubscriptionBuilder builder)
        => builder.SetMessagePumpType(MessagePumpType.Reactor);
    #endregion
    
    #region MakeChannels
    /// <summary>
    /// Configures the subscription to create required infrastructure (queues/exchanges) if missing
    /// </summary>
    /// <remarks>
    /// Default behavior. Creates queues, exchanges, and bindings on startup if they don't exist.
    /// </remarks>
    /// <param name="builder">Subscription builder</param>
    /// <returns>Configured subscription builder</returns>
    public static RocketSubscriptionBuilder CreateInfrastructureIfMissing(this RocketSubscriptionBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Create);
    
    /// <summary>
    /// Configures the subscription to validate infrastructure existence on startup
    /// </summary>
    /// <remarks>
    /// Throws exception during startup if required queues/exchanges are missing.
    /// Recommended for production environments.
    /// </remarks>
    /// <param name="builder">Subscription builder</param>
    /// <returns>Configured subscription builder</returns>
    public static RocketSubscriptionBuilder ValidIfInfrastructureExists(this RocketSubscriptionBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Validate);
    
    /// <summary>
    /// Configures the subscription to assume infrastructure exists
    /// </summary>
    /// <remarks>
    /// Skips infrastructure checks. May cause runtime failures if infrastructure is missing.
    /// Use only when maximum startup speed is required.
    /// </remarks>
    /// <param name="builder">Subscription builder</param>
    /// <returns>Configured subscription builder</returns>
    public static RocketSubscriptionBuilder AssumeInfrastructureExists(this RocketSubscriptionBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Assume);
    #endregion
}