using System;

using Fluent.Brighter.Aws;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter;

public static class SqsPublicationBuilderExtensions
{
    #region Channel Type

    public static SqsPublicationBuilder UsePointToPoint(this SqsPublicationBuilder builder)
        => builder.SetChannelType(ChannelType.PointToPoint);

    public static SqsPublicationBuilder UsePubSub(this SqsPublicationBuilder builder)
        => builder.SetChannelType(ChannelType.PubSub);
    #endregion

    #region Find Queue By
    public static SqsPublicationBuilder FindQueueByName(this SqsPublicationBuilder builder)
        => builder.SetFindQueueBy(QueueFindBy.Name);
    
    public static SqsPublicationBuilder FindQueueByUrl(this SqsPublicationBuilder builder)
        => builder.SetFindQueueBy(QueueFindBy.Url);
    #endregion

    #region Queue attribute
    public static SqsPublicationBuilder SetQueueAttributes(this SqsPublicationBuilder builder,
        Action<SqsAttributesBuilder> configure)
    {
        var attributes = new SqsAttributesBuilder();
        configure(attributes);
        return builder.SetQueueAttributes(attributes.Build());
    }
    #endregion

    #region Data schema
    public static SqsPublicationBuilder SetDataSchema(this SqsPublicationBuilder builder, string dataSchema)
        => builder.SetDataSchema(new Uri(dataSchema,  UriKind.RelativeOrAbsolute));
    #endregion
    
    #region Make channels
    public static SqsPublicationBuilder CreateIfNotExists(this SqsPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Create);
    
    public static SqsPublicationBuilder ValidateIfNotExists(this SqsPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Validate);
    
    public static SqsPublicationBuilder AssumeIfNotExists(this SqsPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Assume);
    #endregion
    
    #region Source 
    public static SqsPublicationBuilder SetSource(this SqsPublicationBuilder builder, string source)
        => builder.SetSource(new Uri(source,  UriKind.RelativeOrAbsolute));
    #endregion
}