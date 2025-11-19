using System;

using Fluent.Brighter.GoogleCloud;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring Google Cloud Pub/Sub publication settings using a fluent API.
/// </summary>
public static class GcpPublicationBuilderExtensions
{
    /// <summary>
    /// Configures the Google Cloud Pub/Sub topic attributes using a fluent builder.
    /// This allows setting properties like project ID, topic name, labels, retention duration,
    /// schema settings, and encryption configuration.
    /// </summary>
    /// <param name="builder">The GCP publication builder instance</param>
    /// <param name="configure">An action to configure the topic attributes using the builder</param>
    /// <returns>The configured <see cref="GcpPublicationBuilder"/> instance for method chaining</returns>
    /// <example>
    /// <code>
    /// builder.SetTopicAttributes(attrs =>
    /// {
    ///     attrs.SetName("my-topic")
    ///          .SetProjectId("my-project")
    ///          .AddLabel("environment", "production");
    /// });
    /// </code>
    /// </example>
    public static GcpPublicationBuilder SetTopicAttributes(this GcpPublicationBuilder builder,
        Action<TopicAttributeBuilder> configure)
    {
        var attrs = new TopicAttributeBuilder();
        configure(attrs);
        return builder.SetTopicAttributes(attrs.Build());
    }

    /// <summary>
    /// Sets the data schema URI from a string value for CloudEvents metadata.
    /// Identifies the schema that data adheres to. If the string is null or empty, sets the data schema to null.
    /// </summary>
    /// <param name="builder">The GCP publication builder instance</param>
    /// <param name="dataSchema">The data schema URI as a string, or null</param>
    /// <returns>The configured <see cref="GcpPublicationBuilder"/> instance for method chaining</returns>
    public static GcpPublicationBuilder SetDataSchema(this GcpPublicationBuilder builder, string? dataSchema)
    {
        if (string.IsNullOrEmpty(dataSchema))
        {
            return builder.SetDataSchema(null);
        }

        return builder.SetDataSchema(new Uri(dataSchema, UriKind.RelativeOrAbsolute));
    }

    /// <summary>
    /// Sets the source URI from a string value for CloudEvents metadata.
    /// Identifies the context in which an event happened, such as the event source organization or process.
    /// </summary>
    /// <param name="builder">The GCP publication builder instance</param>
    /// <param name="dataSchema">The source URI as a string</param>
    /// <returns>The configured <see cref="GcpPublicationBuilder"/> instance for method chaining</returns>
    public static GcpPublicationBuilder SetSource(this GcpPublicationBuilder builder, string dataSchema)
    {
        return builder.SetSource(new Uri(dataSchema, UriKind.RelativeOrAbsolute));
    }

    /// <summary>
    /// Configures the publication to create the Pub/Sub topic if it doesn't exist.
    /// This sets the make channels policy to <see cref="OnMissingChannel.Create"/>.
    /// </summary>
    /// <param name="builder">The GCP publication builder instance</param>
    /// <returns>The configured <see cref="GcpPublicationBuilder"/> instance for method chaining</returns>
    public static GcpPublicationBuilder CreateQueueIfMissing(this GcpPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Create);
    }

    /// <summary>
    /// Configures the publication to validate that the Pub/Sub topic exists without creating it.
    /// This sets the make channels policy to <see cref="OnMissingChannel.Validate"/>.
    /// If the topic doesn't exist, an error will be raised.
    /// </summary>
    /// <param name="builder">The GCP publication builder instance</param>
    /// <returns>The configured <see cref="GcpPublicationBuilder"/> instance for method chaining</returns>
    public static GcpPublicationBuilder ValidIfQueueExists(this GcpPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Validate);
    }

    /// <summary>
    /// Configures the publication to assume the Pub/Sub topic exists without validation or creation.
    /// This sets the make channels policy to <see cref="OnMissingChannel.Assume"/>.
    /// Use this when the topic is managed externally or already known to exist.
    /// </summary>
    /// <param name="builder">The GCP publication builder instance</param>
    /// <returns>The configured <see cref="GcpPublicationBuilder"/> instance for method chaining</returns>
    public static GcpPublicationBuilder AssumeQueueExists(this GcpPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Assume);
    }
}
