using System;

using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.Transforms.Storage;

namespace Fluent.Brighter;

public sealed class LuggageStoreBuilder
{
    private Action<IBrighterBuilder> _store = _ => {};

    public LuggageStoreBuilder EnableLuggageStore<TStoreProvider>()
        where TStoreProvider : class, IAmAStorageProvider, IAmAStorageProviderAsync
    {
        _store = fluent => fluent.UseExternalLuggageStore<TStoreProvider>();
        return this;
    }
    
    public LuggageStoreBuilder EnableLuggageStore<TStoreProvider>(TStoreProvider storeProvider)
        where TStoreProvider : class, IAmAStorageProvider, IAmAStorageProviderAsync
    {
        _store = fluent => fluent.UseExternalLuggageStore(storeProvider);
        return this;
    }
    
    public LuggageStoreBuilder EnableLuggageStore<TStoreProvider>(Func<IServiceProvider, TStoreProvider> storeProvider)
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