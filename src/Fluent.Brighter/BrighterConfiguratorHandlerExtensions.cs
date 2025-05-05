using System;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="IBrighterConfigurator"/> to simplify handler registration.
/// These methods enable fluent configuration of synchronous and asynchronous request handlers.
/// </summary>
public static class BrighterConfiguratorHandlerExtensions
{
    /// <summary>
    /// Registers a synchronous request handler for the specified request type.
    /// </summary>
    /// <typeparam name="TRequest">The type of request the handler will process.</typeparam>
    /// <typeparam name="THandler">The handler implementation type implementing <see cref="IHandleRequests{TRequest}"/>.</typeparam>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the configurator's handler <paramref name="configurator"/> is <see langword="null"/>.</exception>
    public static IBrighterConfigurator AddHandler<TRequest, THandler>(this IBrighterConfigurator configurator)
        where TRequest : class, IRequest
        where THandler : class, IHandleRequests<TRequest>
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.HandlerRegistry += static registry => registry.Register<TRequest, THandler>();
        return configurator;
    }

    /// <summary>
    /// Registers an asynchronous request handler for the specified request type.
    /// </summary>
    /// <typeparam name="TRequest">The type of request the handler will process.</typeparam>
    /// <typeparam name="THandler">The handler implementation type implementing <see cref="IHandleRequestsAsync{TRequest}"/>.</typeparam>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the configurator's handler <paramref name="configurator"/> is <see langword="null"/>.</exception>
    public static IBrighterConfigurator AddHandlerAsync<TRequest, THandler>(this IBrighterConfigurator configurator)
            where TRequest : class, IRequest
            where THandler : class, IHandleRequestsAsync<TRequest>
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.AsyncHandlerRegistry += static registry => registry.RegisterAsync<TRequest, THandler>();
        return configurator;
    }
}