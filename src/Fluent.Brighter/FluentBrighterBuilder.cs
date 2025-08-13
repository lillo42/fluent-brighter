using System;

using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.ServiceActivator.Extensions.DependencyInjection;

namespace Fluent.Brighter;

public class FluentBrighterBuilder
{
    private readonly ConsumerBuilder _consumerBuilder = new();

    public FluentBrighterBuilder Subscriptions(Action<ConsumerBuilder> configure)
    {
        configure(_consumerBuilder);
        return this;
    }
    
    private readonly RequestHandlerBuilder _requestHandlerBuilder = new();
    public FluentBrighterBuilder RequestHandlers(Action<RequestHandlerBuilder> configure)
    {
        configure(_requestHandlerBuilder);
        return this;
    }
    
    private readonly MapperBuilder _mapperBuilder = new();

    public FluentBrighterBuilder Mappers(Action<MapperBuilder> configure)
    {
        configure(_mapperBuilder);
        return this;
    }
    
    private TransformerBuilder _transformerBuilder = new();

    public FluentBrighterBuilder Transformers(Action<TransformerBuilder> configure)
    {
        configure(_transformerBuilder);
        return this;
    }

    private ProducerBuilder _producerBuilder = new();

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