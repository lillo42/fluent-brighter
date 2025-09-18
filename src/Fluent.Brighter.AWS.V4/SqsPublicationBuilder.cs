using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4;

public sealed class SqsPublicationBuilder
{
    private ChannelName? _channelName;

    public SqsPublicationBuilder SetQueueUrl(ChannelName? channelName)
    {
        _channelName = channelName;
        return this;
    }

    private ChannelType _channelType = ChannelType.PointToPoint;
    
    public SqsPublicationBuilder SetChannelType(ChannelType channelType)
    {
        _channelType = channelType;
        return this;
    }

    private QueueFindBy _findQueueBy = QueueFindBy.Name;
    
    public SqsPublicationBuilder SetFindQueueBy(QueueFindBy findQueueBy)
    {
        _findQueueBy = findQueueBy;
        return this;
    }

    private SqsAttributes _queueAttributes = SqsAttributes.Empty;

    public SqsPublicationBuilder SetQueueAttributes(SqsAttributes queueAttributes)
    {
        _queueAttributes = queueAttributes;
        return this;
    }
    
    private Uri? _dataSchema;
    
    public SqsPublicationBuilder SetDataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }
    
    private OnMissingChannel _makeChannels;

    public SqsPublicationBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }
    
    private Type? _requestType;

    public SqsPublicationBuilder SetRequestType(Type? requestType)
    {
        _requestType = requestType;
        return this;
    }

    private Uri _source = new(MessageHeader.DefaultSource);

    public SqsPublicationBuilder SetSource(Uri source)
    {
        _source = source;
        return this;
    }

    private string? _subject;

    public SqsPublicationBuilder SetSubject(string? subject)
    {
        _subject = subject;
        return this;
    }

    private RoutingKey? _topic;
    
    public SqsPublicationBuilder SetQueue(RoutingKey topic)
    {
        _topic = topic;
        _channelName = new ChannelName(_topic.Value);
        return this;
    }

    private CloudEventsType _type = CloudEventsType.Empty;
    
    public SqsPublicationBuilder SetType(CloudEventsType type)
    {
        _type = type;
        return this;
    }

    private IDictionary<string, object>? _defaultHeaders;
    
    public SqsPublicationBuilder SetDefaultHeaders(IDictionary<string, object>? defaultHeaders)
    {
        _defaultHeaders = defaultHeaders;
        return this;
    }

    public SqsPublicationBuilder SetDefaultHeaders(string key, object value)
    {
        _defaultHeaders ??= new Dictionary<string, object>();
        _defaultHeaders[key] = value;
        return this;
    }

    private IDictionary<string, object>? _cloudEventsAdditionalProperties;
    
    public SqsPublicationBuilder SetCloudEventsAdditionalProperties(IDictionary<string, object>? cloudEventsAdditionalProperties)
    {
        _cloudEventsAdditionalProperties = cloudEventsAdditionalProperties;
        return this;
    }

    public SqsPublicationBuilder AddCloudEventsAdditionalProperties(string key, object value)
    {
        _cloudEventsAdditionalProperties ??= new Dictionary<string, object>();
        _cloudEventsAdditionalProperties[key] = value;
        return this;
    }
    
    private string? _replyTo;

    public SqsPublicationBuilder SetReplyTo(string? replyTo)
    {
        _replyTo = replyTo;
        return this;
    }

    internal SqsPublication Build()
    {
        return new SqsPublication
        {
            ChannelName = _channelName,
            ChannelType = _channelType,
            FindQueueBy = _findQueueBy,
            QueueAttributes = _queueAttributes,
            MakeChannels = _makeChannels,
            DataSchema = _dataSchema,
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