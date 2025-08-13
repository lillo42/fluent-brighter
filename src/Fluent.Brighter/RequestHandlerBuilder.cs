using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Fluent.Brighter;

public sealed class RequestHandlerBuilder
{
    private List<Assembly> _assemblies = [];

    public RequestHandlerBuilder()
    {
        _assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(a =>
            !a.IsDynamic && a.FullName?.StartsWith("Microsoft.", true, CultureInfo.InvariantCulture) != true &&
            a.FullName?.StartsWith("System.", true, CultureInfo.InvariantCulture) != true));
    }

    public RequestHandlerBuilder FromAssemblies(params Assembly[] assemblies)
    {
        _assemblies = assemblies.ToList();
        return this;
    }

    private Action<IAmAnAsyncSubcriberRegistry> _asyncRegistration = _ => { };
    private Action<IAmASubscriberRegistry> _syncRegistration = _ => { };

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