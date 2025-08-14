using Fluent.Brighter.Kafka;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class KafkaSubscriptionBuilderExtensions
{
    #region Message Pump
    public static KafkaSubscriptionBuilder UseProactorMode(this KafkaSubscriptionBuilder builder)
        => builder.SetMessagePumpType(MessagePumpType.Proactor);
    
    public static KafkaSubscriptionBuilder UseReactorMode(this KafkaSubscriptionBuilder builder)
        => builder.SetMessagePumpType(MessagePumpType.Reactor);
    #endregion
    
    #region MakeChannels
    public static KafkaSubscriptionBuilder CreateInfrastructureIfMissing(this KafkaSubscriptionBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Create);
    
    public static KafkaSubscriptionBuilder ValidIfInfrastructureExists(this KafkaSubscriptionBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Validate);
    
    public static KafkaSubscriptionBuilder AssumeInfrastructureExists(this KafkaSubscriptionBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Assume);
    #endregion
}