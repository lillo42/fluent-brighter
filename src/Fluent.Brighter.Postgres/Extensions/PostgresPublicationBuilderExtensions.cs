using Fluent.Brighter.Postgres;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class PostgresPublicationBuilderExtensions
{
    #region MakeChannels
    public static PostgresPublicationBuilder CreateQueueIfMissing(this PostgresPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Create);
    
    public static PostgresPublicationBuilder ValidIfQueueExists(this PostgresPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Validate);
    
    public static PostgresPublicationBuilder AssumeQueueExists(this PostgresPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Assume);
    #endregion

    #region Binary Message Payload
    public static PostgresPublicationBuilder EnableBinaryMessagePayload(this PostgresPublicationBuilder builder)
        => builder.SetBinaryMessagePayload(true);
    
    public static PostgresPublicationBuilder DisableBinaryMessagePayload(this PostgresPublicationBuilder builder)
        => builder.SetBinaryMessagePayload(false);
    #endregion
}