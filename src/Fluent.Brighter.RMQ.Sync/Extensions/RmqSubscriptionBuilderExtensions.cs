using Fluent.Brighter.RMQ.Sync;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides fluent extension methods for configuring RabbitMQ subscriptions in Brighter
/// </summary>
/// <remarks>
/// Simplifies common configuration scenarios with semantic methods that replace enum parameters.
/// Grouped by configuration area: Message Pump, Infrastructure Creation, Queue Type, 
/// Durability, and High Availability.
/// </remarks>
public static class RmqSubscriptionBuilderExtensions
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
    public static RmqSubscriptionBuilder UseProactorMode(this RmqSubscriptionBuilder builder)
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
    public static RmqSubscriptionBuilder UseReactorMode(this RmqSubscriptionBuilder builder)
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
    public static RmqSubscriptionBuilder CreateInfrastructureIfMissing(this RmqSubscriptionBuilder builder)
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
    public static RmqSubscriptionBuilder ValidIfInfrastructureExists(this RmqSubscriptionBuilder builder)
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
    public static RmqSubscriptionBuilder AssumeInfrastructureExists(this RmqSubscriptionBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Assume);
    #endregion
    
    #region Durable
    /// <summary>
    /// Enables queue durability (survives broker restarts)
    /// </summary>
    /// <remarks>
    /// Messages will be persisted to disk. Recommended for persistent messaging patterns.
    /// </remarks>
    /// <param name="builder">Subscription builder</param>
    /// <returns>Configured subscription builder</returns>
    public static RmqSubscriptionBuilder EnableDurable(this RmqSubscriptionBuilder builder)
        => builder.SetIsDurable(true);
    
    /// <summary>
    /// Disables queue durability (transient queues)
    /// </summary>
    /// <remarks>
    /// Queues will be deleted on broker restart. Use for temporary data only.
    /// </remarks>
    /// <param name="builder">Subscription builder</param>
    /// <returns>Configured subscription builder</returns>
    public static RmqSubscriptionBuilder DisableDurable(this RmqSubscriptionBuilder builder)
        => builder.SetIsDurable(false);
    #endregion
    
    #region HighAvailability 
    /// <summary>
    /// Enables queue mirroring across cluster nodes
    /// </summary>
    /// <remarks>
    /// Provides high availability through cluster-wide mirroring. 
    /// Requires RabbitMQ cluster configuration.
    /// </remarks>
    /// <param name="builder">Subscription builder</param>
    /// <returns>Configured subscription builder</returns>
    public static RmqSubscriptionBuilder EnableHighAvailability(this RmqSubscriptionBuilder builder)
        => builder.SethHighAvailability(true);
    
    /// <summary>
    /// Disables queue mirroring
    /// </summary>
    /// <remarks>
    /// Default behavior. Queue exists only on the node where it was declared.
    /// </remarks>
    /// <param name="builder">Subscription builder</param>
    /// <returns>Configured subscription builder</returns>
    public static RmqSubscriptionBuilder DisableHighAvailability(this RmqSubscriptionBuilder builder)
        => builder.SethHighAvailability(false);
    #endregion
}