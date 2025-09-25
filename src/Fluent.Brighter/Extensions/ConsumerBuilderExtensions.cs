using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter.Observability;

namespace Fluent.Brighter;

/// <summary>
/// Provides fluent extension methods for configuring service lifetimes and instrumentation in Brighter consumers
/// </summary>
public static class ConsumerBuilderExtensions
{
    #region Handlers
    
    /// <summary>
    /// Configures message handlers as singleton services
    /// </summary>
    /// <param name="consumerBuilder">The ConsumerBuilder instance</param>
    /// <returns>The ConsumerBuilder for fluent chaining</returns>
    public static ConsumerBuilder UseHandlersAsSingleton(this ConsumerBuilder consumerBuilder) 
        => consumerBuilder.SetHandlerLifetime(ServiceLifetime.Singleton);

    /// <summary>
    /// Configures message handlers as scoped services
    /// </summary>
    /// <param name="consumerBuilder">The ConsumerBuilder instance</param>
    /// <returns>The ConsumerBuilder for fluent chaining</returns>
    public static ConsumerBuilder UseHandlersAsScoped(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetHandlerLifetime(ServiceLifetime.Scoped);
    
    /// <summary>
    /// Configures message handlers as transient services
    /// </summary>
    /// <param name="consumerBuilder">The ConsumerBuilder instance</param>
    /// <returns>The ConsumerBuilder for fluent chaining</returns>
    public static ConsumerBuilder UseHandlersAsTransient(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetHandlerLifetime(ServiceLifetime.Transient);
    #endregion
    
    #region Message Mapper 
    
    /// <summary>
    /// Configures message mappers as singleton services
    /// </summary>
    /// <param name="consumerBuilder">The ConsumerBuilder instance</param>
    /// <returns>The ConsumerBuilder for fluent chaining</returns>
    public static ConsumerBuilder UseMappersAsSingleton(this ConsumerBuilder consumerBuilder) 
        => consumerBuilder.SetMapperLifetime(ServiceLifetime.Singleton);

    /// <summary>
    /// Configures message mappers as scoped services
    /// </summary>
    /// <param name="consumerBuilder">The ConsumerBuilder instance</param>
    /// <returns>The ConsumerBuilder for fluent chaining</returns>
    public static ConsumerBuilder UseMappersAsScoped(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetMapperLifetime(ServiceLifetime.Scoped);

    /// <summary>
    /// Configures message mappers as transient services
    /// </summary>
    /// <param name="consumerBuilder">The ConsumerBuilder instance</param>
    /// <returns>The ConsumerBuilder for fluent chaining</returns>
    public static ConsumerBuilder UseMappersAsTransient(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetMapperLifetime(ServiceLifetime.Transient);
    #endregion
    
    #region Transformer 
    
    /// <summary>
    /// Configures message transformers as singleton services
    /// </summary>
    /// <param name="consumerBuilder">The ConsumerBuilder instance</param>
    /// <returns>The ConsumerBuilder for fluent chaining</returns>
    public static ConsumerBuilder UseTransformersAsSingleton(this ConsumerBuilder consumerBuilder) 
        => consumerBuilder.SetTransformerLifetime(ServiceLifetime.Singleton);

    /// <summary>
    /// Configures message transformers as scoped services
    /// </summary>
    /// <param name="consumerBuilder">The ConsumerBuilder instance</param>
    /// <returns>The ConsumerBuilder for fluent chaining</returns>
    public static ConsumerBuilder UseTransformersAsScoped(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetTransformerLifetime(ServiceLifetime.Scoped);

    /// <summary>
    /// Configures message transformers as transient services
    /// </summary>
    /// <param name="consumerBuilder">The ConsumerBuilder instance</param>
    /// <returns>The ConsumerBuilder for fluent chaining</returns>
    public static ConsumerBuilder UseTransformersAsTransient(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetTransformerLifetime(ServiceLifetime.Transient);
    #endregion

    #region Instrumentation

    /// <summary>
    /// Disables all instrumentation and monitoring features
    /// </summary>
    /// <param name="consumerBuilder">The ConsumerBuilder instance</param>
    /// <returns>The ConsumerBuilder for fluent chaining</returns>
    public static ConsumerBuilder UseNoneInstrumentation(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetInstrumentation(InstrumentationOptions.None);

    /// <summary>
    /// Enables all available instrumentation and monitoring features
    /// </summary>
    /// <param name="consumerBuilder">The ConsumerBuilder instance</param>
    /// <returns>The ConsumerBuilder for fluent chaining</returns>
    public static ConsumerBuilder UseAllInstrumentation(this ConsumerBuilder consumerBuilder)
        => consumerBuilder.SetInstrumentation(InstrumentationOptions.All);
    #endregion
}