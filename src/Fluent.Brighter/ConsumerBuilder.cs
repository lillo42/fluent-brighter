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

/// <summary>
/// Fluent builder for configuring Brighter service activator consumers
/// </summary>
/// <remarks>
/// Provides a fluent interface to configure consumer options including lifetimes, 
/// instrumentation, policies, subscriptions, and channel factories.
/// </remarks>
public sealed class ConsumerBuilder
{
    private ServiceLifetime _handlerLifetime = ServiceLifetime.Transient;
    
    /// <summary>
    /// Sets the service lifetime for message handlers
    /// </summary>
    /// <param name="handlerLifetime">The service lifetime (default: Transient)</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder SetHandlerLifetime(ServiceLifetime handlerLifetime)
    {
        _handlerLifetime = handlerLifetime;
        return this;
    }

    private ServiceLifetime _mapperLifetime = ServiceLifetime.Transient;

    /// <summary>
    /// Sets the service lifetime for message mappers
    /// </summary>
    /// <param name="mapperLifetime">The service lifetime (default: Transient)</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder SetMapperLifetime(ServiceLifetime mapperLifetime)
    {
        _mapperLifetime = mapperLifetime;
        return this;
    }

    private ServiceLifetime _transformerLifetime = ServiceLifetime.Transient;

    /// <summary>
    /// Sets the service lifetime for message transformers
    /// </summary>
    /// <param name="transformerLifetime">The service lifetime (default: Transient)</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder SetTransformerLifetime(ServiceLifetime transformerLifetime)
    {
        _transformerLifetime = transformerLifetime;
        return this;
    }

    private InstrumentationOptions _instrumentation = InstrumentationOptions.All;

    /// <summary>
    /// Configures instrumentation options for monitoring
    /// </summary>
    /// <param name="options">Instrumentation configuration flags (default: All)</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder SetInstrumentation(InstrumentationOptions options)
    {
        _instrumentation = options;
        return this;
    }

    private IAmAFeatureSwitchRegistry? _featureSwitchRegistry;

    /// <summary>
    /// Sets the feature switch registry for enabling/disabling features
    /// </summary>
    /// <param name="featureSwitchRegistry">The feature switch registry instance</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder SetFeatureSwitchRegistry(IAmAFeatureSwitchRegistry? featureSwitchRegistry)
    {
        _featureSwitchRegistry = featureSwitchRegistry;
        return this;
    }

    private IAmARequestContextFactory _requestContextFactory = new InMemoryRequestContextFactory();

    /// <summary>
    /// Sets the request context factory implementation
    /// </summary>
    /// <param name="requestContextFactory">Factory for request contexts (default: InMemoryRequestContextFactory)</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder SetRequestContextFactory(IAmARequestContextFactory requestContextFactory)
    {
        _requestContextFactory = requestContextFactory;
        return this;
    }

    private IPolicyRegistry<string> _policyRegistry = new DefaultPolicy();

    /// <summary>
    /// Sets the policy registry for Polly policies
    /// </summary>
    /// <remarks>Obsolete: Prefer resilience pipelines for new development</remarks>
    /// <param name="policyRegistry">The policy registry instance</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder SetPolicyRegistry(IPolicyRegistry<string> policyRegistry)
    {
        _policyRegistry = policyRegistry;
        return this;
    }

    /// <summary>
    /// Adds a Polly policy to the registry
    /// </summary>
    /// <typeparam name="TPolicy">The policy type (IsPolicy)</typeparam>
    /// <param name="key">The policy lookup key</param>
    /// <param name="policy">The policy instance</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder AddPolicy<TPolicy>(string key, TPolicy policy)
        where TPolicy : IsPolicy
    {
        _policyRegistry.Add(key, policy);
        return this;
    }

    private ResiliencePipelineRegistry<string> _resiliencePipelineRegistry = new ResiliencePipelineRegistry<string>()
        .AddBrighterDefault();

    /// <summary>
    /// Sets the resilience pipeline registry for Polly v8 pipelines
    /// </summary>
    /// <param name="resiliencePipelineRegistry">The pipeline registry instance</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder SetResiliencePipelineRegistry(
        ResiliencePipelineRegistry<string> resiliencePipelineRegistry)
    {
        _resiliencePipelineRegistry = resiliencePipelineRegistry;
        return this;
    }

    /// <summary>
    /// Adds a resilience pipeline to the registry
    /// </summary>
    /// <typeparam name="TPolicy">The policy type</typeparam>
    /// <param name="key">The pipeline lookup key</param>
    /// <param name="configure">Configuration action for the pipeline builder</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder AddResiliencePipeline<TPolicy>(string key,
        Action<ResiliencePipelineBuilder, ConfigureBuilderContext<string>> configure)
    {
        _resiliencePipelineRegistry.TryAddBuilder(key, configure);
        return this;
    }

    private IAmAChannelFactory? _defaultChannelFactory;
    
    /// <summary>
    /// Sets the default channel factory for message consumption
    /// </summary>
    /// <param name="channelFactory">The channel factory implementation</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder SetDefaultChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _defaultChannelFactory = channelFactory;
        return this;
    }

    private readonly List<IAmAChannelFactory> _channelFactories = [];
    
    /// <summary>
    /// Adds a channel factory to the configuration
    /// </summary>
    /// <param name="channelFactory">The channel factory implementation</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder AddChannelFactory(IAmAChannelFactory channelFactory)
    {
        _channelFactories.Add(channelFactory);
        return this;
    }
    
    private InboxConfiguration? _inboxConfiguration = new();
    
    /// <summary>
    /// Configures the inbox settings for idempotency
    /// </summary>
    /// <param name="inboxConfiguration">The inbox configuration</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder SetInbox(InboxConfiguration inboxConfiguration)
    {
        _inboxConfiguration = inboxConfiguration;
        return this;
    }
    
    private readonly InboxConfigurationBuilder _inboxConfigurationBuilder = new();
    
    /// <summary>
    /// Configures the inbox using a builder action
    /// </summary>
    /// <param name="configuration">Action to configure inbox settings</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder SetInbox(Action<InboxConfigurationBuilder> configuration)
    {
        configuration(_inboxConfigurationBuilder);
        return this;
    }

    private List<Subscription> _subscriptions = new();

    /// <summary>
    /// Sets the collection of subscriptions
    /// </summary>
    /// <param name="subscriptions">Enumerable collection of subscriptions</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder SetSubscriptions(IEnumerable<Subscription> subscriptions)
    {
        _subscriptions = subscriptions.ToList();
        return this;
    }
    
    /// <summary>
    /// Adds a single subscription
    /// </summary>
    /// <param name="subscription">The subscription to add</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
    public ConsumerBuilder AddSubscription(Subscription subscription)
    {
        _subscriptions.Add(subscription);
        return this;
    }

    private Action<ConsumersOptions>? _configuration;

    /// <summary>
    /// Sets additional configuration via ConsumersOptions action
    /// </summary>
    /// <param name="configuration">Configuration action for ConsumersOptions</param>
    /// <returns>The ConsumerBuilder instance for fluent chaining</returns>
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