using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter.Postgres;

/// <summary>
/// Builder class for configuring and creating a PostgreSQL publication.
/// A publication defines how messages are published to PostgreSQL, including topic mappings,
/// message metadata, CloudEvents properties, and database-specific settings.
/// </summary>
public sealed class PostgresPublicationBuilder
{
    private Uri? _dataSchema;

    /// <summary>
    /// Sets the data schema URI for CloudEvents messages.
    /// This defines the schema that describes the structure of the message data.
    /// </summary>
    /// <param name="dataSchema">The URI pointing to the data schema definition, or null if no schema is defined.</param>
    /// <returns>The current <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public PostgresPublicationBuilder SetDataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }

    private OnMissingChannel _makeChannels = OnMissingChannel.Create; 

    /// <summary>
    /// Sets the behavior for handling missing channels/topics in PostgreSQL.
    /// Determines whether to create, validate, or assume the existence of channels.
    /// </summary>
    /// <param name="makeChannels">The <see cref="OnMissingChannel"/> behavior to apply. Default is <see cref="OnMissingChannel.Create"/>.</param>
    /// <returns>The current <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public PostgresPublicationBuilder SetMakeChannels(OnMissingChannel makeChannels)
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
    /// <returns>The current <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public PostgresPublicationBuilder SetRequestType(Type? requestType)
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
    /// <returns>The current <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public PostgresPublicationBuilder SetSource(Uri source)
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
    /// <returns>The current <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public PostgresPublicationBuilder SetSubject(string? subject)
    {
        _subject = subject;
        return this;
    }

    private RoutingKey? _topic;

    /// <summary>
    /// Sets the routing key (topic/queue name) where messages will be published in PostgreSQL.
    /// This determines the destination topic for outgoing messages.
    /// </summary>
    /// <param name="topic">The routing key identifying the PostgreSQL topic/queue, or null if not specified.</param>
    /// <returns>The current <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public PostgresPublicationBuilder SetQueue(RoutingKey? topic)
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
    /// <returns>The current <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public PostgresPublicationBuilder SetType(CloudEventsType type)
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
    /// <returns>The current <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public PostgresPublicationBuilder SetDefaultHeaders(IDictionary<string, object>? defaultHeaders)
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
    /// <returns>The current <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public PostgresPublicationBuilder SetCloudEventsAdditionalProperties(
        IDictionary<string, object>? cloudEventsAdditionalProperties)
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
    /// <returns>The current <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public PostgresPublicationBuilder SetReplyTo(string? replyTo)
    {
        _replyTo = replyTo;
        return this;
    }


    private string? _schemaName;

    /// <summary>
    /// Sets the PostgreSQL database schema name where the message queue tables are located.
    /// This allows you to organize queue tables in a specific database schema.
    /// </summary>
    /// <param name="schemaName">The PostgreSQL schema name, or null to use the default schema.</param>
    /// <returns>The current <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public PostgresPublicationBuilder SetSchemaName(string? schemaName)
    {
        _schemaName = schemaName;
        return this;
    }

    private string? _queueStoreTable;

    /// <summary>
    /// Sets the PostgreSQL table name to use for storing messages in the queue.
    /// This allows you to customize the table name where messages are persisted.
    /// </summary>
    /// <param name="queueStoreTable">The PostgreSQL table name for the queue store, or null to use the default table name.</param>
    /// <returns>The current <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public PostgresPublicationBuilder SetQueueStoreTable(string? queueStoreTable)
    {
        _queueStoreTable = queueStoreTable;
        return this;
    }

    private bool? _binaryMessagePayload;

    /// <summary>
    /// Sets whether to store message payloads in binary format in PostgreSQL.
    /// When enabled, message payloads are stored as binary data (bytea) instead of text, which can be more efficient for certain message types.
    /// </summary>
    /// <param name="binaryMessagePayload">True to use binary format, false for text format, or null to use the default setting.</param>
    /// <returns>The current <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public PostgresPublicationBuilder SetBinaryMessagePayload(bool? binaryMessagePayload)
    {
        _binaryMessagePayload = binaryMessagePayload;
        return this;
    }

    /// <summary>
    /// Builds and returns a configured <see cref="PostgresPublication"/> instance.
    /// This method is called internally to create the publication with all the configured settings
    /// including CloudEvents properties, routing information, and PostgreSQL-specific options.
    /// </summary>
    /// <returns>A configured <see cref="PostgresPublication"/> instance.</returns>
    internal PostgresPublication Build()
    {
        return new PostgresPublication
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
            SchemaName = _schemaName,
            QueueStoreTable = _queueStoreTable,
            BinaryMessagePayload = _binaryMessagePayload
        };
    }
}