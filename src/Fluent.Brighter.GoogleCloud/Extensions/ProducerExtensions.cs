using System;

using Fluent.Brighter.GoogleCloud;

using Paramore.Brighter;
using Paramore.Brighter.Firestore;
using Paramore.Brighter.Locking.Firestore;
using Paramore.Brighter.Outbox.Firestore;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for <see cref="ProducerBuilder"/> to configure Google Cloud Platform components.
/// </summary>
public static class ProducerExtensions
{
    /// <summary>
    /// Adds Google Cloud Pub/Sub publication support to the producer.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance.</param>
    /// <param name="configure">An action to configure the <see cref="GcpPublicationFactoryBuilder"/>.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    /// <example>
    /// <code>
    /// builder.AddGcpPubSubPublication(pub => pub
    ///     .SetConnection(conn => conn.SetProjectId("my-project"))
    ///     .SetPublication(new GcpPublication { Topic = "my-topic" }));
    /// </code>
    /// </example>
    public static ProducerBuilder AddGcpPubSubPublication(this ProducerBuilder builder,
        Action<GcpPublicationFactoryBuilder> configure)
    {
        var factory = new GcpPublicationFactoryBuilder();
        configure(factory);
        return builder.AddMessageProducerFactory(factory.Build());
    }


    #region Outbox

    #region Firestore

    /// <summary>
    /// Configures the producer to use Google Cloud Firestore as the outbox for storing outgoing messages.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance.</param>
    /// <param name="configure">An action to configure the <see cref="FirestoreOutboxBuilder"/>.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    /// <remarks>
    /// This automatically sets up the Firestore connection provider and unit of work for transactional outbox support.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.UseFirestoreOutbox(outbox => outbox
    ///     .SetConfiguration(firestoreConfig));
    /// </code>
    /// </example>
    public static ProducerBuilder UseFirestoreOutbox(this ProducerBuilder builder,
        Action<FirestoreOutboxBuilder> configure)
    {
        var outbox = new FirestoreOutboxBuilder();
        configure(outbox);
        return builder.UseFirestoreOutbox(outbox.Build());
    }

    /// <summary>
    /// Configures the producer to use a pre-built Google Cloud Firestore outbox instance.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance.</param>
    /// <param name="outbox">The <see cref="FirestoreOutbox"/> instance to use.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    /// <remarks>
    /// This automatically sets up the Firestore connection provider and unit of work for transactional outbox support.
    /// </remarks>
    public static ProducerBuilder UseFirestoreOutbox(this ProducerBuilder builder, FirestoreOutbox outbox)
    {
        return builder.SetOutbox(outbox)
            .SetConnectionProvider(typeof(FirestoreConnectionProvider))
            .SetTransactionProvider(typeof(FirestoreUnitOfWork));
    }

    #endregion

    #region Spanner

    /// <summary>
    /// Configures the producer to use Google Cloud Spanner as the outbox with relational database configuration.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance.</param>
    /// <param name="configure">An action to configure the <see cref="RelationalDatabaseConfigurationBuilder"/>.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    /// <remarks>
    /// This overload provides a simplified configuration approach for Spanner outbox using relational database settings.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.UseSpannerOutbox(config => config
    ///     .SetConnectionString("Data Source=projects/my-project/instances/my-instance/databases/my-database"));
    /// </code>
    /// </example>
    public static ProducerBuilder UseSpannerOutbox(this ProducerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UseSpannerOutbox(configuration.Build());
    }

    /// <summary>
    /// Configures the producer to use Google Cloud Spanner as the outbox with a pre-built configuration.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance.</param>
    /// <param name="configuration">The <see cref="RelationalDatabaseConfiguration"/> for Spanner.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    public static ProducerBuilder UseSpannerOutbox(this ProducerBuilder builder,
        RelationalDatabaseConfiguration configuration)
    {
        return builder.UseSpannerOutbox(cfg => cfg.SetConfiguration(configuration));
    }

    /// <summary>
    /// Configures the producer to use Google Cloud Spanner as the outbox with detailed builder configuration.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance.</param>
    /// <param name="configuration">An action to configure the <see cref="SpannerOutboxBuilder"/>.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    /// <remarks>
    /// This is the most flexible overload, allowing full control over Spanner outbox configuration.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.UseSpannerOutbox(outbox => outbox
    ///     .SetConfiguration(spannerConfig));
    /// </code>
    /// </example>
    public static ProducerBuilder UseSpannerOutbox(this ProducerBuilder builder,
        Action<SpannerOutboxBuilder> configuration)
    {
        var outbox = new SpannerOutboxBuilder();
        configuration(outbox);
        builder.SetOutbox(outbox.Build());
        return builder;
    }

    #endregion

    #endregion


    #region Distributed lock

    /// <summary>
    /// Configures the producer to use Google Cloud Firestore as the distributed lock provider.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance.</param>
    /// <param name="configure">An action to configure the <see cref="FirestoreLockingBuilder"/>.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    /// <remarks>
    /// Distributed locks are used to coordinate access to shared resources across multiple application instances.
    /// Firestore's transactional guarantees ensure that only one process can acquire a lock at a time.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.UseFirestoreDistributedLock(lock => lock
    ///     .SetConfiguration(firestoreConfig));
    /// </code>
    /// </example>
    public static ProducerBuilder UseFirestoreDistributedLock(this ProducerBuilder builder,
        Action<FirestoreLockingBuilder> configure)
    {
        var locking = new FirestoreLockingBuilder();
        configure(locking);
        return builder.UseFirestoreDistributedLock(locking.Build());
    }

    /// <summary>
    /// Configures the producer to use a pre-built Firestore distributed lock instance.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance.</param>
    /// <param name="locking">The <see cref="FirestoreDistributedLock"/> instance to use.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    /// <remarks>
    /// Use this overload when you have already created and configured a <see cref="FirestoreDistributedLock"/> instance.
    /// </remarks>
    public static ProducerBuilder UseFirestoreDistributedLock(this ProducerBuilder builder, FirestoreDistributedLock locking)
    {
        return builder.SetDistributedLock(locking);
    }

    #endregion
    
}