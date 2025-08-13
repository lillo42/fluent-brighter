using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter.Observability;

namespace Fluent.Brighter;

public static class ConsumerBuilderExtensions
{
    #region CommandProcessor
    public static ConsumerBuilder UseCommandProcessorAsSingleton(this ConsumerBuilder consumerBuilder) 
        => consumerBuilder.SetCommandProcessorLifetime(ServiceLifetime.Singleton);

    public static ConsumerBuilder UseCommandProcessorAsScoped(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetCommandProcessorLifetime(ServiceLifetime.Scoped);
    
    public static ConsumerBuilder UseCommandProcessorAsTransient(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetCommandProcessorLifetime(ServiceLifetime.Transient);
    #endregion
    
    #region Handlers
    public static ConsumerBuilder UseHandlersAsSingleton(this ConsumerBuilder consumerBuilder) 
        => consumerBuilder.SetHandlerLifetime(ServiceLifetime.Singleton);

    public static ConsumerBuilder UseHandlersAsScoped(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetHandlerLifetime(ServiceLifetime.Scoped);
    
    public static ConsumerBuilder UseHandlersAsTransient(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetHandlerLifetime(ServiceLifetime.Transient);
    #endregion
    
    #region Message Mapper 
    public static ConsumerBuilder UseMappersAsSingleton(this ConsumerBuilder consumerBuilder) 
        => consumerBuilder.SetMapperLifetime(ServiceLifetime.Singleton);

    public static ConsumerBuilder UseMappersAsScoped(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetMapperLifetime(ServiceLifetime.Scoped);

    public static ConsumerBuilder UseMappersAsTransient(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetMapperLifetime(ServiceLifetime.Transient);
    #endregion
    
    #region Transformer 
    public static ConsumerBuilder UseTransformersAsSingleton(this ConsumerBuilder consumerBuilder) 
        => consumerBuilder.SetTransformerLifetime(ServiceLifetime.Singleton);

    public static ConsumerBuilder UseTransformersAsScoped(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetTransformerLifetime(ServiceLifetime.Scoped);

    public static ConsumerBuilder UseTransformersAsTransient(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetTransformerLifetime(ServiceLifetime.Transient);
    #endregion

    #region Instrumentation

    public static ConsumerBuilder UseNoneInstrumentation(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetInstrumentation(InstrumentationOptions.None);

    public static ConsumerBuilder UseAllInstrumentation(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetInstrumentation(InstrumentationOptions.All);
    #endregion
}