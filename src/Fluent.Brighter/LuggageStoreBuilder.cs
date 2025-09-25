using System;

using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.Transforms.Storage;

namespace Fluent.Brighter;

/// <summary>
/// Builder class for configuring external luggage storage in Paramore.Brighter.
/// Provides methods to enable and configure storage providers for handling large messages
/// that exceed normal message size limits.
/// </summary>
public sealed class LuggageStoreBuilder
{
    private Action<IBrighterBuilder> _store = static _ => { };

    /// <summary>
    /// Use luggage storage using a specified storage provider type that will be resolved
    /// from the dependency injection container.
    /// </summary>
    /// <typeparam name="TStoreProvider">The type of storage provider implementing IAmAStorageProvider and IAmAStorageProviderAsync</typeparam>
    /// <returns>The builder instance for method chaining</returns>
    public LuggageStoreBuilder UseLuggageStore<TStoreProvider>()
        where TStoreProvider : class, IAmAStorageProvider, IAmAStorageProviderAsync
    {
        _store = static fluent => fluent.UseExternalLuggageStore<TStoreProvider>();
        return this;
    }

    /// <summary>
    /// Use luggage storage using a pre-configured storage provider instance.
    /// </summary>
    /// <typeparam name="TStoreProvider">The type of storage provider implementing IAmAStorageProvider and IAmAStorageProviderAsync</typeparam>
    /// <param name="storeProvider">Pre-configured storage provider instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public LuggageStoreBuilder UseLuggageStore<TStoreProvider>(TStoreProvider storeProvider)
        where TStoreProvider : class, IAmAStorageProvider, IAmAStorageProviderAsync
    {
        _store = fluent => fluent.UseExternalLuggageStore(storeProvider);
        return this;
    }

    /// <summary>
    /// Use luggage storage using a factory function to create the storage provider.
    /// </summary>
    /// <typeparam name="TStoreProvider">The type of storage provider implementing IAmAStorageProvider and IAmAStorageProviderAsync</typeparam>
    /// <param name="storeProvider">Factory function to create the storage provider from the service provider</param>
    /// <returns>The builder instance for method chaining</returns>
    public LuggageStoreBuilder UseLuggageStore<TStoreProvider>(Func<IServiceProvider, TStoreProvider> storeProvider)
        where TStoreProvider : class, IAmAStorageProvider, IAmAStorageProviderAsync
    {
        _store = fluent => fluent.UseExternalLuggageStore(storeProvider);
        return this;
    }

    internal void SetLuggageStore(IBrighterBuilder builder)
    {
        _store(builder);
    }
}