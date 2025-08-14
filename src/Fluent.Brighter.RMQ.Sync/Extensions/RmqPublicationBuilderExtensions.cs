using Fluent.Brighter.RMQ.Sync;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class RmqPublicationBuilderExtensions
{
    #region MakeChannels
    public static RmqPublicationBuilder CreateTopicIfMissing(this RmqPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Create);
    
    public static RmqPublicationBuilder ValidIfTopicExists(this RmqPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Validate);
    
    public static RmqPublicationBuilder AssumeTopicExists(this RmqPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Assume);
    #endregion
}