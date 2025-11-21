using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.GcpPubSub;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Central configuration class for setting up Google Cloud Platform services in Paramore.Brighter.
/// Provides fluent methods to configure Pub/Sub for message publication and subscription,
/// Firestore and Spanner for inbox/outbox patterns, distributed locking, and GCS-based luggage storage.
/// </summary>
public sealed class GcpConfigurator
{
    private GcpMessagingGatewayConnection? _connection;
    private Action<FluentBrighterBuilder> _action = _ => { };

    public GcpConfigurator SetConnection(GcpMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }
    
    public GcpConfigurator SetConnection(Action<GcpMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new GcpMessagingGatewayConnectionBuilder();
        configure(connection);
        _connection = connection.Build();
        return this;
    }

    /// <summary>
    /// Sets the Google Cloud Project ID for all GCP services.
    /// This project ID will be used as the default for Pub/Sub, Firestore, Spanner, and GCS operations.
    /// </summary>
    /// <param name="projectId">The Google Cloud Project ID</param>
    /// <returns>The configurator instance for method chaining</returns>
    public GcpConfigurator SetProjectId(string projectId)
    {
        _connection ??= new GcpMessagingGatewayConnection();
        _connection.ProjectId = projectId;
        return this;
    }

    #region Pub/Sub Publication

    /// <summary>
    /// Configures Google Cloud Pub/Sub for message publication.
    /// Allows configuration of topic attributes, publisher client settings, and CloudEvents metadata.
    /// </summary>
    /// <param name="configure">Action to configure Pub/Sub publication settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    /// <example>
    /// <code>
    /// configurator.UsePubSubPublication(pub =>
    /// {
    ///     pub.AddPublication&lt;MyCommand&gt;(cfg =>
    ///     {
    ///         cfg.SetTopicAttributes(attrs => attrs.SetName("my-topic"))
    ///            .SetSource("https://example.com");
    ///     });
    /// });
    /// </code>
    /// </example>
    public GcpConfigurator UsePubSubPublication(Action<GcpPublicationFactoryBuilder> configure)
    {
        _action += fluent => fluent
            .Producers(producer => producer.AddGcpPubSubPublication(cfg =>
            {
                cfg.SetConnection(_connection!);
                configure(cfg);
            }));

        return this;
    }

    #endregion

    #region Pub/Sub Subscription

    /// <summary>
    /// Configures Google Cloud Pub/Sub subscriptions for message consumption.
    /// Allows configuration of subscription properties including acknowledgment deadlines,
    /// message ordering, dead letter policies, and streaming vs. pull mode.
    /// </summary>
    /// <param name="configure">Action to configure Pub/Sub subscription settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    /// <example>
    /// <code>
    /// configurator.UsePubSubSubscription(sub =>
    /// {
    ///     sub.AddSubscription&lt;MyCommand&gt;(cfg =>
    ///     {
    ///         cfg.SetAckDeadlineSeconds(60)
    ///            .SetNoOfPerformers(5)
    ///            .EnableMessageOrdering();
    ///     });
    /// });
    /// </code>
    /// </example>
    public GcpConfigurator UsePubSubSubscription(Action<PubSubSubscriptionConfigurator> configure)
    {
        _action += fluent => fluent
            .Subscriptions(sub =>
            {
                var channel = new GcpPubSubChannelFactory(_connection!);
                var configurator = new PubSubSubscriptionConfigurator(channel);
                configure(configurator);

                sub.AddChannelFactory(channel);
                foreach (var subscription in configurator.Subscriptions)
                {
                    sub.AddSubscription(subscription);
                }
            });

        return this;
    }

    #endregion

    #region Firestore Inbox

    /// <summary>
    /// Configures Firestore as the inbox store using default settings.
    /// The inbox pattern ensures idempotent message processing by tracking received messages.
    /// </summary>
    /// <returns>The configurator instance for method chaining</returns>
    public GcpConfigurator UseFirestoreInbox(string tableName)
    {
        return UseFirestoreOutbox(cfg => cfg
            .SetConfiguration(c => c
                .SetInbox(tb => tb
                    .SetName(tableName))));
    }

    /// <summary>
    /// Configures Firestore as the inbox store with custom configuration.
    /// The inbox pattern ensures idempotent message processing by tracking received messages.
    /// </summary>
    /// <param name="configure">Action to configure Firestore inbox settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    /// <example>
    /// <code>
    /// configurator.UseFirestoreInbox(cfg =>
    /// {
    ///     cfg.SetConfiguration(config =>
    ///     {
    ///         config.SetProjectId("my-project")
    ///               .SetDatabase("(default)")
    ///               .SetInbox(inbox => inbox.SetName("inbox-collection"));
    ///     });
    /// });
    /// </code>
    /// </example>
    public GcpConfigurator UseFirestoreInbox(Action<FirestoreInboxBuilder> configure)
    {
        _action += fluent => fluent
            .Subscriptions(sub => sub.UseFirestoreInbox(cfg =>
            {
                cfg.SetConfiguration(c => c
                    .SetCredential(_connection!.Credential)
                    .SetProjectId(_connection!.ProjectId));
                configure(cfg);
            }));

        return this;
    }

    #endregion

    #region Firestore Outbox

    /// <summary>
    /// Configures Firestore as the outbox store using default settings.
    /// The outbox pattern ensures reliable message publishing by storing messages before sending.
    /// </summary>
    /// <returns>The configurator instance for method chaining</returns>
    public GcpConfigurator UseFirestoreOutbox(string tableName)
    {
        return UseFirestoreOutbox(cfg => cfg
            .SetConfiguration(c => c
                .SetLocking(tb => tb
                    .SetName(tableName))));
    }

    /// <summary>
    /// Configures Firestore as the outbox store with custom configuration.
    /// The outbox pattern ensures reliable message publishing by storing messages before sending.
    /// </summary>
    /// <param name="configure">Action to configure Firestore outbox settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    /// <example>
    /// <code>
    /// configurator.UseFirestoreOutbox(cfg =>
    /// {
    ///     cfg.SetConfiguration(config =>
    ///     {
    ///         config.SetProjectId("my-project")
    ///               .SetDatabase("(default)")
    ///               .SetOutbox(outbox => outbox.SetName("outbox-collection"));
    ///     });
    /// });
    /// </code>
    /// </example>
    public GcpConfigurator UseFirestoreOutbox(Action<FirestoreOutboxBuilder> configure)
    {
        _action += fluent => fluent
            .Producers(prod => prod.UseFirestoreOutbox(cfg =>
            {
                cfg.SetConfiguration(config => config
                    .SetProjectId(_connection!.ProjectId)
                    .SetCredential(_connection.Credential)
                );
                configure(cfg);
            }));

        return this;
    }

    #endregion

    #region Spanner Inbox

    /// <summary>
    /// Configures Cloud Spanner as the inbox store with custom configuration.
    /// The inbox pattern ensures idempotent message processing by tracking received messages.
    /// Cloud Spanner provides strong consistency and high availability for the inbox.
    /// </summary>
    /// <param name="configure">Action to configure Spanner inbox settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    /// <example>
    /// <code>
    /// configurator.UseSpannerInbox(cfg =>
    /// {
    ///     cfg.SetConfiguration(config =>
    ///     {
    ///         config.SetTableName("Inbox")
    ///               .SetContextKey("SpannerConnection");
    ///     });
    /// });
    /// </code>
    /// </example>
    public GcpConfigurator UseSpannerInbox(Action<SpannerInboxBuilder> configure)
    {
        _action += fluent => fluent
            .Subscriptions(sub => sub.UseSpannerInbox(configure));

        return this;
    }

    #endregion

    #region Spanner Outbox

    /// <summary>
    /// Configures Cloud Spanner as the outbox store with custom configuration.
    /// The outbox pattern ensures reliable message publishing by storing messages before sending.
    /// Cloud Spanner provides strong consistency and high availability for the outbox.
    /// </summary>
    /// <param name="configure">Action to configure Spanner outbox settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    /// <example>
    /// <code>
    /// configurator.UseSpannerOutbox(cfg =>
    /// {
    ///     cfg.SetConfiguration(config =>
    ///     {
    ///         config.SetTableName("Outbox")
    ///               .SetContextKey("SpannerConnection");
    ///     });
    /// });
    /// </code>
    /// </example>
    public GcpConfigurator UseSpannerOutbox(Action<SpannerOutboxBuilder> configure)
    {
        _action += fluent => fluent
            .Producers(prod => prod.UseSpannerOutbox(configure));

        return this;
    }

    #endregion

    #region Firestore Distributed Lock

    /// <summary>
    /// Configures Firestore for distributed locking with default settings.
    /// Distributed locks prevent concurrent processing of the same message across multiple instances.
    /// </summary>
    /// <returns>The configurator instance for method chaining</returns>
    public GcpConfigurator UseFirestoreDistributedLock(string tableName)
    {
        return UseFirestoreDistributedLock(cfg => cfg
            .SetConfiguration(c => c
                .SetLocking(tb => tb.SetName(tableName))));
    }

    /// <summary>
    /// Configures Firestore for distributed locking with custom configuration.
    /// Distributed locks prevent concurrent processing of the same message across multiple instances.
    /// </summary>
    /// <param name="configure">Action to configure Firestore distributed locking settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    /// <example>
    /// <code>
    /// configurator.UseFirestoreDistributedLock(cfg =>
    /// {
    ///     cfg.SetProjectId("my-project")
    ///        .SetDatabase("(default)")
    ///        .SetLocking(lock => lock.SetName("locks").SetTtl(TimeSpan.FromMinutes(5)));
    /// });
    /// </code>
    /// </example>
    public GcpConfigurator UseFirestoreDistributedLock(Action<FirestoreLockingBuilder> configure)
    {
        _action += fluent => fluent
            .Producers(prod => prod.UseFirestoreDistributedLock(cfg =>
            {
                cfg.SetConfiguration(c => c
                    .SetProjectId(_connection!.ProjectId)
                    .SetCredential(_connection!.Credential));
                configure(cfg);
            }));

        return this;
    }

    #endregion

    #region GCS Luggage Store

    /// <summary>
    /// Configures Google Cloud Storage (GCS) as the luggage store with a specific bucket name.
    /// The luggage store handles large message payloads by storing them in GCS and passing references.
    /// </summary>
    /// <param name="bucketName">Name of the GCS bucket to use</param>
    /// <returns>The configurator instance for method chaining</returns>
    public GcpConfigurator UseGcsLuggageStore(string bucketName)
    {
        return UseGcsLuggageStore(cfg => cfg.SetBucketName(bucketName));
    }

    /// <summary>
    /// Configures Google Cloud Storage (GCS) as the luggage store with custom settings.
    /// The luggage store handles large message payloads by storing them in GCS and passing references.
    /// </summary>
    /// <param name="configure">Action to configure GCS luggage store settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    /// <example>
    /// <code>
    /// configurator.UseGcsLuggageStore(cfg =>
    /// {
    ///     cfg.SetProjectId("my-project")
    ///        .SetBucketName("message-luggage")
    ///        .SetCredential(GoogleCredential.GetApplicationDefault());
    /// });
    /// </code>
    /// </example>
    public GcpConfigurator UseGcsLuggageStore(Action<GcsLuggageStoreBuilder> configure)
    {
        _action += fluent => fluent
            .SetLuggageStore(store => store.UseGcsLuggageStore(cfg =>
            {
                cfg
                    .SetCredential(_connection!.Credential)
                    .SetProjectId(_connection.ProjectId);
                configure(cfg);
            }));

        return this;
    }

    #endregion

    #region Outbox Archive

    /// <summary>
    /// Configures Firestore as the outbox archive store using default settings.
    /// The outbox archiver moves old messages from the outbox to an archive for long-term storage.
    /// </summary>
    /// <returns>The configurator instance for method chaining</returns>
    public GcpConfigurator UseFirestoreOutboxArchive()
    {
        _action += static fluent => fluent.UseFirestoreTransactionOutboxArchive();
        return this;
    }

    /// <summary>
    /// Configures Firestore as the outbox archive store with custom settings.
    /// The outbox archiver moves old messages from the outbox to an archive for long-term storage.
    /// </summary>
    /// <param name="configure">Action to configure outbox archiver options</param>
    /// <returns>The configurator instance for method chaining</returns>
    public GcpConfigurator UseFirestoreOutboxArchive(Action<TimedOutboxArchiverOptionsBuilder> configure)
    {
        _action += fluent => fluent.UseFirestoreTransactionOutboxArchive(configure);
        return this;
    }

    /// <summary>
    /// Configures Cloud Spanner as the outbox archive store using default settings.
    /// The outbox archiver moves old messages from the outbox to an archive for long-term storage.
    /// </summary>
    /// <returns>The configurator instance for method chaining</returns>
    public GcpConfigurator UseSpannerOutboxArchive()
    {
        _action += static fluent => fluent.UseSpannerTransactionOutboxArchive();
        return this;
    }

    /// <summary>
    /// Configures Cloud Spanner as the outbox archive store with custom settings.
    /// The outbox archiver moves old messages from the outbox to an archive for long-term storage.
    /// </summary>
    /// <param name="configure">Action to configure outbox archiver options</param>
    /// <returns>The configurator instance for method chaining</returns>
    public GcpConfigurator UseSpannerOutboxArchive(Action<TimedOutboxArchiverOptionsBuilder> configure)
    {
        _action += fluent => fluent.UseSpannerTransactionOutboxArchive(configure);
        return this;
    }

    #endregion

    /// <summary>
    /// Internal method to apply all configured settings to the FluentBrighterBuilder.
    /// </summary>
    /// <param name="builder">The FluentBrighterBuilder to configure</param>
    /// <exception cref="ConfigurationException">Thrown when project ID is not set</exception>
    internal void SetFluentBrighter(FluentBrighterBuilder builder)
    {
        if (_connection == null)
        {
            throw new ConfigurationException("Google Cloud Project ID was not set. Use SetProjectId() to configure.");
        }

        _action(builder);
    }
}