using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4;

public sealed class SnsPublicationBuilder
{
    private TopicFindBy _findTopicBy = TopicFindBy.Convention;

    public SnsPublicationBuilder SetFindTopicBy(TopicFindBy findTopicBy)
    {
        _findTopicBy = findTopicBy;
        return this;
    }

    private SnsAttributes? _topicAttributes;

    public SnsPublicationBuilder SetTopicAttributes(SnsAttributes? topicAttributes)
    {
        _topicAttributes = topicAttributes;
        return this;
    }

    private string? _topicArn;

    public SnsPublicationBuilder SetTopicArn(string? topicArn)
    {
        _topicArn = topicArn;
        return this;
    }

    private Uri? _dataSchema;

    public SnsPublicationBuilder SetDataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }
    
    private OnMissingChannel _makeChannels;
    
    public SnsPublicationBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }

    private Type? _requestType;

    public SnsPublicationBuilder SetRequestType(Type? requestType)
    {
        _requestType = requestType;
        return this;
    }

    private Uri _source = new Uri(MessageHeader.DefaultSource);

    public SnsPublicationBuilder SetSource(Uri source)
    {
        _source = source;
        return this;
    }

    private string? _subject;

    public SnsPublicationBuilder SetSubject(string? subject)
    {
        _subject = subject;
        return this;
    }

    private RoutingKey? _topic;

    public SnsPublicationBuilder SetTopic(RoutingKey? topic)
    {
        _topic = topic;
        return this;
    }

    private CloudEventsType _type = CloudEventsType.Empty;

    public SnsPublicationBuilder SetType(CloudEventsType type)
    {
        _type = type;
        return this;
    }

    private IDictionary<string, object>? _defaultHeaders;

    public SnsPublicationBuilder SetDefaultHeaders(IDictionary<string, object>? defaultHeaders)
    {
        _defaultHeaders = defaultHeaders;
        return this;
    }

    private IDictionary<string, object>? _cloudEventsAdditionalProperties;

    public SnsPublicationBuilder SetCloudEventsAdditionalProperties(
        IDictionary<string, object>? cloudEventsAdditionalProperties)
    {
        _cloudEventsAdditionalProperties = cloudEventsAdditionalProperties;
        return this;
    }


    private string? _replyTo;

    public SnsPublicationBuilder SetReplyTo(string? replyTo)
    {
        _replyTo = replyTo;
        return this;
    }

    internal SnsPublication Build()
    {
        return new SnsPublication(_findTopicBy, _topicArn, _topicAttributes)
        {
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