using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4;

/// <summary>
/// Builder class for fluently configuring an SQS publication in Paramore.Brighter.
/// Provides methods to set various properties like queue identification, channel configuration,
/// attributes, data schema, and message metadata for Amazon SQS messaging.
/// </summary>
public sealed class SqsPublicationBuilder
{
    private ChannelName? _channelName;

    /// <summary>
    /// Sets the channel name which typically corresponds to the SQS queue URL or name
    /// </summary>
    /// <param name="channelName">The name of the channel/queue</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsPublicationBuilder SetQueueUrl(ChannelName? channelName)
    {
        _channelName = channelName;
        return this;
    }

    private ChannelType _channelType = ChannelType.PointToPoint;

    /// <summary>
    /// Sets the channel type which determines the messaging pattern
    /// (Point-to-Point for queues or Publish-Subscribe for topics)
    /// </summary>
    /// <param name="channelType">The type of channel</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsPublicationBuilder SetChannelType(ChannelType channelType)
    {
        _channelType = channelType;
        return this;
    }

    private QueueFindBy _findQueueBy = QueueFindBy.Name;

    /// <summary>
    /// Sets the method for finding the target queue (by name or ARN)
    /// </summary>
    /// <param name="findQueueBy">The queue lookup strategy</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsPublicationBuilder SetFindQueueBy(QueueFindBy findQueueBy)
    {
        _findQueueBy = findQueueBy;
        return this;
    }

    private SqsAttributes _queueAttributes = SqsAttributes.Empty;

    /// <summary>
    /// Sets custom attributes for the SQS queue when creating a new queue
    /// </summary>
    /// <param name="queueAttributes">SQS queue attributes like visibility timeout, message retention, etc.</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsPublicationBuilder SetQueueAttributes(SqsAttributes queueAttributes)
    {
        _queueAttributes = queueAttributes;
        return this;
    }

    private Uri? _dataSchema;

    /// <summary>
    /// Sets the URI of the data schema for CloudEvents metadata
    /// </summary>
    /// <param name="dataSchema">URI pointing to the event data schema</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsPublicationBuilder SetDataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }

    private OnMissingChannel _makeChannels;

    /// <summary>
    /// Sets the channel creation behavior when a queue doesn't exist
    /// </summary>
    /// <param name="makeChannels">Policy for channel creation (validate, create, or assume)</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsPublicationBuilder SetMakeChannels(OnMissingChannel makeChannels)
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
    public SqsPublicationBuilder SetRequestType(Type? requestType)
    {
        _requestType = requestType;
        return this;
    }

    private Uri _source = new(MessageHeader.DefaultSource);

    /// <summary>
    /// Sets the source URI for CloudEvents metadata
    /// </summary>
    /// <param name="source">URI identifying the event source</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsPublicationBuilder SetSource(Uri source)
    {
        _source = source;
        return this;
    }

    private string? _subject;

    /// <summary>
    /// Sets the subject line for messages (used in certain message formats)
    /// </summary>
    /// <param name="subject">The message subject</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsPublicationBuilder SetSubject(string? subject)
    {
        _subject = subject;
        return this;
    }

    private RoutingKey? _topic;

    /// <summary>
    /// Sets the queue name and automatically derives the channel name from it
    /// </summary>
    /// <param name="topic">The queue name/routing key</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsPublicationBuilder SetQueue(RoutingKey topic)
    {
        _topic = topic;
        _channelName = new ChannelName(_topic.Value);
        return this;
    }

    private CloudEventsType _type = CloudEventsType.Empty;

    /// <summary>
    /// Sets the CloudEvents type metadata for message classification
    /// </summary>
    /// <param name="type">The CloudEvents type specification</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsPublicationBuilder SetType(CloudEventsType type)
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
    public SqsPublicationBuilder SetDefaultHeaders(IDictionary<string, object>? defaultHeaders)
    {
        _defaultHeaders = defaultHeaders;
        return this;
    }

    /// <summary>
    /// Adds or updates a single default header to include with all published messages
    /// </summary>
    /// <param name="key">The header name</param>
    /// <param name="value">The header value</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsPublicationBuilder AddDefaultHeaders(string key, object value)
    {
        _defaultHeaders ??= new Dictionary<string, object>();
        _defaultHeaders[key] = value;
        return this;
    }

    private IDictionary<string, object>? _cloudEventsAdditionalProperties;

    /// <summary>
    /// Sets additional properties for CloudEvents metadata
    /// </summary>
    /// <param name="cloudEventsAdditionalProperties">Extended CloudEvents properties</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsPublicationBuilder SetCloudEventsAdditionalProperties(IDictionary<string, object>? cloudEventsAdditionalProperties)
    {
        _cloudEventsAdditionalProperties = cloudEventsAdditionalProperties;
        return this;
    }

    /// <summary>
    /// Adds or updates a single CloudEvents additional property
    /// </summary>
    /// <param name="key">The property name</param>
    /// <param name="value">The property value</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsPublicationBuilder AddCloudEventsAdditionalProperties(string key, object value)
    {
        _cloudEventsAdditionalProperties ??= new Dictionary<string, object>();
        _cloudEventsAdditionalProperties[key] = value;
        return this;
    }

    private string? _replyTo;

    /// <summary>
    /// Sets the reply-to address for request-reply messaging patterns
    /// </summary>
    /// <param name="replyTo">The reply endpoint address</param>
    /// <returns>The builder instance for method chaining</returns>
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