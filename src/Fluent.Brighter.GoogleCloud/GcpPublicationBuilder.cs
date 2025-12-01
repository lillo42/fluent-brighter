using System;
using System.Collections.Generic;

using Google.Cloud.PubSub.V1;
using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.GcpPubSub;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for fluently configuring a Google Cloud Pub/Sub publication in Paramore.Brighter.
/// Provides methods to set topic attributes, publisher client configuration, data schema,
/// channel creation behavior, and message metadata.
/// </summary>
public sealed class GcpPublicationBuilder
{
    private TopicAttributes? _topicAttributes;

    /// <summary>
    /// Sets the attributes for the Google Cloud Pub/Sub Topic.
    /// This includes settings such as the ProjectId, Topic Name, and potentially more advanced configurations.
    /// </summary>
    /// <param name="topicAttributes">The topic attributes configuration</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPublicationBuilder SetTopicAttributes(TopicAttributes? topicAttributes)
    {
        _topicAttributes = topicAttributes;
        return this;
    }

    private Action<PublisherClientBuilder>? _publisherClientConfiguration;

    /// <summary>
    /// Sets an action to allow advanced customization of the PublisherClientBuilder.
    /// This is used to configure the client that publishes messages to the topic, for scenarios like
    /// setting custom client options, retries, or deadlines.
    /// </summary>
    /// <param name="publisherClientConfiguration">Action to configure the PublisherClientBuilder</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPublicationBuilder SetPublisherClientConfiguration(Action<PublisherClientBuilder>? publisherClientConfiguration)
    {
        _publisherClientConfiguration = publisherClientConfiguration;
        return this;
    }

    private Uri? _dataSchema;

    /// <summary>
    /// Sets the URI of the data schema for CloudEvents metadata.
    /// Identifies the schema that data adheres to. Incompatible changes to the schema should be reflected by a different URI.
    /// </summary>
    /// <param name="dataSchema">URI pointing to the event data schema</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPublicationBuilder SetDataSchema(Uri? dataSchema)
    {
        _dataSchema = dataSchema;
        return this;
    }

    private OnMissingChannel _makeChannels;

    /// <summary>
    /// Sets what to do with infrastructure dependencies for the producer.
    /// Controls whether to create, validate, or assume the existence of the topic.
    /// </summary>
    /// <param name="makeChannels">Policy for channel creation (validate, create, or assume)</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPublicationBuilder SetMakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }

    private Type? _requestType;

    /// <summary>
    /// Sets the type of the request that we expect to publish on this channel.
    /// </summary>
    /// <param name="requestType">The type of the message request</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPublicationBuilder SetRequestType(Type? requestType)
    {
        _requestType = requestType;
        return this;
    }

    private Uri _source = new Uri("http://goparamore.io");

    /// <summary>
    /// Sets the source URI for CloudEvents metadata.
    /// Identifies the context in which an event happened. Often this will include information such as the type of
    /// the event source, the organization publishing the event or the process that produced the event.
    /// Producers MUST ensure that source + id is unique for each distinct event.
    /// </summary>
    /// <param name="source">URI identifying the event source</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPublicationBuilder SetSource(Uri source)
    {
        _source = source;
        return this;
    }

    private string? _subject;

    /// <summary>
    /// Sets the subject of the event in the context of the event producer.
    /// In publish-subscribe scenarios, a subscriber will typically subscribe to events emitted by a source,
    /// but the source identifier alone might not be sufficient as a qualifier for any specific event if the
    /// source context has internal sub-structure.
    /// </summary>
    /// <param name="subject">The event subject</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPublicationBuilder SetSubject(string? subject)
    {
        _subject = subject;
        return this;
    }

    private RoutingKey? _topic;

    /// <summary>
    /// Sets the topic this publication is for defined by a RoutingKey.
    /// In a pub-sub scenario there is typically a topic, to which we publish and then a subscriber creates its own
    /// queue which the broker delivers messages to.
    /// </summary>
    /// <param name="topic">The topic name or routing key</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPublicationBuilder SetTopic(RoutingKey topic)
    {
        _topic = topic;
        return this;
    }

    private CloudEventsType _type = CloudEventsType.Empty;

    /// <summary>
    /// Sets the CloudEvents type metadata for message classification.
    /// This attribute contains a value describing the type of event related to the originating occurrence.
    /// Often this attribute is used for routing, observability, policy enforcement, etc.
    /// Should be prefixed with a reverse-DNS name.
    /// </summary>
    /// <param name="type">The CloudEvents type specification</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPublicationBuilder SetType(CloudEventsType type)
    {
        _type = type;
        return this;
    }

    private IDictionary<string, object>? _defaultHeaders;

    /// <summary>
    /// Sets the default headers to be included in published messages when using default message mappers.
    /// These headers will be automatically added to all messages published through Brighter's message producers.
    /// </summary>
    /// <param name="defaultHeaders">Dictionary of header names and values</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPublicationBuilder SetDefaultHeaders(IDictionary<string, object>? defaultHeaders)
    {
        _defaultHeaders = defaultHeaders;
        return this;
    }

    private IDictionary<string, object>? _cloudEventsAdditionalProperties;

    /// <summary>
    /// Sets a dictionary of additional properties related to CloudEvents.
    /// This property enables the inclusion of custom or vendor-specific metadata beyond the standard CloudEvents attributes.
    /// These properties are serialized alongside the core CloudEvents attributes when mapping to a CloudEvent message.
    /// </summary>
    /// <param name="cloudEventsAdditionalProperties">Extended CloudEvents properties</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPublicationBuilder SetCloudEventsAdditionalProperties(
        IDictionary<string, object>? cloudEventsAdditionalProperties)
    {
        _cloudEventsAdditionalProperties = cloudEventsAdditionalProperties;
        return this;
    }

    private string? _replyTo;

    /// <summary>
    /// Sets the reply to topic. Used when doing Request-Reply instead of Publish-Subscribe to identify
    /// the queue that the sender is listening on. Usually a sender listens on a private queue, so that they
    /// do not have to filter replies intended for other listeners.
    /// </summary>
    /// <param name="replyTo">The reply endpoint address</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcpPublicationBuilder SetReplyTo(string? replyTo)
    {
        _replyTo = replyTo;
        return this;
    }

    /// <summary>
    /// Builds a GcpPublication instance with the configured values.
    /// </summary>
    /// <returns>A configured GcpPublication instance</returns>
    internal GcpPublication Build()
    {
        return new GcpPublication
        {
            TopicAttributes = _topicAttributes,
            PublisherClientConfiguration = _publisherClientConfiguration,
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