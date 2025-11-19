using System;

using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Google.Cloud.ResourceManager.V3;
using Paramore.Brighter.MessagingGateway.GcpPubSub;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for configuring and creating a <see cref="GcpMessagingGatewayConnection"/>.
/// Provides a fluent interface for setting up Google Cloud Pub/Sub gateway connections.
/// </summary>
public class GcpMessagingGatewayConnectionBuilder
{
    private ICredential? _credential;
    
    /// <summary>
    /// Sets the <see cref="ICredential"/> to use for authentication with Google Cloud Pub/Sub.
    /// </summary>
    /// <param name="credential">The credential to use for authentication. If not set, default Google credential resolution will be used.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public GcpMessagingGatewayConnectionBuilder SetCredential(ICredential credential)
    {
        _credential = credential;
        return this;
    }
    
    private string _projectId = string.Empty;
    
    /// <summary>
    /// Sets the Google Cloud project ID.
    /// </summary>
    /// <param name="projectId">The Google Cloud project ID. This is required for most operations.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public GcpMessagingGatewayConnectionBuilder SetProjectId(string projectId)
    {
        _projectId = projectId;
        return this;
    }
    
    private Action<PublisherServiceApiClientBuilder>? _topicManagerConfiguration;
    
    /// <summary>
    /// Sets an action to configure the <see cref="PublisherServiceApiClientBuilder"/> used for topic management.
    /// </summary>
    /// <param name="configure">Action to configure the builder for topic management (create/update/delete topics).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public GcpMessagingGatewayConnectionBuilder SetTopicManagerConfiguration(Action<PublisherServiceApiClientBuilder> configure)
    {
        _topicManagerConfiguration = configure;
        return this;
    }
    
    private Action<PublisherClientBuilder>? _publisherConfiguration;
    
    /// <summary>
    /// Sets an action to configure the <see cref="PublisherClientBuilder"/> used to publish messages to a topic.
    /// </summary>
    /// <param name="configure">Action to configure the builder for publishing messages.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public GcpMessagingGatewayConnectionBuilder SetPublisherConfiguration(Action<PublisherClientBuilder> configure)
    {
        _publisherConfiguration = configure;
        return this;
    }
    
    private Action<SubscriberServiceApiClientBuilder>? _subscriptionManagerConfiguration;
    
    /// <summary>
    /// Sets an action to configure the <see cref="SubscriberServiceApiClientBuilder"/> used for subscription management.
    /// </summary>
    /// <param name="configure">Action to configure the builder for pull mode and subscription management (create/update/delete subscription).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public GcpMessagingGatewayConnectionBuilder SetSubscriptionManagerConfiguration(Action<SubscriberServiceApiClientBuilder> configure)
    {
        _subscriptionManagerConfiguration = configure;
        return this;
    }
    
    private Action<SubscriberClientBuilder>? _streamConfiguration;
    
    /// <summary>
    /// Sets an action to configure the <see cref="SubscriberClientBuilder"/> used for pull mode message consumption.
    /// </summary>
    /// <param name="configure">Action to configure the builder for streaming message consumption.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public GcpMessagingGatewayConnectionBuilder SetStreamConfiguration(Action<SubscriberClientBuilder> configure)
    {
        _streamConfiguration = configure;
        return this;
    }
    
    private Action<ProjectsClientBuilder>? _projectsClientConfiguration;
    
    /// <summary>
    /// Sets an action to configure the <see cref="ProjectsClientBuilder"/> used for managing projects.
    /// </summary>
    /// <param name="configure">Action to configure the builder for project-level operations.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public GcpMessagingGatewayConnectionBuilder SetProjectsClientConfiguration(Action<ProjectsClientBuilder> configure)
    {
        _projectsClientConfiguration = configure;
        return this;
    }
    
    /// <summary>
    /// Builds the <see cref="GcpMessagingGatewayConnection"/> with the configured settings.
    /// </summary>
    /// <returns>A new instance of <see cref="GcpMessagingGatewayConnection"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the project ID is not set.</exception>
    internal GcpMessagingGatewayConnection Build()
    {
        if (string.IsNullOrEmpty(_projectId))
        {
            throw new InvalidOperationException("Project ID must be set before building the connection.");
        }
        
        return new GcpMessagingGatewayConnection
        {
            Credential = _credential,
            ProjectId = _projectId,
            TopicManagerConfiguration = _topicManagerConfiguration,
            PublisherConfiguration = _publisherConfiguration,
            SubscriptionManagerConfiguration = _subscriptionManagerConfiguration,
            StreamConfiguration = _streamConfiguration,
            ProjectsClientConfiguration = _projectsClientConfiguration
        };
    }
}