using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Sync;

namespace Fluent.Brighter.RMQ.Sync;

/// <summary>
/// Fluent builder for configuring RabbitMQ message publication settings in Brighter
/// </summary>
/// <remarks>
/// Provides a fluent interface to define publication parameters including CloudEvents metadata,
/// routing information, channel management behavior, and confirmation settings.
/// </remarks>
public sealed class RmqPublicationBuilder
{
    private Uri? _dataSchema;

    /// <summary>
    /// Sets the CloudEvents data schema URI (optional)
    /// </summary>
    /// <param name="dataSchema">URI identifying the schema of the payload data</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqPublicationBuilder SetDataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }

    private OnMissingChannel _makeChannels = OnMissingChannel.Create; 

    /// <summary>
    /// Configures channel creation behavior when missing (default: Create)
    /// </summary>
    /// <param name="makeChannels">Action to take when required channels don't exist</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqPublicationBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }

    private Type? _requestType;

    /// <summary>
    /// Sets the .NET type of the message being published (optional)
    /// </summary>
    /// <param name="requestType">The type of the request message</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqPublicationBuilder SetRequestType(Type? requestType)
    {
        _requestType = requestType;
        return this;
    }

    private Uri _source = new(MessageHeader.DefaultSource);

    /// <summary>
    /// Sets the CloudEvents source URI (default: Brighter default source)
    /// </summary>
    /// <param name="source">URI identifying the source of the event</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqPublicationBuilder SetSource(Uri source)
    {
        _source = source;
        return this;
    }

    private string? _subject;

    /// <summary>
    /// Sets the CloudEvents subject (optional)
    /// </summary>
    /// <param name="subject">Describes the subject of the event</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqPublicationBuilder SetSubject(string? subject)
    {
        _subject = subject;
        return this;
    }

    private RoutingKey? _topic;

    /// <summary>
    /// Sets the RabbitMQ routing key/topic (optional)
    /// </summary>
    /// <param name="topic">Routing key for message distribution</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqPublicationBuilder SetTopic(RoutingKey? topic)
    {
        _topic = topic;
        return this;
    }

    private CloudEventsType _type = CloudEventsType.Empty;

    /// <summary>
    /// Sets the CloudEvents type (default: Empty)
    /// </summary>
    /// <param name="type">Event type identifier</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqPublicationBuilder SetType(CloudEventsType type)
    {
        _type = type;
        return this;
    }

    private IDictionary<string, object>? _defaultHeaders;

    /// <summary>
    /// Sets default headers attached to all published messages (optional)
    /// </summary>
    /// <param name="defaultHeaders">Dictionary of header key-value pairs</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqPublicationBuilder SetDefaultHeaders(IDictionary<string, object>? defaultHeaders)
    {
        _defaultHeaders = defaultHeaders;
        return this;
    }

    private IDictionary<string, object>? _cloudEventsAdditionalProperties;

    /// <summary>
    /// Sets additional CloudEvents extension attributes (optional)
    /// </summary>
    /// <param name="cloudEventsAdditionalProperties">Extension attributes dictionary</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqPublicationBuilder SetCloudEventsAdditionalProperties(
        IDictionary<string, object>? cloudEventsAdditionalProperties)
    {
        _cloudEventsAdditionalProperties = cloudEventsAdditionalProperties;
        return this;
    }

    private string? _replyTo;

    /// <summary>
    /// Sets the reply-to queue for request-reply patterns (optional)
    /// </summary>
    /// <param name="replyTo">Name of the reply queue</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqPublicationBuilder SetReplyTo(string? replyTo)
    {
        _replyTo = replyTo;
        return this;
    }

    private int _waitForConfirmsTimeOutInMilliseconds = 500;

    /// <summary>
    /// Sets the timeout for publisher confirms (default: 500ms)
    /// </summary>
    /// <param name="waitForConfirmsTimeOutInMilliseconds">
    /// Time in milliseconds to wait for broker acknowledgments
    /// </param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqPublicationBuilder SetWaitForConfirmsTimeOutInMilliseconds(int waitForConfirmsTimeOutInMilliseconds)
    {
        _waitForConfirmsTimeOutInMilliseconds = waitForConfirmsTimeOutInMilliseconds;
        return this;
    }

    /// <summary>
    /// Constructs the final RmqPublication configuration
    /// </summary>
    /// <returns>Fully configured publication settings</returns>
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