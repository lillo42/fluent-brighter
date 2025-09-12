using System;

using Fluent.Brighter.Aws;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter;

public static class SnsPublicationBuilderExtensions
{
    #region Find Topic by
    public static SnsPublicationBuilder FindTopicByArn(this SnsPublicationBuilder builder)
        => builder.SetFindTopicBy(TopicFindBy.Arn);

    public static SnsPublicationBuilder FindTopicByConvention(this SnsPublicationBuilder builder)
        => builder.SetFindTopicBy(TopicFindBy.Convention);
    
    public static SnsPublicationBuilder FindTopicByName(this SnsPublicationBuilder builder)
        => builder.SetFindTopicBy(TopicFindBy.Name);
    #endregion

    #region Topic attribute
    public static SnsPublicationBuilder SetTopicAttributes(this SnsPublicationBuilder builder,
        Action<SnsAttributesBuilder> configure)
    {
        var attributes = new SnsAttributesBuilder();
        configure(attributes);
        return builder.SetTopicAttributes(attributes.Build());
    }
    #endregion
    
    #region Data schema
    public static SnsPublicationBuilder SetDataSchema(this SnsPublicationBuilder builder, string dataSchema)
        => builder.SetDataSchema(new Uri(dataSchema,  UriKind.RelativeOrAbsolute));
    #endregion
    
    #region Source 
    public static SnsPublicationBuilder SetSource(this SnsPublicationBuilder builder, string source)
        => builder.SetSource(new Uri(source,  UriKind.RelativeOrAbsolute));
    #endregion
    
    #region Make channels
    public static SnsPublicationBuilder CreateIfNotExists(this SnsPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Create);
    
    public static SnsPublicationBuilder ValidateIfNotExists(this SnsPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Validate);
    
    public static SnsPublicationBuilder AssumeIfNotExists(this SnsPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Assume);
    #endregion

}