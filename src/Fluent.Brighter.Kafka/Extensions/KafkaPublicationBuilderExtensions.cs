using Fluent.Brighter.Kafka;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class KafkaPublicationBuilderExtensions
{
    #region MakeChannels
    public static KafkaPublicationBuilder CreateTopicIfMissing(this KafkaPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Create);
    
    public static KafkaPublicationBuilder ValidIfTopicExists(this KafkaPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Validate);
    
    public static KafkaPublicationBuilder AssumeTopicExists(this KafkaPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Assume);
    #endregion
}