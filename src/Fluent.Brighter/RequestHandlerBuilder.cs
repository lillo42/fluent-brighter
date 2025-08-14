using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Fluent.Brighter;

/// <summary>
/// Builder for configuring request handlers in Brighter's pipeline
/// </summary>
/// <remarks>
/// Provides a fluent interface to register synchronous and asynchronous request handlers.
/// Automatically scans non-Microsoft/non-System assemblies for handlers by default.
/// </remarks>
public sealed class RequestHandlerBuilder
{
    private List<Assembly> _assemblies = [];

    /// <summary>
    /// Initializes a new RequestHandlerBuilder instance
    /// </summary>
    /// <remarks>
    /// Automatically includes all non-dynamic assemblies in the current AppDomain that
    /// are not Microsoft or System assemblies for handler discovery.
    /// </remarks>
    public RequestHandlerBuilder()
    {
        _assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(a =>
            !a.IsDynamic && a.FullName.StartsWith("Microsoft.", true, CultureInfo.InvariantCulture) != true &&
            a.FullName?.StartsWith("System.", true, CultureInfo.InvariantCulture) != true));
    }

    /// <summary>
    /// Specifies specific assemblies to scan for request handlers
    /// </summary>
    /// <param name="assemblies">The assemblies to include in handler scanning</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public RequestHandlerBuilder FromAssemblies(params Assembly[] assemblies)
    {
        _assemblies = assemblies.ToList();
        return this;
    }

    private Action<IAmAnAsyncSubcriberRegistry> _asyncRegistration = _ => { };
    private Action<IAmASubscriberRegistry> _syncRegistration = _ => { };

    /// <summary>
    /// Registers a synchronous handler for a specific request type
    /// </summary>
    /// <typeparam name="TRequest">The request type (IRequest)</typeparam>
    /// <typeparam name="TRequestHandler">The handler implementation</typeparam>
    /// <returns>The builder instance for fluent chaining</returns>
    public RequestHandlerBuilder AddHandler<TRequest, TRequestHandler>()
        where TRequest : class, IRequest
        where TRequestHandler : class, IHandleRequests<TRequest>
    {
        _syncRegistration += registry =>
        {
            registry.Register<TRequest, TRequestHandler>();
        };

        return this;
    }
    
    /// <summary>
    /// Registers multiple synchronous handlers for a request type using custom routing
    /// </summary>
    /// <typeparam name="TRequest">The request type (IRequest)</typeparam>
    /// <param name="router">
    /// Function that determines which handlers should process a request based on the request and context
    /// </param>
    /// <param name="handlerTypes">Collection of handler types to register</param>
    public RequestHandlerBuilder AddHandler<TRequest>(
        Func<IRequest?, IRequestContext?, List<Type>> router, 
        IEnumerable<Type> handlerTypes)
        where TRequest : class, IRequest
    {
        _syncRegistration += registry =>
        {
            registry.Register<TRequest>(router,  handlerTypes);
        };

        return this;
    }
    
    /// <summary>
    /// Registers an asynchronous handler for a specific request type
    /// </summary>
    /// <typeparam name="TRequest">The request type (IRequest)</typeparam>
    /// <typeparam name="TRequestHandler">The async handler implementation</typeparam>
    /// <returns>The builder instance for fluent chaining</returns>
    public RequestHandlerBuilder AddHandlerAsync<TRequest, TRequestHandler>()
        where TRequest : class, IRequest
        where TRequestHandler : class, IHandleRequestsAsync<TRequest>
    {
        _asyncRegistration += registry =>
        {
            registry.RegisterAsync<TRequest, TRequestHandler>();
        };

        return this;
    }
    
    /// <summary>
    /// Registers multiple asynchronous handlers for a request type using custom routing
    /// </summary>
    /// <typeparam name="TRequest">The request type (IRequest)</typeparam>
    /// <param name="router">
    /// Function that determines which handlers should process a request based on the request and context
    /// </param>
    /// <param name="handlerTypes">Collection of async handler types to register</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public RequestHandlerBuilder AddHandlerAsync<TRequest>(
        Func<IRequest?, IRequestContext?, List<Type>> router, 
        IEnumerable<Type> handlerTypes)
        where TRequest : class, IRequest
    {
        _asyncRegistration += registry =>
        {
            registry.RegisterAsync<TRequest>(router,  handlerTypes);
        };

        return this;
    }

    internal void SetRequestHandlers(IBrighterBuilder builder)
    {
        builder.AsyncHandlers(_asyncRegistration);
        builder.Handlers(_syncRegistration);
        builder.HandlersFromAssemblies(_assemblies);
    }
}