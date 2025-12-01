using System;
using System.Data.Common;

using Fluent.Brighter.GoogleCloud;

using Google.Cloud.Firestore.V1;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for FluentBrighterBuilder to provide GCP-specific configurations
/// and Firestore/Spanner outbox archiving functionality.
/// </summary>
public static class FluentBrighterExtensions
{
    /// <summary>
    /// Configures Google Cloud Platform services (Pub/Sub, Firestore, Spanner, GCS) for use with Paramore.Brighter
    /// using a fluent configuration pattern.
    /// </summary>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <param name="configure">Action to configure GCP services</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    public static FluentBrighterBuilder UsingGcp(this FluentBrighterBuilder builder,
        Action<GcpConfigurator> configure)
    {
        var configurator = new GcpConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }

    #region Outbox 

    /// <summary>
    /// Configures Firestore as the transaction outbox archive store using default settings.
    /// Note: Currently uses a null archive provider implementation.
    /// </summary>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    public static FluentBrighterBuilder UseFirestoreTransactionOutboxArchive(this FluentBrighterBuilder builder)
    {
        return builder.UseOutboxArchiver<CommitRequest>(new NullOutboxArchiveProvider());
    }

    /// <summary>
    /// Configures Firestore as the transaction outbox archive store with custom settings.
    /// Note: Currently uses a null archive provider implementation.
    /// </summary>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <param name="configure">Action to configure outbox archiver options</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    public static FluentBrighterBuilder UseFirestoreTransactionOutboxArchive(this FluentBrighterBuilder builder, Action<TimedOutboxArchiverOptionsBuilder> configure)
    {
        var options = new TimedOutboxArchiverOptionsBuilder();
        configure(options);
        return builder.UseOutboxArchiver<CommitRequest>(new NullOutboxArchiveProvider(), options.Build());
    }

    /// <summary>
    /// Configures Cloud Spanner as the transaction outbox archive store using default settings.
    /// Note: Currently uses a null archive provider implementation.
    /// </summary>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    public static FluentBrighterBuilder UseSpannerTransactionOutboxArchive(this FluentBrighterBuilder builder)
    {
        return builder.UseOutboxArchiver<DbTransaction>(new NullOutboxArchiveProvider());
    }

    /// <summary>
    /// Configures Cloud Spanner as the transaction outbox archive store with custom settings.
    /// Note: Currently uses a null archive provider implementation.
    /// </summary>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <param name="configure">Action to configure outbox archiver options</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    public static FluentBrighterBuilder UseSpannerTransactionOutboxArchive(this FluentBrighterBuilder builder, Action<TimedOutboxArchiverOptionsBuilder> configure)
    {
        var options = new TimedOutboxArchiverOptionsBuilder();
        configure(options);
        return builder.UseOutboxArchiver<DbTransaction>(new NullOutboxArchiveProvider(), options.Build());
    }

    #endregion
}