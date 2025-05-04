using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Fluent.Brighter;

internal class BrighterConfigurator(IServiceCollection services, BrighterConfiguratorOptions options) : IBrighterConfigurator
{
    private readonly List<IAmAMessageProducerFactory> _producerRegistries = [];
    private readonly Dictionary<Subscription, IAmAChannelFactory> _subscriptions = [];

    public IServiceCollection Services { get; } = services;
    public BrighterConfiguratorOptions Options { get; } = options;
    public Action<IAmAnAsyncSubcriberRegistry> AsyncHandlerRegistry { get; set; } = static _ => { };
    public Action<IAmASubscriberRegistry> HandlerRegistry { get; set; } = static _ => { };
    public Action<ServiceCollectionMessageMapperRegistry> MapperRegistry { get; set; } = static _ => { };
    public AutoFromAssembly FromAssembly { get; set; }


    internal IAmAProducerRegistry? ProducerRegistry => _producerRegistries.Count == 0 ? null : new CombinedProducerRegistryFactory([.. _producerRegistries]).Create();
    internal IEnumerable<Subscription> Subscriptions => _subscriptions.Keys;
    internal IAmAChannelFactory ChannelFactory => new CombinedChannelFactory(_subscriptions);

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
}