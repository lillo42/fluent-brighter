using Paramore.Brighter;
using Paramore.Brighter.Firestore;
using Paramore.Brighter.Outbox.Firestore;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for fluently configuring a Firestore outbox for Paramore.Brighter.
/// Provides methods to set Firestore configuration and connection provider for outbox operations.
/// </summary>
public sealed class FirestoreOutboxBuilder
{
    private FirestoreConfiguration? _configuration;
    
    /// <summary>
    /// Sets the Firestore configuration containing project ID, database, and collection settings.
    /// </summary>
    /// <param name="configuration">The Firestore configuration</param>
    /// <returns>The builder instance for method chaining</returns>
    public FirestoreOutboxBuilder SetConfiguration(FirestoreConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }


    private IAmAFirestoreConnectionProvider? _firestoreConnectionProvider;

    /// <summary>
    /// Sets the Firestore connection provider for managing database connections.
    /// If not set, a default <see cref="FirestoreConnectionProvider"/> will be created using the configuration.
    /// </summary>
    /// <param name="connectionProvider">The Firestore connection provider</param>
    /// <returns>The builder instance for method chaining</returns>
    public FirestoreOutboxBuilder SetConnectionProvider(IAmAFirestoreConnectionProvider connectionProvider)
    {
        _firestoreConnectionProvider = connectionProvider;
        return this;
    }
    
    /// <summary>
    /// Builds the FirestoreOutbox instance with the configured options.
    /// </summary>
    /// <returns>A configured <see cref="FirestoreOutbox"/> instance</returns>
    /// <exception cref="ConfigurationException">Thrown if configuration is not set</exception>
    internal FirestoreOutbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Firestore configuration is required");
        }

        _firestoreConnectionProvider ??= new FirestoreConnectionProvider(_configuration);
        return new FirestoreOutbox(_firestoreConnectionProvider, _configuration);
    }
}