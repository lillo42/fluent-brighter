using Fluent.Brighter.Postgres;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class PostgresSubscriptionBuilderExtensions
{
    #region Message Pump
    public static PostgresSubscriptionBuilder UseProactor(this PostgresSubscriptionBuilder builder)
        => builder.SetMessagePumpType(MessagePumpType.Proactor);
    
    public static PostgresSubscriptionBuilder UseReactor(this PostgresSubscriptionBuilder builder)
        => builder.SetMessagePumpType(MessagePumpType.Reactor);
    #endregion
    
    #region MakeChannels
    public static PostgresSubscriptionBuilder CreateQueueIfMissing(this PostgresSubscriptionBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Create);
    
    public static PostgresSubscriptionBuilder ValidIfQueueExists(this PostgresSubscriptionBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Validate);
    
    public static PostgresSubscriptionBuilder AssumeQueueExists(this PostgresSubscriptionBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Assume);
    #endregion
    
    #region Binary Message Payload
    public static PostgresSubscriptionBuilder EnableBinaryMessagePayload(this PostgresSubscriptionBuilder builder)
        => builder.SetBinaryMessagePayload(true);
    
    public static PostgresSubscriptionBuilder DisableBinaryMessagePayload(this PostgresSubscriptionBuilder builder)
        => builder.SetBinaryMessagePayload(false);
    #endregion
    
    #region Table with large message
    public static PostgresSubscriptionBuilder EnableTableWithLargeMessage(this PostgresSubscriptionBuilder builder)
        => builder.SetTableWithLargeMessage(true);
    
    public static PostgresSubscriptionBuilder DisableTableWithLargeMessage(this PostgresSubscriptionBuilder builder)
        => builder.SetTableWithLargeMessage(false);
    #endregion
}