using System;

using MongoDB.Driver;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter.MongoDb;

/// <summary>
/// Provides a fluent interface for configuring a MongoDB collection used by Brighter features
/// such as the inbox, outbox, or distributed locking.
/// </summary>
public sealed class MongoDbCollectionConfigurationBuilder
{
    private readonly MongoDbCollectionConfiguration _configuration = new();

    /// <summary>
    /// Sets the name of the MongoDB collection.
    /// </summary>
    /// <param name="name">The collection name. Must not be null or empty.</param>
    /// <returns>The current <see cref="MongoDbCollectionConfigurationBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> is null or empty.</exception>
    public MongoDbCollectionConfigurationBuilder SetName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Collection name cannot be null or empty.", nameof(name));
        _configuration.Name = name;
        return this;
    }

    /// <summary>
    /// Specifies a delegate that customizes how the collection is resolved or created at runtime.
    /// This allows advanced control over collection initialization (e.g., applying custom indexes or validation rules).
    /// </summary>
    /// <param name="makeCollection">A delegate of type <see cref="OnResolvingACollection"/> that configures the collection.</param>
    /// <returns>The current <see cref="MongoDbCollectionConfigurationBuilder"/> instance for method chaining.</returns>
    public MongoDbCollectionConfigurationBuilder SetMakeCollection(OnResolvingACollection makeCollection)
    {
        _configuration.MakeCollection = makeCollection;
        return this;
    }

    /// <summary>
    /// Sets the MongoDB collection settings (e.g., read/write concerns, collation).
    /// </summary>
    /// <param name="settings">The <see cref="MongoCollectionSettings"/> to apply to the collection.</param>
    /// <returns>The current <see cref="MongoDbCollectionConfigurationBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="settings"/> is null.</exception>
    public MongoDbCollectionConfigurationBuilder SetSettings(MongoCollectionSettings settings)
    {
        _configuration.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        return this;
    }

    /// <summary>
    /// Configures options used when creating the collection (e.g., capped collections, time-series options).
    /// </summary>
    /// <param name="createOptions">The <see cref="CreateCollectionOptions"/> to use during collection creation.</param>
    /// <returns>The current <see cref="MongoDbCollectionConfigurationBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="createOptions"/> is null.</exception>
    public MongoDbCollectionConfigurationBuilder SetCreateCollectionOptions(CreateCollectionOptions createOptions)
    {
        _configuration.CreateCollectionOptions = createOptions ?? throw new ArgumentNullException(nameof(createOptions));
        return this;
    }

    /// <summary>
    /// Sets a time-to-live (TTL) for documents in the collection, enabling automatic expiration.
    /// When specified, Brighter will configure a TTL index on the appropriate timestamp field.
    /// </summary>
    /// <param name="timeToLive">
    /// The duration after which documents should expire, or <see langword="null"/> to disable TTL.
    /// </param>
    /// <returns>The current <see cref="MongoDbCollectionConfigurationBuilder"/> instance for method chaining.</returns>
    public MongoDbCollectionConfigurationBuilder SetTimeToLive(TimeSpan? timeToLive)
    {
        _configuration.TimeToLive = timeToLive;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured <see cref="MongoDbCollectionConfiguration"/> instance.
    /// </summary>
    /// <returns>A fully configured <see cref="MongoDbCollectionConfiguration"/>.</returns>
    internal MongoDbCollectionConfiguration Build()
    {
        return _configuration;
    }
}