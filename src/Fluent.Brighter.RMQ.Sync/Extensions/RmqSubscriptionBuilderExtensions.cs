using Fluent.Brighter.RMQ.Sync;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class RmqSubscriptionBuilderExtensions
{
    #region Message Pump
    public static RmqSubscriptionBuilder UseProactorMode(this RmqSubscriptionBuilder builder)
        => builder.SetMessagePumpType(MessagePumpType.Proactor);
    
    public static RmqSubscriptionBuilder UseReactorMode(this RmqSubscriptionBuilder builder)
        => builder.SetMessagePumpType(MessagePumpType.Reactor);
    #endregion
    
    #region MakeChannels
    public static RmqSubscriptionBuilder CreateInfrastructureIfMissing(this RmqSubscriptionBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Create);
    
    public static RmqSubscriptionBuilder ValidIfInfrastructureExists(this RmqSubscriptionBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Validate);
    
    public static RmqSubscriptionBuilder AssumeInfrastructureExists(this RmqSubscriptionBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Assume);
    #endregion
    
    #region Durable
    public static RmqSubscriptionBuilder EnableDurable(this RmqSubscriptionBuilder builder)
        => builder.SetIsDurable(true);
    
    public static RmqSubscriptionBuilder DisableDurable(this RmqSubscriptionBuilder builder)
        => builder.SetIsDurable(false);
    #endregion
    
    #region Durable
    public static RmqSubscriptionBuilder EnableHighAvailability(this RmqSubscriptionBuilder builder)
        => builder.SethHighAvailability(true);
    
    public static RmqSubscriptionBuilder DisableHighAvailability(this RmqSubscriptionBuilder builder)
        => builder.SethHighAvailability(false);
    #endregion
}