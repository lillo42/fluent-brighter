using System;

using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.ServiceActivator.Extensions.DependencyInjection;

namespace Fluent.Brighter;

/// <summary>
/// Main entry point for fluently configuring Brighter components
/// </summary>
/// <remarks>
/// Provides a centralized fluent interface to configure all aspects of Brighter including:
/// - Subscriptions (message consumers)
/// - Request handlers
/// - Message mappers
/// - Message transformers
/// - Message producers
/// 
/// This builder coordinates configuration across specialized sub-builders.
/// </remarks>
public class FluentBrighterBuilder
{
    private readonly ConsumerBuilder _consumerBuilder = new();

    /// <summary>
    /// Configures message subscriptions and consumer settings
    /// </summary>
    /// <param name="configure">Configuration action for ConsumerBuilder</param>
    /// <returns>The FluentBrighterBuilder instance for fluent chaining</returns>
    public FluentBrighterBuilder Subscriptions(Action<ConsumerBuilder> configure)
    {
        configure(_consumerBuilder);
        return this;
    }
    
    private readonly RequestHandlerBuilder _requestHandlerBuilder = new();
    
    /// <summary>
    /// Configures request handlers (synchronous and asynchronous)
    /// </summary>
    /// <param name="configure">Configuration action for RequestHandlerBuilder</param>
    /// <returns>The FluentBrighterBuilder instance for fluent chaining</returns>
    public FluentBrighterBuilder RequestHandlers(Action<RequestHandlerBuilder> configure)
    {
        configure(_requestHandlerBuilder);
        return this;
    }
    
    private readonly MapperBuilder _mapperBuilder = new();

    /// <summary>
    /// Configures message mappers for request/message translation
    /// </summary>
    /// <param name="configure">Configuration action for MapperBuilder</param>
    /// <returns>The FluentBrighterBuilder instance for fluent chaining</returns>
    public FluentBrighterBuilder Mappers(Action<MapperBuilder> configure)
    {
        configure(_mapperBuilder);
        return this;
    }
    
    private TransformerBuilder _transformerBuilder = new();

    /// <summary>
    /// Configures message transformers for pipeline processing
    /// </summary>
    /// <param name="configure">Configuration action for TransformerBuilder</param>
    /// <returns>The FluentBrighterBuilder instance for fluent chaining</returns>
    public FluentBrighterBuilder Transformers(Action<TransformerBuilder> configure)
    {
        configure(_transformerBuilder);
        return this;
    }

    private ProducerBuilder _producerBuilder = new();

    /// <summary>
    /// Configures message producers and outbox settings
    /// </summary>
    /// <param name="configure">Configuration action for ProducerBuilder</param>
    /// <returns>The FluentBrighterBuilder instance for fluent chaining</returns>
    public FluentBrighterBuilder Producers(Action<ProducerBuilder> configure)
    {
        configure(_producerBuilder);
        return this;
    }
    
    internal void SetConsumerOptions(ConsumersOptions options)
        =>  _consumerBuilder.SetConsumerOptions(options);

    internal void SetBrighterBuilder(IBrighterBuilder builder)
    {
        _requestHandlerBuilder.SetRequestHandlers(builder);
        _mapperBuilder.SetMappers(builder);
        _transformerBuilder.SetTransforms(builder);
        _producerBuilder.SetProducer(builder);
    }
}