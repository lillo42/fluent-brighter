using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Redis;

namespace Fluent.Brighter.Redis;

/// <summary>
/// Builder class for configuring and creating a Redis publication.
/// A publication defines how messages are published to Redis, including topic mappings,
/// message metadata, CloudEvents properties, and Redis-specific settings.
/// </summary>
public sealed class RedisPublicationBuilder
{
    private Uri? _dataSchema;
    
    /// <summary>
    /// Sets the data schema URI for CloudEvents messages.
    /// This defines the schema that describes the structure of the message data.
    /// </summary>
    /// <param name="dataSchema">The URI pointing to the data schema definition, or null if no schema is defined.</param>
    /// <returns>The current <see cref="RedisPublicationBuilder"/> instance for method chaining.</returns>
    public RedisPublicationBuilder SetDataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }
    
    private OnMissingChannel _makeChannels = OnMissingChannel.Create;
    
    /// <summary>
    /// Sets the behavior for handling missing channels/topics in Redis.
    /// Determines whether to create, validate, or assume the existence of channels.
    /// </summary>
    /// <param name="makeChannels">The <see cref="OnMissingChannel"/> behavior to apply. Default is <see cref="OnMissingChannel.Create"/>.</param>
    /// <returns>The current <see cref="RedisPublicationBuilder"/> instance for method chaining.</returns>
    public RedisPublicationBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }
    
    private Type? _requestType;
    
    /// <summary>
    /// Sets the request type for the publication.
    /// This defines the .NET type of the message/command/event that will be published.
    /// </summary>
    /// <param name="requestType">The <see cref="Type"/> of the request, or null if not specified.</param>
    /// <returns>The current <see cref="RedisPublicationBuilder"/> instance for method chaining.</returns>
    public RedisPublicationBuilder SetRequestType(Type? requestType)
    {
        _requestType = requestType;
        return this;
    }
    
    private Uri _source = new(MessageHeader.DefaultSource);
    
    /// <summary>
    /// Sets the source URI for CloudEvents messages.
    /// This identifies the context in which an event happened and is part of the CloudEvents specification.
    /// </summary>
    /// <param name="source">The URI identifying the source of the event. Default is <see cref="MessageHeader.DefaultSource"/>.</param>
    /// <returns>The current <see cref="RedisPublicationBuilder"/> instance for method chaining.</returns>
    public RedisPublicationBuilder SetSource(Uri source)
    {
        _source = source;
        return this;
    }
    
    private string? _subject;
    
    /// <summary>
    /// Sets the subject for CloudEvents messages.
    /// This describes the subject of the event in the context of the event producer and is part of the CloudEvents specification.
    /// </summary>
    /// <param name="subject">The subject of the event, or null if not specified.</param>
    /// <returns>The current <see cref="RedisPublicationBuilder"/> instance for method chaining.</returns>
    public RedisPublicationBuilder SetSubject(string? subject)
    {
        _subject = subject;
        return this;
    }
    
    private RoutingKey? _topic;
    
    /// <summary>
    /// Sets the routing key (topic/channel name) where messages will be published in Redis.
    /// This determines the destination topic for outgoing messages.
    /// </summary>
    /// <param name="topic">The routing key identifying the Redis topic/channel, or null if not specified.</param>
    /// <returns>The current <see cref="RedisPublicationBuilder"/> instance for method chaining.</returns>
    public RedisPublicationBuilder SetTopic(RoutingKey? topic)
    {
        _topic = topic;
        return this;
    }
    
    private CloudEventsType _type = CloudEventsType.Empty;
    
    /// <summary>
    /// Sets the CloudEvents type for messages.
    /// This describes the type of event related to the originating occurrence and is part of the CloudEvents specification.
    /// </summary>
    /// <param name="type">The <see cref="CloudEventsType"/> to apply. Default is <see cref="CloudEventsType.Empty"/>.</param>
    /// <returns>The current <see cref="RedisPublicationBuilder"/> instance for method chaining.</returns>
    public RedisPublicationBuilder SetType(CloudEventsType type)
    {
        _type = type;
        return this;
    }
    
    private IDictionary<string, object>? _defaultHeaders;
    
    /// <summary>
    /// Sets default headers to be included with every published message.
    /// These headers provide additional metadata that will be sent along with messages.
    /// </summary>
    /// <param name="defaultHeaders">A dictionary of default header key-value pairs, or null if no default headers are needed.</param>
    /// <returns>The current <see cref="RedisPublicationBuilder"/> instance for method chaining.</returns>
    public RedisPublicationBuilder SetDefaultHeaders(IDictionary<string, object>? defaultHeaders)
    {
        _defaultHeaders = defaultHeaders;
        return this;
    }
    
    private IDictionary<string, object>? _cloudEventsAdditionalProperties;
    
    /// <summary>
    /// Sets additional CloudEvents extension properties to be included with published messages.
    /// CloudEvents allows custom extension attributes beyond the standard specification to provide additional context.
    /// </summary>
    /// <param name="cloudEventsAdditionalProperties">A dictionary of additional CloudEvents properties, or null if no additional properties are needed.</param>
    /// <returns>The current <see cref="RedisPublicationBuilder"/> instance for method chaining.</returns>
    /// <remarks>
    /// These properties are serialized alongside the core CloudEvents attributes when mapping to a CloudEvent message.
    /// If any key conflicts with standard CloudEvents JSON properties (e.g., "id", "source", "type"),
    /// the serializer will prioritize the value in this dictionary, potentially overriding standard properties.
    /// Exercise caution to avoid unintended overwrites of core CloudEvents attributes.
    /// </remarks>
    public RedisPublicationBuilder SetCloudEventsAdditionalProperties(IDictionary<string, object>? cloudEventsAdditionalProperties)
    {
        _cloudEventsAdditionalProperties = cloudEventsAdditionalProperties;
        return this;
    }
    
    private string? _replyTo;
    
    /// <summary>
    /// Sets the reply-to address for request-reply messaging patterns.
    /// This specifies where response messages should be sent when implementing request-reply communication.
    /// </summary>
    /// <param name="replyTo">The reply-to address/topic name, or null if no reply is expected.</param>
    /// <returns>The current <see cref="RedisPublicationBuilder"/> instance for method chaining.</returns>
    public RedisPublicationBuilder SetReplyTo(string? replyTo)
    {
        _replyTo = replyTo;
        return this;
    }
    
    /// <summary>
    /// Builds and returns a configured <see cref="RedisMessagePublication"/> instance.
    /// This method is called internally to create the publication with all the configured settings
    /// including CloudEvents properties, routing information, and Redis-specific options.
    /// </summary>
    /// <returns>A configured <see cref="RedisMessagePublication"/> instance.</returns>
    internal RedisMessagePublication Build()
    {
        return new RedisMessagePublication
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
            ReplyTo = _replyTo
        };
    }
}