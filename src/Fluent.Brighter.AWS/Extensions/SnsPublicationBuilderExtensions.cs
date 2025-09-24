using System;

using Fluent.Brighter.AWS;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for SnsPublicationBuilder to provide fluent configuration
/// for topic discovery methods, attributes, data schema, source URI, and channel creation behavior.
/// </summary>
public static class SnsPublicationBuilderExtensions
{
    #region Find Topic by
    /// <summary>
    /// Configures the publication to find topics by their Amazon Resource Name (ARN).
    /// </summary>
    /// <param name="builder">The SNS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SnsPublicationBuilder FindTopicByArn(this SnsPublicationBuilder builder)
    {
        return builder.SetFindTopicBy(TopicFindBy.Arn);
    }

    /// <summary>
    /// Configures the publication to find topics by naming convention.
    /// </summary>
    /// <param name="builder">The SNS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SnsPublicationBuilder FindTopicByConvention(this SnsPublicationBuilder builder)
    {
        return builder.SetFindTopicBy(TopicFindBy.Convention);
    }

    /// <summary>
    /// Configures the publication to find topics by their name.
    /// </summary>
    /// <param name="builder">The SNS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SnsPublicationBuilder FindTopicByName(this SnsPublicationBuilder builder)
    {
        return builder.SetFindTopicBy(TopicFindBy.Name);
    }
    #endregion

    #region Topic attribute

    /// <summary>
    /// Sets topic attributes using a builder pattern for fluent configuration.
    /// </summary>
    /// <param name="builder">The SNS publication builder instance</param>
    /// <param name="configure">Action to configure SNS topic attributes</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SnsPublicationBuilder SetTopicAttributes(this SnsPublicationBuilder builder,
        Action<SnsAttributesBuilder> configure)
    {
        var attributes = new SnsAttributesBuilder();
        configure(attributes);
        return builder.SetTopicAttributes(attributes.Build());
    }
    #endregion

    #region Data schema

    /// <summary>
    /// Sets the data schema URI for CloudEvents metadata using a string URI.
    /// </summary>
    /// <param name="builder">The SNS publication builder instance</param>
    /// <param name="dataSchema">The data schema URI as a string</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SnsPublicationBuilder SetDataSchema(this SnsPublicationBuilder builder, string dataSchema)
    {
        return builder.SetDataSchema(new Uri(dataSchema, UriKind.RelativeOrAbsolute));
    }
    #endregion

    #region Source 

    /// <summary>
    /// Sets the source URI for CloudEvents metadata using a string URI.
    /// </summary>
    /// <param name="builder">The SNS publication builder instance</param>
    /// <param name="source">The source URI as a string</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SnsPublicationBuilder SetSource(this SnsPublicationBuilder builder, string source)
    {
        return builder.SetSource(new Uri(source, UriKind.RelativeOrAbsolute));
    }
    #endregion

    #region Make channels

    /// <summary>
    /// Configures the publication to create topics if they don't exist.
    /// </summary>
    /// <param name="builder">The SNS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SnsPublicationBuilder CreateTopicIfMissing(this SnsPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Create);
    }

    /// <summary>
    /// Configures the publication to validate topic existence and throw an exception if not found.
    /// </summary>
    /// <param name="builder">The SNS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SnsPublicationBuilder ValidateIfNotExists(this SnsPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Validate);
    }

    /// <summary>
    /// Configures the publication to assume topics exist (no validation or creation).
    /// </summary>
    /// <param name="builder">The SNS publication builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SnsPublicationBuilder AssumeIfNotExists(this SnsPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Assume);
    }
    #endregion
}