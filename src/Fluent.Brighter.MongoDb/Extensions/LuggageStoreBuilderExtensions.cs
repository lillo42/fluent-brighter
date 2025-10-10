using System;

using Fluent.Brighter.MongoDb;

using Paramore.Brighter.Transformers.MongoGridFS;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="LuggageStoreBuilder"/> to configure a MongoDB GridFS-based
/// luggage store for storing large message payloads (e.g., attachments, binary data) in Brighter.
/// </summary>
public static class LuggageStoreBuilderExtensions
{
    /// <summary>
    /// Configures a MongoDB GridFS luggage store using a fluent builder delegate for advanced setup
    /// (e.g., custom bucket name, connection settings, or GridFS options).
    /// </summary>
    /// <param name="builder">The <see cref="LuggageStoreBuilder"/> to extend.</param>
    /// <param name="configure">An action that configures a <see cref="MongoDbLuggageStoreBuilder"/>.</param>
    /// <returns>The same <see cref="LuggageStoreBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="configure"/> is null.
    /// </exception>
    public static LuggageStoreBuilder UseMongoGridFsLuggageStore(this LuggageStoreBuilder builder,
        Action<MongoDbLuggageStoreBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var store = new MongoDbLuggageStoreBuilder();
        configure(store);
        return builder.UseMongoGridFsLuggageStore(store.Build());
    }

    /// <summary>
    /// Registers a pre-configured MongoDB GridFS luggage store instance.
    /// Useful when the store is built externally or shared across components.
    /// </summary>
    /// <param name="builder">The <see cref="LuggageStoreBuilder"/> to extend.</param>
    /// <param name="store">An instance of <see cref="MongoDbLuggageStore"/> representing the configured GridFS store.</param>
    /// <returns>The same <see cref="LuggageStoreBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="store"/> is null.
    /// </exception>
    public static LuggageStoreBuilder UseMongoGridFsLuggageStore(this LuggageStoreBuilder builder, MongoDbLuggageStore store)
    {
        if (store == null)
        {
            throw new ArgumentNullException(nameof(store));
        }
        
        builder.UseLuggageStore(store);
        return builder;
    }
}