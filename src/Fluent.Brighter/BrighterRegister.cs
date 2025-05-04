using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Paramore.Brighter;
using Paramore.Brighter.Transforms.Transformers;

using Polly.Registry;

namespace Fluent.Brighter;

internal class BrighterRegister(IServiceCollection services, BrighterOptions options) : IBrighterRegister
{
    private readonly BusConfiguration _configuration = new();

    public IServiceCollection Services { get; } = services;

    public BrighterOptions Options { get; } = options;

    public IBrighterRegister AddExternalBus(IAmAProducerRegistry producerRegistry)
    {
        _ = Services.AddSingleton(producerRegistry);

        _configuration.Type = ExternalBusType.FireAndForget;
        return this;
    }

    public void AddBrighter()
    {
        Services.TryAddSingleton<IPolicyRegistry<string>>(new DefaultPolicy());
        Services.TryAddSingleton<IAmAnOutboxSync<Message>>(new InMemoryOutbox());
        Services.TryAddSingleton<IAmAnOutboxAsync<Message>>(new InMemoryOutbox());

        Services.Add(new ServiceDescriptor(typeof(IAmACommandProcessor), BuildCommandFactory, Options.CommandProcessorLifetime));
        _ = Services.AddTransient<ServiceProviderHandlerFactory>()
            .AddTransient<ServiceProviderMapperFactory>()
            .AddTransient<ServiceProviderMapperFactory>()
            .AddTransient<ServiceProviderTransformerFactory>()
            ;

        _ = this.AddMessageTransform<ClaimCheckTransformer>()
            .AddMessageTransform<CompressPayloadTransformer>();
    }

    private static IAmACommandProcessor BuildCommandFactory(IServiceProvider provider)
    {
        // var featureSwitchRegistry = provider.GetService<IAmAFeatureSwitchRegistry>();
        // var policyRegistry = provider.GetRequiredService<IPolicyRegistry<string>>();
        // var subscriberRegistry = provider.GetRequiredService<IAmASubscriberRegistry>();
        // var handlerFactory = provider.GetRequiredService<IAmAHandlerFactory>();
        // var requestFactory = provider.GetRequiredService<IAmARequestContextFactory>();
        // var producerRegistry = provider.GetService<IAmAProducerRegistry>();
        // if (provider == null)
        // {
        //     return new CommandProcessor(
        //             featureSwitchRegistry: featureSwitchRegistry,
        //             handlerFactory: handlerFactory,
        //             policyRegistry: policyRegistry,
        //             requestContextFactory: requestFactory,
        //             subscriberRegistry: subscriberRegistry
        //             );
        // }
        //
        // var mapperRegistry = provider.GetService<IAmAMessageMapperRegistry>();
        // var transformRegistry = provider.GetService<IAmAMessageTransformerFactory>();
        // var outbox = provider.GetRequiredService<IAmAnOutbox<Message>>();
        // var inbox = provider.GetService<InboxConfiguration>();
        // var channelFactory = provider.GetService<IAmAChannelFactory>();
        // var transaction = provider.GetService<IAmABoxTransactionConnectionProvider>();
        //
        // if (producerRegistry != null)
        // {
        //     return new CommandProcessor(
        //             featureSwitchRegistry: featureSwitchRegistry,
        //             handlerFactory: handlerFactory,
        //             policyRegistry: policyRegistry,
        //             requestContextFactory: requestFactory,
        //             subscriberRegistry: subscriberRegistry,
        //             mapperRegistry: mapperRegistry,
        //             producerRegistry: producerRegistry,
        //             outBox: outbox,
        //             messageTransformerFactory: transformRegistry,
        //             // boxTransactionConnectionProvider:
        //             inboxConfiguration: inbox
        //             );
        //
        // }
        //
        return null!;
    }

    public IBrighterRegister AddChannelFactory(IAmAChannelFactory channelFactory, IEnumerable<Subscription> subscriptions)
    {
        throw new NotImplementedException();
    }
}