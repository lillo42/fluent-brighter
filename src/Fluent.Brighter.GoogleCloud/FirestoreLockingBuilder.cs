using System;

using Paramore.Brighter.Firestore;
using Paramore.Brighter.Locking.Firestore;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for creating instances of <see cref="FirestoreDistributedLock"/>.
/// </summary>
public class FirestoreLockingBuilder
{
    private IAmAFirestoreConnectionProvider? _connectionProvider;
    
    /// <summary>
    /// Sets the Firestore connection provider.
    /// </summary>
    /// <param name="connectionProvider">The connection provider to use for Firestore operations.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FirestoreLockingBuilder SetConnectionProvider(IAmAFirestoreConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    private FirestoreConfiguration? _configuration;
    
    /// <summary>
    /// Sets the Firestore configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for connecting to Firestore.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FirestoreLockingBuilder SetConfiguration(FirestoreConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    /// <summary>
    /// Builds a new instance of <see cref="FirestoreDistributedLock"/> using the configured settings.
    /// </summary>
    /// <returns>A configured <see cref="FirestoreDistributedLock"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when configuration is not set.</exception>
    internal FirestoreDistributedLock Build()
    {
        if (_configuration == null)
        {
            throw new InvalidOperationException("Configuration must be set before building.");
        }

        return _connectionProvider != null
            ? new FirestoreDistributedLock(_connectionProvider, _configuration)
            : new FirestoreDistributedLock(_configuration);
    }
}