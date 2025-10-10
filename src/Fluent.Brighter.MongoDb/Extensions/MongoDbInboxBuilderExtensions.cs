using System;

using Fluent.Brighter.MongoDb;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter;


/// <summary>
/// Provides extension methods for <see cref="MongoDbInboxBuilder"/> to simplify configuration
/// of MongoDB connection settings and inbox collection options using fluent delegates or simple parameters.
/// </summary>
public static class MongoDbInboxBuilderExtensions
{
    /// <summary>
    /// Configures the MongoDB connection for the inbox using a fluent configuration delegate.
    /// This creates a new <see cref="MongoDbConfiguration"/> internally and sets it on the inbox builder.
    /// </summary>
    /// <param name="builder">The <see cref="MongoDbInboxBuilder"/> to extend.</param>
    /// <param name="configure">An action that configures a <see cref="MongoDbConfigurationBuilder"/>.</param>
    /// <returns>The same <see cref="MongoDbInboxBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="configure"/> is null.
    /// </exception>
    public static MongoDbInboxBuilder SetConfiguration(this MongoDbInboxBuilder builder,
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
    /// Sets the inbox collection by name only, using default collection settings.
    /// </summary>
    /// <param name="builder">The <see cref="MongoDbInboxBuilder"/> to extend.</param>
    /// <param name="collectionName">The name of the MongoDB collection to use for the inbox.</param>
    /// <returns>The same <see cref="MongoDbInboxBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="collectionName"/> is null or empty.
    /// </exception>
    public static MongoDbInboxBuilder SetCollection(this MongoDbInboxBuilder builder, string collectionName)
    {
        return builder.SetCollection(new MongoDbCollectionConfiguration { Name = collectionName });
    }

    /// <summary>
    /// Configures the inbox collection using a fluent collection builder for advanced settings
    /// such as TTL, custom indexes, or collection creation options.
    /// </summary>
    /// <param name="builder">The <see cref="MongoDbInboxBuilder"/> to extend.</param>
    /// <param name="configure">An action that configures a <see cref="MongoDbCollectionConfigurationBuilder"/>.</param>
    /// <returns>The same <see cref="MongoDbInboxBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="configure"/> is null.
    /// </exception>
    public static MongoDbInboxBuilder SetCollection(this MongoDbInboxBuilder builder,
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