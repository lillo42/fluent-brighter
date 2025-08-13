using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter.Postgres;

public sealed class PostgresPublicationBuilder
{
    private Uri? _dataSchema;

    public PostgresPublicationBuilder SetDataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }

    private OnMissingChannel _makeChannels = OnMissingChannel.Create; // Default value from Publication

    public PostgresPublicationBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }


    private Type? _requestType;

    public PostgresPublicationBuilder SetRequestType(Type? requestType)
    {
        _requestType = requestType;
        return this;
    }


    private Uri _source = new(MessageHeader.DefaultSource);

    public PostgresPublicationBuilder SetSource(Uri source)
    {
        _source = source;
        return this;
    }

    private string? _subject;

    public PostgresPublicationBuilder SetSubject(string? subject)
    {
        _subject = subject;
        return this;
    }

    private RoutingKey? _topic;

    public PostgresPublicationBuilder SetTopic(RoutingKey? topic)
    {
        _topic = topic;
        return this;
    }

    private CloudEventsType _type = CloudEventsType.Empty;

    public PostgresPublicationBuilder SetType(CloudEventsType type)
    {
        _type = type;
        return this;
    }

    private IDictionary<string, object>? _defaultHeaders;

    public PostgresPublicationBuilder SetDefaultHeaders(IDictionary<string, object>? defaultHeaders)
    {
        _defaultHeaders = defaultHeaders;
        return this;
    }


    private IDictionary<string, object>? _cloudEventsAdditionalProperties;

    public PostgresPublicationBuilder SetCloudEventsAdditionalProperties(
        IDictionary<string, object>? cloudEventsAdditionalProperties)
    {
        _cloudEventsAdditionalProperties = cloudEventsAdditionalProperties;
        return this;
    }

    private string? _replyTo;

    public PostgresPublicationBuilder SetReplyTo(string? replyTo)
    {
        _replyTo = replyTo;
        return this;
    }


    private string? _schemaName;

    public PostgresPublicationBuilder SetSchemaName(string? schemaName)
    {
        _schemaName = schemaName;
        return this;
    }

    private string? _queueStoreTable;

    public PostgresPublicationBuilder SetQueueStoreTable(string? queueStoreTable)
    {
        _queueStoreTable = queueStoreTable;
        return this;
    }

    private bool? _binaryMessagePayload;

    public PostgresPublicationBuilder SetBinaryMessagePayload(bool? binaryMessagePayload)
    {
        _binaryMessagePayload = binaryMessagePayload;
        return this;
    }

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