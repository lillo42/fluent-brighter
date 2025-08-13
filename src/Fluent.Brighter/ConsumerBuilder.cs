using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;
using Paramore.Brighter.FeatureSwitch;
using Paramore.Brighter.Observability;
using Paramore.Brighter.ServiceActivator.Extensions.DependencyInjection;

using Polly;
using Polly.Registry;

namespace Fluent.Brighter;

public sealed class ConsumerBuilder
{
    private ServiceLifetime _commandProcessorLifetime = ServiceLifetime.Transient;

    public ConsumerBuilder SetCommandProcessorLifetime(ServiceLifetime commandProcessorLifetime)
    {
        _commandProcessorLifetime = commandProcessorLifetime;
        return this;
    }

    private ServiceLifetime _handlerLifetime = ServiceLifetime.Transient;
    public ConsumerBuilder SetHandlerLifetime(ServiceLifetime handlerLifetime)
    {
        _handlerLifetime = handlerLifetime;
        return this;
    }

    private ServiceLifetime _mapperLifetime = ServiceLifetime.Transient;

    public ConsumerBuilder SetMapperLifetime(ServiceLifetime mapperLifetime)
    {
        _mapperLifetime = mapperLifetime;
        return this;
    }

    private ServiceLifetime _transformerLifetime = ServiceLifetime.Transient;

    public ConsumerBuilder SetTransformerLifetime(ServiceLifetime transformerLifetime)
    {
        _transformerLifetime = transformerLifetime;
        return this;
    }

    private InstrumentationOptions _instrumentation = InstrumentationOptions.All;

    public ConsumerBuilder SetInstrumentation(InstrumentationOptions options)
    {
        _instrumentation = options;
        return this;
    }

    private IAmAFeatureSwitchRegistry? _featureSwitchRegistry;

    public ConsumerBuilder SetFeatureSwitchRegistry(IAmAFeatureSwitchRegistry? featureSwitchRegistry)
    {
        _featureSwitchRegistry = featureSwitchRegistry;
        return this;
    }

    private IAmARequestContextFactory _requestContextFactory = new InMemoryRequestContextFactory();

    public ConsumerBuilder SetRequestContextFactory(IAmARequestContextFactory requestContextFactory)
    {
        _requestContextFactory = requestContextFactory;
        return this;
    }

    private IPolicyRegistry<string> _policyRegistry = new DefaultPolicy();

    public ConsumerBuilder SetPolicyRegistry(IPolicyRegistry<string> policyRegistry)
    {
        _policyRegistry = policyRegistry;
        return this;
    }

    public ConsumerBuilder AddPolicy<TPolicy>(string key, TPolicy policy)
        where TPolicy : IsPolicy
    {
        _policyRegistry.Add(key, policy);
        return this;
    }

    private ResiliencePipelineRegistry<string> _resiliencePipelineRegistry = new ResiliencePipelineRegistry<string>()
        .AddBrighterDefault();

    public ConsumerBuilder SetResiliencePipelineRegistry(
        ResiliencePipelineRegistry<string> resiliencePipelineRegistry)
    {
        _resiliencePipelineRegistry = resiliencePipelineRegistry;
        return this;
    }

    public ConsumerBuilder AddResiliencePipeline<TPolicy>(string key,
        Action<ResiliencePipelineBuilder, ConfigureBuilderContext<string>> configure)
    {
        _resiliencePipelineRegistry.TryAddBuilder(key, configure);
        return this;
    }

    private IAmAChannelFactory? _defaultChannelFactory;
    public ConsumerBuilder SetDefaultChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _defaultChannelFactory = channelFactory;
        return this;
    }

    private List<IAmAChannelFactory> _channelFactories = [];
    public ConsumerBuilder AddChannelFactory(IAmAChannelFactory channelFactory)
    {
        _channelFactories.Add(channelFactory);
        return this;
    }
    
    private InboxConfiguration? _inboxConfiguration = new();
    public ConsumerBuilder SetInbox(InboxConfiguration inboxConfiguration)
    {
        _inboxConfiguration = inboxConfiguration;
        return this;
    }
    
    private InboxConfigurationBuilder _inboxConfigurationBuilder = new();
    public ConsumerBuilder SetInbox(Action<InboxConfigurationBuilder> configuration)
    {
        configuration(_inboxConfigurationBuilder);
        return this;
    }

    private List<Subscription> _subscriptions = new();

    public ConsumerBuilder SetSubscriptions(IEnumerable<Subscription> subscriptions)
    {
        _subscriptions = subscriptions.ToList();
        return this;
    }

    public ConsumerBuilder AddSubscription(Subscription subscription)
    {
        _subscriptions.Add(subscription);
        return this;
    }

    private Action<ConsumersOptions>? _configuration;

    public ConsumerBuilder SetConfiguration(Action<ConsumersOptions> configuration)
    {
        _configuration = configuration;
        return this;
    }
    
    internal void SetConsumerOptions(ConsumersOptions options)
    {
        if (_defaultChannelFactory == null && _channelFactories.Count > 0)
        {
            _defaultChannelFactory = new CombinedChannelFactory(_channelFactories);
        }
        
        options.CommandProcessorLifetime = _commandProcessorLifetime;
        options.HandlerLifetime = _handlerLifetime;
        options.MapperLifetime = _mapperLifetime;
        options.TransformerLifetime = _transformerLifetime;
        options.InstrumentationOptions = _instrumentation;
        options.FeatureSwitchRegistry = _featureSwitchRegistry;
        options.RequestContextFactory = _requestContextFactory;
        options.ResiliencePipelineRegistry = _resiliencePipelineRegistry;
#pragma warning disable CS0618 // Type or member is obsolete
        options.PolicyRegistry = _policyRegistry;
#pragma warning restore CS0618 // Type or member is obsolete
        
        options.DefaultChannelFactory = _defaultChannelFactory;
        options.InboxConfiguration = _inboxConfiguration ?? _inboxConfigurationBuilder.Build();
        options.Subscriptions = _subscriptions;
        
        _configuration?.Invoke(options);
    }
}