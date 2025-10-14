using System;
using System.Collections.Generic;

using Paramore.Brighter;

namespace Fluent.Brighter.SqlServer;

/// <summary>
/// Provides a fluent builder for configuring a <see cref="Publication"/> used when publishing messages
/// through Microsoft SQL Server-based outbox or messaging infrastructure in the Fluent Brighter library.
/// </summary>
public sealed class SqlServerPublicationBuilder
{
    private Uri? _dataSchema;

    /// <summary>
    /// Sets the URI identifying the schema that the event data adheres to.
    /// Corresponds to the <c>dataSchema</c> attribute in CloudEvents.
    /// </summary>
    /// <param name="dataSchema">The schema URI, or <see langword="null"/> if not applicable.</param>
    /// <returns>The current instance of <see cref="SqlServerPublicationBuilder"/> to allow method chaining.</returns>
    public SqlServerPublicationBuilder SetDataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }

    private OnMissingChannel _makeChannels = OnMissingChannel.Create;

    /// <summary>
    /// Specifies the behavior when a required messaging channel (e.g., topic or queue) does not exist.
    /// </summary>
    /// <param name="makeChannels">The policy for handling missing channels. Defaults to <see cref="OnMissingChannel.Create"/>.</param>
    /// <returns>The current instance of <see cref="SqlServerPublicationBuilder"/> to allow method chaining.</returns>
    public SqlServerPublicationBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }

    private Type? _requestType;

    /// <summary>
    /// Sets the .NET <see cref="Type"/> of the request/command that triggers this publication.
    /// Used for routing and correlation within Brighter’s messaging pipeline.
    /// </summary>
    /// <param name="requestType">The request type, or <see langword="null"/> if unspecified.</param>
    /// <returns>The current instance of <see cref="SqlServerPublicationBuilder"/> to allow method chaining.</returns>
    public SqlServerPublicationBuilder SetRequestType(Type? requestType)
    {
        _requestType = requestType;
        return this;
    }

    private Uri _source = new(MessageHeader.DefaultSource);

    /// <summary>
    /// Sets the source URI identifying the context in which the event occurred.
    /// Corresponds to the <c>source</c> attribute in CloudEvents.
    /// </summary>
    /// <param name="source">The event source URI. Must not be <see langword="null"/>.</param>
    /// <returns>The current instance of <see cref="SqlServerPublicationBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is <see langword="null"/>.</exception>
    public SqlServerPublicationBuilder SetSource(Uri source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        return this;
    }

    private string? _subject;

    /// <summary>
    /// Sets an optional subject that describes the specific entity or resource related to the event.
    /// Corresponds to the <c>subject</c> attribute in CloudEvents.
    /// </summary>
    /// <param name="subject">The event subject, or <see langword="null"/> if not applicable.</param>
    /// <returns>The current instance of <see cref="SqlServerPublicationBuilder"/> to allow method chaining.</returns>
    public SqlServerPublicationBuilder SetSubject(string? subject)
    {
        _subject = subject;
        return this;
    }

    private RoutingKey? _topic;

    /// <summary>
    /// Sets the routing key (or topic/queue name) used to route the published message.
    /// </summary>
    /// <param name="topic">The routing key, or <see langword="null"/> if using default routing.</param>
    /// <returns>The current instance of <see cref="SqlServerPublicationBuilder"/> to allow method chaining.</returns>
    /// <remarks>
    /// Despite the method name <c>SetQueue</c>, this property typically represents a topic or routing key
    /// in publish-subscribe scenarios. The name reflects legacy Brighter terminology.
    /// </remarks>
    public SqlServerPublicationBuilder SetQueue(RoutingKey? topic)
    {
        _topic = topic;
        return this;
    }

    private CloudEventsType _type = CloudEventsType.Empty;

    /// <summary>
    /// Sets the event type, which describes the nature or category of the event.
    /// Corresponds to the <c>type</c> attribute in CloudEvents.
    /// </summary>
    /// <param name="type">The CloudEvents type. Should not be empty in most real-world scenarios.</param>
    /// <returns>The current instance of <see cref="SqlServerPublicationBuilder"/> to allow method chaining.</returns>
    public SqlServerPublicationBuilder SetType(CloudEventsType type)
    {
        _type = type;
        return this;
    }

    private IDictionary<string, object>? _defaultHeaders;

    /// <summary>
    /// Sets default message headers that will be applied to all messages published using this configuration.
    /// </summary>
    /// <param name="defaultHeaders">A dictionary of header name-value pairs, or <see langword="null"/> for none.</param>
    /// <returns>The current instance of <see cref="SqlServerPublicationBuilder"/> to allow method chaining.</returns>
    public SqlServerPublicationBuilder SetDefaultHeaders(IDictionary<string, object>? defaultHeaders)
    {
        _defaultHeaders = defaultHeaders;
        return this;
    }

    private IDictionary<string, object>? _cloudEventsAdditionalProperties;

    /// <summary>
    /// Sets additional CloudEvents extension attributes to include in the published event.
    /// </summary>
    /// <param name="cloudEventsAdditionalProperties">A dictionary of extension name-value pairs, or <see langword="null"/> for none.</param>
    /// <returns>The current instance of <see cref="SqlServerPublicationBuilder"/> to allow method chaining.</returns>
    public SqlServerPublicationBuilder SetCloudEventsAdditionalProperties(
        IDictionary<string, object>? cloudEventsAdditionalProperties)
    {
        _cloudEventsAdditionalProperties = cloudEventsAdditionalProperties;
        return this;
    }

    private string? _replyTo;

    /// <summary>
    /// Sets the reply-to address for request-reply messaging patterns.
    /// </summary>
    /// <param name="replyTo">The reply queue or address, or <see langword="null"/> if not used.</param>
    /// <returns>The current instance of <see cref="SqlServerPublicationBuilder"/> to allow method chaining.</returns>
    public SqlServerPublicationBuilder SetReplyTo(string? replyTo)
    {
        _replyTo = replyTo;
        return this;
    }

    /// <summary>
    /// Builds and returns a configured <see cref="Publication"/> instance.
    /// </summary>
    /// <returns>A fully configured <see cref="Publication"/> object.</returns>
    internal Publication Build()
    {
        return new Publication
        {
            DataSchema = _dataSchema,
            MakeChannels = _makeChannels,
            RequestType = _requestType,
            Source = _source,
            Subject = _subject,
            Topic = _topic,
            Type = _type,
            DefaultHeaders = _defaultHeaders,
            CloudEventsAdditionalProperties = _cloudEventsAdditionalProperties,
            ReplyTo = _replyTo,
        };
    }
}