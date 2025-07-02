using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.Observability;
using Paramore.Brighter.Transforms.Storage;

namespace Fluent.Brighter;

internal class BrighterConfigurator(IServiceCollection services, BrighterConfiguratorOptions options) : IBrighterConfigurator
{
    private readonly List<IAmAMessageProducerFactory> _producerRegistries = [];
    private readonly Dictionary<Subscription, IAmAChannelFactory> _subscriptions = [];

    public IServiceCollection Services { get; } = services;
    public Dictionary<string, List<Assembly>> Assemblies { get; set; } = new();
    public BrighterConfiguratorOptions Options { get; } = options;
    public Action<IAmAnAsyncSubcriberRegistry> AsyncHandlerRegistry { get; set; } = static _ => { };
    public Action<IAmASubscriberRegistry> HandlerRegistry { get; set; } = static _ => { };
    public Action<ServiceCollectionMessageMapperRegistry> MapperRegistry { get; set; } = static _ => { };
    public AutoFromAssembly FromAssembly { get; set; } = AutoFromAssembly.All;

    internal IAmAProducerRegistry? ProducerRegistry => _producerRegistries.Count == 0 ? null : new CombinedProducerRegistryFactory([.. _producerRegistries]).Create();
    internal IEnumerable<Subscription> Subscriptions => _subscriptions.Keys;
    internal IAmAChannelFactory ChannelFactory => new CombinedChannelFactory(_subscriptions);
    
    internal BrighterOutboxConfiguration? OutboxConfiguration { get; set; }
    internal InboxConfiguration InboxConfiguration { get; set; } = new();
    internal IDistributedLock? DistributedLockConfiguration { get; set; }
    
    public IBrighterConfigurator AddExternalBus(IAmAMessageProducerFactory producerRegistry)
    {
        _producerRegistries.Add(producerRegistry);
        return this;
    }

    public IBrighterConfigurator AddChannelFactory(IAmAChannelFactory channelFactory, IEnumerable<Subscription> subscriptions)
    {
        foreach (var subscription in subscriptions)
        {
            _subscriptions[subscription] = channelFactory;
        }

        return this;
    }

    public IBrighterConfigurator SetOutbox(BrighterOutboxConfiguration configuration)
    {
        OutboxConfiguration = configuration;
        return this;
    }

    public IBrighterConfigurator SetInbox(InboxConfiguration configuration)
    {
        InboxConfiguration = configuration;
        return this;
    }

    public IBrighterConfigurator SetDistributedLock(IDistributedLock? distributedLock)
    {
        DistributedLockConfiguration = distributedLock;
        return this;
    }

    public IBrighterConfigurator SetLuggageStore<TStoreProvider>(TStoreProvider luggage)
        where TStoreProvider : class, IAmAStorageProvider, IAmAStorageProviderAsync
    {
        Services
            .AddSingleton(_ =>
            {
                luggage.EnsureStoreExistsAsync().GetAwaiter().GetResult();
                return luggage;
            })
            .AddSingleton(provider =>
            {
                IAmAStorageProvider store = provider.GetRequiredService<TStoreProvider>();
                store.Tracer = provider.GetRequiredService<IAmABrighterTracer>();
                return store;
            })
            .AddSingleton(provider =>
            {
                IAmAStorageProviderAsync store = provider.GetRequiredService<TStoreProvider>();
                store.Tracer = provider.GetRequiredService<IAmABrighterTracer>();
                return store;
            });
        return this;
    }
}