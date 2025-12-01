using System;

using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore.V1;

using Paramore.Brighter;
using Paramore.Brighter.Firestore;
using Paramore.Brighter.Observability;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for fluently configuring a Firestore configuration for Paramore.Brighter.
/// Provides methods to set project ID, database, collections, credentials, and other Firestore settings.
/// </summary>
public sealed class FirestoreConfigurationBuilder
{
    private string? _projectId;

    /// <summary>
    /// Sets the Google Cloud project ID.
    /// </summary>
    /// <param name="projectId">The Google Cloud project ID</param>
    /// <returns>The builder instance for method chaining</returns>
    public FirestoreConfigurationBuilder SetProjectId(string projectId)
    {
        _projectId = projectId;
        return this;
    }

    private string? _database;

    /// <summary>
    /// Sets the Firestore database ID (e.g., "(default)").
    /// </summary>
    /// <param name="database">The Firestore database ID</param>
    /// <returns>The builder instance for method chaining</returns>
    public FirestoreConfigurationBuilder SetDatabase(string database)
    {
        _database = database;
        return this;
    }

    private FirestoreCollection? _inbox;

    /// <summary>
    /// Sets the default inbox Firestore collection.
    /// </summary>
    /// <param name="inbox">The inbox Firestore collection</param>
    /// <returns>The builder instance for method chaining</returns>
    public FirestoreConfigurationBuilder SetInbox(FirestoreCollection inbox)
    {
        _inbox = inbox;
        return this;
    }

    private FirestoreCollection? _outbox;

    /// <summary>
    /// Sets the default outbox Firestore collection.
    /// </summary>
    /// <param name="outbox">The outbox Firestore collection</param>
    /// <returns>The builder instance for method chaining</returns>
    public FirestoreConfigurationBuilder SetOutbox(FirestoreCollection outbox)
    {
        _outbox = outbox;
        return this;
    }

    private FirestoreCollection? _locking;

    /// <summary>
    /// Sets the default locking Firestore collection.
    /// </summary>
    /// <param name="locking">The locking Firestore collection</param>
    /// <returns>The builder instance for method chaining</returns>
    public FirestoreConfigurationBuilder SetLocking(FirestoreCollection locking)
    {
        _locking = locking;
        return this;
    }

    private TimeProvider _timeProvider = TimeProvider.System;

    /// <summary>
    /// Sets the <see cref="TimeProvider"/> to use for timestamp generation.
    /// Defaults to <see cref="TimeProvider.System"/>.
    /// </summary>
    /// <param name="timeProvider">The time provider</param>
    /// <returns>The builder instance for method chaining</returns>
    public FirestoreConfigurationBuilder SetTimeProvider(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        return this;
    }

    private ICredential? _credential;

    /// <summary>
    /// Sets the Google credential to use for authentication.
    /// If not set, Application Default Credentials will be used.
    /// </summary>
    /// <param name="credential">The Google credential</param>
    /// <returns>The builder instance for method chaining</returns>
    public FirestoreConfigurationBuilder SetCredential(ICredential? credential)
    {
        _credential = credential;
        return this;
    }

    private InstrumentationOptions _instrumentation = InstrumentationOptions.All;

    /// <summary>
    /// Sets the instrumentation options for tracing.
    /// </summary>
    /// <param name="instrumentation">The instrumentation options</param>
    /// <returns>The builder instance for method chaining</returns>
    public FirestoreConfigurationBuilder SetInstrumentation(InstrumentationOptions instrumentation)
    {
        _instrumentation = instrumentation;
        return this;
    }

    private Action<FirestoreClientBuilder>? _configure;

    /// <summary>
    /// Sets an action to configure the <see cref="FirestoreClientBuilder"/>
    /// before building the <see cref="FirestoreClient"/>. This allows for advanced
    /// customization of the client.
    /// </summary>
    /// <param name="configure">The configuration action</param>
    /// <returns>The builder instance for method chaining</returns>
    public FirestoreConfigurationBuilder SetConfigure(Action<FirestoreClientBuilder> configure)
    {
        _configure = configure;
        return this;
    }

    /// <summary>
    /// Builds the FirestoreConfiguration instance with the configured options.
    /// </summary>
    /// <returns>A configured <see cref="FirestoreConfiguration"/> instance</returns>
    /// <exception cref="ConfigurationException">Thrown if project ID or database is not set</exception>
    internal FirestoreConfiguration Build()
    {
        if (string.IsNullOrEmpty(_projectId))
        {
            throw new ConfigurationException("Project ID is null or empty");
        }

        if (string.IsNullOrEmpty(_database))
        {
            throw new ConfigurationException("Database is null or empty");
        }

        return new FirestoreConfiguration(_projectId!, _database!)
        {
            Inbox = _inbox,
            Outbox = _outbox,
            Locking = _locking,
            TimeProvider = _timeProvider,
            Credential = _credential,
            Instrumentation = _instrumentation,
            Configure = _configure
        };
    }
}

