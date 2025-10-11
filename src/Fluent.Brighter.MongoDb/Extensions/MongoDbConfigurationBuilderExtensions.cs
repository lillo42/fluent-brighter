using System;

using Fluent.Brighter.MongoDb;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="MongoDbConfigurationBuilder"/> to simplify configuration
/// of inbox, outbox, and locking collections using either a collection name or a fluent collection builder.
/// </summary>
public static class MongoDbConfigurationBuilderExtensions
{
    #region Outbox

    /// <summary>
    /// Configures the outbox collection by name only, using default collection settings.
    /// </summary>
    /// <param name="builder">The <see cref="MongoDbConfigurationBuilder"/> to extend.</param>
    /// <param name="collectionName">The name of the MongoDB collection to use for the outbox.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="collectionName"/> is null or empty.</exception>
    public static MongoDbConfigurationBuilder SetOutbox(this MongoDbConfigurationBuilder builder, string collectionName)
    {
        if (string.IsNullOrEmpty(collectionName))
        {
            throw new ArgumentException("Collection name cannot be null or empty.", nameof(collectionName));
        }
        
        return builder.SetOutbox(new MongoDbCollectionConfiguration { Name = collectionName });
    }

    /// <summary>
    /// Configures the outbox collection using a fluent builder for advanced settings (e.g., TTL, indexes, creation options).
    /// </summary>
    /// <param name="builder">The <see cref="MongoDbConfigurationBuilder"/> to extend.</param>
    /// <param name="configure">An action that configures a <see cref="MongoDbCollectionConfigurationBuilder"/>.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public static MongoDbConfigurationBuilder SetOutbox(this MongoDbConfigurationBuilder builder,
        Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var collection = new MongoDbCollectionConfigurationBuilder();
        configure(collection);
        return builder.SetOutbox(collection.Build());
    }

    #endregion

    #region Inbox

    /// <summary>
    /// Configures the inbox collection by name only, using default collection settings.
    /// </summary>
    /// <param name="builder">The <see cref="MongoDbConfigurationBuilder"/> to extend.</param>
    /// <param name="collectionName">The name of the MongoDB collection to use for the inbox.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="collectionName"/> is null or empty.</exception>
    public static MongoDbConfigurationBuilder SetInbox(this MongoDbConfigurationBuilder builder, string collectionName)
    {
        if (string.IsNullOrEmpty(collectionName))
        {
            throw new ArgumentException("Collection name cannot be null or empty.", nameof(collectionName));
        }
        
        return builder.SetInbox(new MongoDbCollectionConfiguration { Name = collectionName });
    }

    /// <summary>
    /// Configures the inbox collection using a fluent builder for advanced settings (e.g., TTL, custom indexes).
    /// </summary>
    /// <param name="builder">The <see cref="MongoDbConfigurationBuilder"/> to extend.</param>
    /// <param name="configure">An action that configures a <see cref="MongoDbCollectionConfigurationBuilder"/>.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public static MongoDbConfigurationBuilder SetInbox(this MongoDbConfigurationBuilder builder,
        Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var collection = new MongoDbCollectionConfigurationBuilder();
        configure(collection);
        return builder.SetInbox(collection.Build());
    }

    #endregion

    #region Locking

    /// <summary>
    /// Configures the distributed locking collection by name only, using default collection settings.
    /// </summary>
    /// <param name="builder">The <see cref="MongoDbConfigurationBuilder"/> to extend.</param>
    /// <param name="collectionName">The name of the MongoDB collection to use for distributed locks.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="collectionName"/> is null or empty.</exception>
    public static MongoDbConfigurationBuilder SetLocking(this MongoDbConfigurationBuilder builder, string collectionName)
    {
        if (string.IsNullOrEmpty(collectionName))
        {
            throw new ArgumentException("Collection name cannot be null or empty.", nameof(collectionName));
        }
        
        return builder.SetLocking(new MongoDbCollectionConfiguration { Name = collectionName });
    }

    /// <summary>
    /// Configures the distributed locking collection using a fluent builder for advanced settings (e.g., TTL for lock expiration).
    /// </summary>
    /// <param name="builder">The <see cref="MongoDbConfigurationBuilder"/> to extend.</param>
    /// <param name="configure">An action that configures a <see cref="MongoDbCollectionConfigurationBuilder"/>.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public static MongoDbConfigurationBuilder SetLocking(this MongoDbConfigurationBuilder builder,
        Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var collection = new MongoDbCollectionConfigurationBuilder();
        configure(collection);
        return builder.SetLocking(collection.Build());
    }

    #endregion
}