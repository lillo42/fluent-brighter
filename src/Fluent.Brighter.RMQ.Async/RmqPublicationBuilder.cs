using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Async;

namespace Fluent.Brighter.RMQ.Async;

public sealed class RmqPublicationBuilder
{
    private Uri? _dataSchema;

    public RmqPublicationBuilder SetDataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }

    private OnMissingChannel _makeChannels = OnMissingChannel.Create; 

    public RmqPublicationBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }


    private Type? _requestType;

    public RmqPublicationBuilder SetRequestType(Type? requestType)
    {
        _requestType = requestType;
        return this;
    }


    private Uri _source = new(MessageHeader.DefaultSource);

    public RmqPublicationBuilder SetSource(Uri source)
    {
        _source = source;
        return this;
    }

    private string? _subject;

    public RmqPublicationBuilder SetSubject(string? subject)
    {
        _subject = subject;
        return this;
    }

    private RoutingKey? _topic;

    public RmqPublicationBuilder SetTopic(RoutingKey? topic)
    {
        _topic = topic;
        return this;
    }

    private CloudEventsType _type = CloudEventsType.Empty;

    public RmqPublicationBuilder SetType(CloudEventsType type)
    {
        _type = type;
        return this;
    }

    private IDictionary<string, object>? _defaultHeaders;

    public RmqPublicationBuilder SetDefaultHeaders(IDictionary<string, object>? defaultHeaders)
    {
        _defaultHeaders = defaultHeaders;
        return this;
    }


    private IDictionary<string, object>? _cloudEventsAdditionalProperties;

    public RmqPublicationBuilder SetCloudEventsAdditionalProperties(
        IDictionary<string, object>? cloudEventsAdditionalProperties)
    {
        _cloudEventsAdditionalProperties = cloudEventsAdditionalProperties;
        return this;
    }

    private string? _replyTo;

    public RmqPublicationBuilder SetReplyTo(string? replyTo)
    {
        _replyTo = replyTo;
        return this;
    }

    private int _waitForConfirmsTimeOutInMilliseconds = 500;

    public RmqPublicationBuilder SetWaitForConfirmsTimeOutInMilliseconds(int waitForConfirmsTimeOutInMilliseconds)
    {
        _waitForConfirmsTimeOutInMilliseconds = waitForConfirmsTimeOutInMilliseconds;
        return this;
    }

    internal RmqPublication Build()
    {
        return new RmqPublication 
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
            WaitForConfirmsTimeOutInMilliseconds = _waitForConfirmsTimeOutInMilliseconds,
        };
    }
}