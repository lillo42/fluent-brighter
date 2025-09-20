using System;

using Fluent.Brighter.Aws;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for SqsPublicationBuilder to provide fluent configuration
/// for channel types, queue discovery methods, attributes, data schema, source URI,
/// and channel creation behavior in Amazon SQS.
/// </summary>
public static class SqsPublicationBuilderExtensions
{
    #region Channel Type

    /// <summary>
    /// Configures the publication to use point-to-point messaging pattern (standard queue behavior).
    /// </summary>
    /// <param name="builder">The SQS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsPublicationBuilder UsePointToPoint(this SqsPublicationBuilder builder)
        => builder.SetChannelType(ChannelType.PointToPoint);

    /// <summary>
    /// Configures the publication to use publish-subscribe messaging pattern (SNS-to-SQS integration).
    /// </summary>
    /// <param name="builder">The SQS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsPublicationBuilder UsePubSub(this SqsPublicationBuilder builder)
        => builder.SetChannelType(ChannelType.PubSub);
    #endregion

    #region Find Queue By
    
    /// <summary>
    /// Configures the publication to find queues by their name.
    /// </summary>
    /// <param name="builder">The SQS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsPublicationBuilder FindQueueByName(this SqsPublicationBuilder builder)
        => builder.SetFindQueueBy(QueueFindBy.Name);
    
    /// <summary>
    /// Configures the publication to find queues by their URL.
    /// </summary>
    /// <param name="builder">The SQS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsPublicationBuilder FindQueueByUrl(this SqsPublicationBuilder builder)
        => builder.SetFindQueueBy(QueueFindBy.Url);
    #endregion

    #region Queue attribute
    
    /// <summary>
    /// Sets queue attributes using a builder pattern for fluent configuration.
    /// </summary>
    /// <param name="builder">The SQS publication builder instance</param>
    /// <param name="configure">Action to configure SQS queue attributes</param>
    /// <returns>The builder instance for method chaining</returns> 
    public static SqsPublicationBuilder SetQueueAttributes(this SqsPublicationBuilder builder,
        Action<SqsAttributesBuilder> configure)
    {
        var attributes = new SqsAttributesBuilder();
        configure(attributes);
        return builder.SetQueueAttributes(attributes.Build());
    }
    #endregion

    #region Data schema
    
    /// <summary>
    /// Configures the publication to find queues by their name.
    /// </summary>
    /// <param name="builder">The SQS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsPublicationBuilder SetDataSchema(this SqsPublicationBuilder builder, string dataSchema)
        => builder.SetDataSchema(new Uri(dataSchema,  UriKind.RelativeOrAbsolute));
    #endregion
    
    #region Make channels
    
    /// <summary>
    /// Configures the publication to create queues if they don't exist.
    /// </summary>
    /// <param name="builder">The SQS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsPublicationBuilder CreateIfMissing(this SqsPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Create);
    
    /// <summary>
    /// Configures the publication to validate queue existence and throw an exception if not found.
    /// </summary>
    /// <param name="builder">The SQS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsPublicationBuilder ValidateIfNotExists(this SqsPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Validate);
    
    /// <summary>
    /// Configures the publication to assume queues exist (no validation or creation).
    /// </summary>
    /// <param name="builder">The SQS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsPublicationBuilder AssumeIfNotExists(this SqsPublicationBuilder builder)
        => builder.SetMakeChannels(OnMissingChannel.Assume);
    #endregion
    
    #region Source 
    
    /// <summary>
    /// Sets the source URI for CloudEvents metadata using a string URI.
    /// </summary>
    /// <param name="builder">The SQS publication builder instance</param>
    /// <param name="source">The source URI as a string</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SqsPublicationBuilder SetSource(this SqsPublicationBuilder builder, string source)
        => builder.SetSource(new Uri(source,  UriKind.RelativeOrAbsolute));
    #endregion
}