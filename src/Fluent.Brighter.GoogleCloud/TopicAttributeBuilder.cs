using System;
using System.Collections.Generic;

using Google.Cloud.PubSub.V1;
using Google.Protobuf.WellKnownTypes;

using Paramore.Brighter.MessagingGateway.GcpPubSub;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for fluently configuring Google Cloud Pub/Sub topic attributes.
/// Provides methods to set topic properties including retention, labels, schema settings,
/// and encryption configuration.
/// </summary>
public sealed class TopicAttributeBuilder
{
    private string _name = string.Empty;

    /// <summary>
    /// Sets the name of the Topic. This is the resource identifier within the Project.
    /// If not set, it defaults to the Brighter Publication Topic name.
    /// </summary>
    /// <param name="name">The topic name</param>
    /// <returns>The builder instance for method chaining</returns>
    public TopicAttributeBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    private string? _projectId;

    /// <summary>
    /// Sets the Google Cloud Project ID where the Topic should be located.
    /// If null, the GcpMessagingGatewayConnection ProjectId will be used.
    /// </summary>
    /// <param name="projectId">The Google Cloud Project ID</param>
    /// <returns>The builder instance for method chaining</returns>
    public TopicAttributeBuilder SetProjectId(string? projectId)
    {
        _projectId = projectId;
        return this;
    }

    private Dictionary<string, string> _labels = new();

    /// <summary>
    /// Sets a dictionary of key-value pairs that are attached to the Topic as labels.
    /// Labels are typically used for organization, billing, and resource management.
    /// </summary>
    /// <param name="labels">Dictionary of label key-value pairs</param>
    /// <returns>The builder instance for method chaining</returns>
    public TopicAttributeBuilder SetLabels(Dictionary<string, string> labels)
    {
        _labels = labels;
        return this;
    }

    /// <summary>
    /// Adds a single label to the Topic.
    /// </summary>
    /// <param name="key">The label key</param>
    /// <param name="value">The label value</param>
    /// <returns>The builder instance for method chaining</returns>
    public TopicAttributeBuilder AddLabel(string key, string value)
    {
        _labels[key] = value;
        return this;
    }

    private TimeSpan? _messageRetentionDuration;

    /// <summary>
    /// Sets the duration for which Pub/Sub retains messages published to the topic.
    /// If null, messages are retained for 7 days (the default maximum).
    /// Note: The value must be at least 10 minutes (600 seconds).
    /// </summary>
    /// <param name="duration">The message retention duration</param>
    /// <returns>The builder instance for method chaining</returns>
    public TopicAttributeBuilder SetMessageRetentionDuration(TimeSpan? duration)
    {
        _messageRetentionDuration = duration;
        return this;
    }

    private MessageStoragePolicy? _storePolicy;

    /// <summary>
    /// Sets the message storage policy configuration for the Topic.
    /// This defines which Google Cloud regions are allowed to store messages for this topic.
    /// </summary>
    /// <param name="storePolicy">The message storage policy</param>
    /// <returns>The builder instance for method chaining</returns>
    public TopicAttributeBuilder SetStorePolicy(MessageStoragePolicy? storePolicy)
    {
        _storePolicy = storePolicy;
        return this;
    }

    private SchemaSettings? _schemaSettings;

    /// <summary>
    /// Sets the schema settings for the Topic.
    /// This is used to enforce a specific schema (like Avro or Protocol Buffers) on published messages.
    /// </summary>
    /// <param name="schemaSettings">The schema settings</param>
    /// <returns>The builder instance for method chaining</returns>
    public TopicAttributeBuilder SetSchemaSettings(SchemaSettings? schemaSettings)
    {
        _schemaSettings = schemaSettings;
        return this;
    }

    private string? _kmsKeyName;

    /// <summary>
    /// Sets the Cloud KMS key name that is used to encrypt and decrypt messages published to the topic.
    /// This enables Customer-Managed Encryption Keys (CMEK).
    /// </summary>
    /// <param name="kmsKeyName">The KMS key name</param>
    /// <returns>The builder instance for method chaining</returns>
    public TopicAttributeBuilder SetKmsKeyName(string? kmsKeyName)
    {
        _kmsKeyName = kmsKeyName;
        return this;
    }

    private Action<Topic>? _topicConfiguration;

    /// <summary>
    /// Sets an action to configure the Topic object before it is used for creation or update.
    /// This allows for setting any property on the underlying Google Pub/Sub Topic object not exposed
    /// directly by TopicAttributes.
    /// </summary>
    /// <param name="topicConfiguration">Action to configure the Topic</param>
    /// <returns>The builder instance for method chaining</returns>
    public TopicAttributeBuilder SetTopicConfiguration(Action<Topic>? topicConfiguration)
    {
        _topicConfiguration = topicConfiguration;
        return this;
    }

    private Action<FieldMask>? _updateMaskConfiguration;

    /// <summary>
    /// Sets an action to configure the FieldMask used when updating a Pub/Sub Topic.
    /// This is required to explicitly tell the Pub/Sub API which fields are being changed.
    /// Use this to include fields configured via TopicConfiguration that aren't standard Brighter attributes.
    /// </summary>
    /// <param name="updateMaskConfiguration">Action to configure the FieldMask</param>
    /// <returns>The builder instance for method chaining</returns>
    public TopicAttributeBuilder SetUpdateMaskConfiguration(Action<FieldMask>? updateMaskConfiguration)
    {
        _updateMaskConfiguration = updateMaskConfiguration;
        return this;
    }

    /// <summary>
    /// Builds a TopicAttributes instance with the configured values.
    /// </summary>
    /// <returns>A configured TopicAttributes instance</returns>
    internal TopicAttributes Build()
    {
        return new TopicAttributes
        {
            Name = _name,
            ProjectId = _projectId,
            Labels = _labels,
            MessageRetentionDuration = _messageRetentionDuration,
            StorePolicy = _storePolicy,
            SchemaSettings = _schemaSettings,
            KmsKeyName = _kmsKeyName,
            TopicConfiguration = _topicConfiguration,
            UpdateMaskConfiguration = _updateMaskConfiguration
        };
    }
}