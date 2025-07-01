using System;
using System.Collections.Generic;

using Paramore.Brighter;

namespace Fluent.Brighter.MsSql;

/// <summary>
/// A fluent builder for creating instances of <see cref="Publication"/>.
/// </summary>
public class MsSqlPublicationBuilder
{
    private OnMissingChannel _makeChannel = OnMissingChannel.Create;

    /// <summary>
    /// Configures the publisher to create exchanges if they don't exist.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public MsSqlPublicationBuilder CreateExchangeIfMissing()
    {
        return MakeExchange(OnMissingChannel.Create);
    }

    /// <summary>
    /// Configures the publisher to validate exchanges exist before publishing.
    /// Throws an exception if the exchange is missing.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public MsSqlPublicationBuilder ValidateIfExchangeExists()
    {
        return MakeExchange(OnMissingChannel.Validate);
    }

    /// <summary>
    /// Configures the publisher to assume exchanges exist without validation.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public MsSqlPublicationBuilder AssumeExchangeExists()
    {
        return MakeExchange(OnMissingChannel.Assume);
    }

    /// <summary>
    /// Sets the behavior when an exchange is missing.
    /// </summary>
    /// <param name="makeChannel">The <see cref="OnMissingChannel"/> mode to use.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public MsSqlPublicationBuilder MakeExchange(OnMissingChannel makeChannel)
    {
        _makeChannel = makeChannel;
        return this;
    }

    private RoutingKey? _topic;

    /// <summary>
    /// Sets the topic
    /// </summary>
    /// <param name="topic">The topic name</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public MsSqlPublicationBuilder Queue(RoutingKey topic)
    {
        _topic = topic;
        return this;
    }

    private Type? _requestType;

    /// <summary>
    /// Sets the request type
    /// </summary>
    /// <param name="type">The request type.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public MsSqlPublicationBuilder RequestType(Type? type)
    {
        _requestType = type;
        return this;
    }
    
    /// <summary>
    /// Sets the request type
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public MsSqlPublicationBuilder RequestType<TRequest>()
        where TRequest : class, IRequest
    {
        return RequestType(typeof(TRequest));
    }
    private Uri? _dataSchema;
    
    /// <summary>
    /// Sets the schema URI that data adheres to. Incompatible changes to the schema SHOULD be reflected by a different URI [[11]].
    /// </summary>
    /// <param name="dataSchema">The schema URI.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlPublicationBuilder DataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }

    private Uri _source = new("http://goparamore.io");

    /// <summary>
    /// Sets the source URI identifying the context where the event occurred [[11]].
    /// Default: "http://goparamore.io" for backward compatibility [[1]].
    /// </summary>
    /// <param name="source">The source URI.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlPublicationBuilder Source(Uri source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        return this;
    }

    private string? _subject;
    
    /// <summary>
    /// Sets the subject describing the event's context within the source [[11]].
    /// </summary>
    /// <param name="subject">The subject string.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlPublicationBuilder Subject(string? subject)
    {
        _subject = subject;
        return this;
    }

    private string _type = "goparamore.io.Paramore.Brighter.Message";

    /// <summary>
    /// Sets the event type for routing and policy enforcement [[11]].
    /// Default: "goparamore.io.Paramore.Brighter.Message" [[1]].
    /// </summary>
    /// <param name="type">The event type string.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlPublicationBuilder Type(string type)
    {
        _type = type ?? throw new ArgumentNullException(nameof(type));
        return this;
    }

    private IDictionary<string, object>? _cloudEventsAdditionalProperties;

    /// <summary>
    /// Sets additional properties for custom metadata in CloudEvents [[1]].
    /// This is used by <see cref="Paramore.Brighter.MessageMappers.CloudEventJsonMessageMapper{T}"/> [[2]].
    /// </summary>
    /// <param name="properties">Dictionary of additional properties.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlPublicationBuilder CloudEventsAdditionalProperties(IDictionary<string, object>? properties)
    {
        _cloudEventsAdditionalProperties = properties;
        return this;
    }

    private string? _replyTo;
    
    /// <summary>
    /// Sets the reply-to queue for request-reply patterns [[1]].
    /// </summary>
    /// <param name="replyTo">The reply-to queue name.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlPublicationBuilder ReplyTo(string? replyTo)
    {
        _replyTo = replyTo;
        return this;
    } 

    /// <summary>
    /// Create a new instance of <see cref="Publication"/> 
    /// </summary>
    /// <returns></returns>
    public Publication Build()
    {
        return new Publication
        {
            CloudEventsAdditionalProperties = _cloudEventsAdditionalProperties,
            DataSchema = _dataSchema,
            MakeChannels = _makeChannel,
            ReplyTo = _replyTo,
            RequestType = _requestType,
            Subject = _subject,
            Source = _source,
            Topic = _topic,
            Type = _type,
        };
    }
}