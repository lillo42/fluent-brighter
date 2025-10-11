using System;

using Fluent.Brighter.MongoDb;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="MongoDbLockingBuilder"/> to simplify configuration
/// of MongoDB connection settings and distributed locking collection options using fluent delegates or simple parameters.
/// </summary>
public static class MongoDbLockingBuilderExtensions
{
    /// <summary>
    /// Configures the MongoDB connection for distributed locking using a fluent configuration delegate.
    /// This creates a new <see cref="MongoDbConfiguration"/> internally and sets it on the locking builder.
    /// </summary>
    /// <param name="builder">The <see cref="MongoDbLockingBuilder"/> to extend.</param>
    /// <param name="configure">An action that configures a <see cref="MongoDbConfigurationBuilder"/>.</param>
    /// <returns>The same <see cref="MongoDbLockingBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="configure"/> is null.
    /// </exception>
    public static MongoDbLockingBuilder SetConfiguration(this MongoDbLockingBuilder builder,
        Action<MongoDbConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var configuration = new MongoDbConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }

    /// <summary>
    /// Sets the distributed locking collection by name only, using default collection settings.
    /// </summary>
    /// <param name="builder">The <see cref="MongoDbLockingBuilder"/> to extend.</param>
    /// <param name="collectionName">The name of the MongoDB collection to use for distributed locks.</param>
    /// <returns>The same <see cref="MongoDbLockingBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="collectionName"/> is null or empty.
    /// </exception>
    public static MongoDbLockingBuilder SetCollection(this MongoDbLockingBuilder builder, string collectionName)
    {
        if (string.IsNullOrEmpty(collectionName))
        {
            throw new ArgumentException("Collection name cannot be null or empty.", nameof(collectionName));
        }
        
        return builder.SetCollection(new MongoDbCollectionConfiguration { Name = collectionName });
    }

    /// <summary>
    /// Configures the distributed locking collection using a fluent collection builder for advanced settings
    /// such as TTL (for automatic lock expiration), custom indexes, or collection creation options.
    /// </summary>
    /// <param name="builder">The <see cref="MongoDbLockingBuilder"/> to extend.</param>
    /// <param name="configure">An action that configures a <see cref="MongoDbCollectionConfigurationBuilder"/>.</param>
    /// <returns>The same <see cref="MongoDbLockingBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="configure"/> is null.
    /// </exception>
    public static MongoDbLockingBuilder SetCollection(this MongoDbLockingBuilder builder,
        Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var collection = new MongoDbCollectionConfigurationBuilder();
        configure(collection);
        return builder.SetCollection(collection.Build());
    }
}