using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

/// <summary>
/// Builder class for fluently configuring an SNS publication in Paramore.Brighter.
/// Provides methods to set various properties like topic identification method, attributes,
/// ARN, data schema, channel creation behavior, and message metadata.
/// </summary>
public sealed class SnsPublicationBuilder
{
    private TopicFindBy _findTopicBy = TopicFindBy.Convention;

    /// <summary>
    /// Sets the method for finding the target topic (by convention, name, or ARN)
    /// </summary>
    /// <param name="findTopicBy">The topic lookup strategy</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsPublicationBuilder SetFindTopicBy(TopicFindBy findTopicBy)
    {
        _findTopicBy = findTopicBy;
        return this;
    }

    private SnsAttributes? _topicAttributes;

    /// <summary>
    /// Sets custom attributes for the SNS topic when creating a new topic
    /// </summary>
    /// <param name="topicAttributes">SNS topic attributes like policy, delivery policy, etc.</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsPublicationBuilder SetTopicAttributes(SnsAttributes? topicAttributes)
    {
        _topicAttributes = topicAttributes;
        return this;
    }

    private string? _topicArn;

    /// <summary>
    /// Sets the Amazon Resource Name (ARN) of the target topic
    /// </summary>
    /// <param name="topicArn">The ARN of the SNS topic</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsPublicationBuilder SetTopicArn(string? topicArn)
    {
        _topicArn = topicArn;
        return this;
    }

    private Uri? _dataSchema;

    /// <summary>
    /// Sets the URI of the data schema for CloudEvents metadata
    /// </summary>
    /// <param name="dataSchema">URI pointing to the event data schema</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsPublicationBuilder SetDataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }
    
    private OnMissingChannel _makeChannels;
    
    /// <summary>
    /// Sets the channel creation behavior when a topic doesn't exist
    /// </summary>
    /// <param name="makeChannels">Policy for channel creation (validate, create, or assume)</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsPublicationBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }

    private Type? _requestType;
    
    /// <summary>
    /// Sets the .NET type of the request message being published
    /// </summary>
    /// <param name="requestType">The type of the message request</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsPublicationBuilder SetRequestType(Type? requestType)
    {
        _requestType = requestType;
        return this;
    }

    private Uri _source = new Uri(MessageHeader.DefaultSource);

    /// <summary>
    /// Sets the source URI for CloudEvents metadata
    /// </summary>
    /// <param name="source">URI identifying the event source</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsPublicationBuilder SetSource(Uri source)
    {
        _source = source;
        return this;
    }

    private string? _subject;

    /// <summary>
    /// Sets the subject line for SNS notifications
    /// </summary>
    /// <param name="subject">The email subject for SNS notifications</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsPublicationBuilder SetSubject(string? subject)
    {
        _subject = subject;
        return this;
    }

    private RoutingKey? _topic;

    /// <summary>
    /// Sets the topic name or routing key for the publication
    /// </summary>
    /// <param name="topic">The topic name or routing key</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsPublicationBuilder SetTopic(RoutingKey? topic)
    {
        _topic = topic;
        return this;
    }

    private CloudEventsType _type = CloudEventsType.Empty;

    /// <summary>
    /// Sets the CloudEvents type metadata for message classification
    /// </summary>
    /// <param name="type">The CloudEvents type specification</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsPublicationBuilder SetType(CloudEventsType type)
    {
        _type = type;
        return this;
    }

    private IDictionary<string, object>? _defaultHeaders;

    /// <summary>
    /// Sets default headers to include with all published messages
    /// </summary>
    /// <param name="defaultHeaders">Dictionary of header names and values</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsPublicationBuilder SetDefaultHeaders(IDictionary<string, object>? defaultHeaders)
    {
        _defaultHeaders = defaultHeaders;
        return this;
    }

    private IDictionary<string, object>? _cloudEventsAdditionalProperties;

    /// <summary>
    /// Sets additional properties for CloudEvents metadata
    /// </summary>
    /// <param name="cloudEventsAdditionalProperties">Extended CloudEvents properties</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsPublicationBuilder SetCloudEventsAdditionalProperties(
        IDictionary<string, object>? cloudEventsAdditionalProperties)
    {
        _cloudEventsAdditionalProperties = cloudEventsAdditionalProperties;
        return this;
    }


    private string? _replyTo;

    /// <summary>
    /// Sets the reply-to address for request-reply messaging patterns
    /// </summary>
    /// <param name="replyTo">The reply endpoint address</param>
    /// <returns>The builder instance for method chaining</returns>
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