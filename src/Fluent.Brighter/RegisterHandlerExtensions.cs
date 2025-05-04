
using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class RegisterHandlerExtensions
{
    public static IBrighterRegister AddHandler<TRequest, THandler>(this IBrighterRegister register, ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TRequest : class, IRequest
        where THandler : class, IHandleRequests<TRequest>
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IHandleRequests<TRequest>), typeof(THandler), lifetime));
        return register;
    }

    public static IBrighterRegister AddHandler<TRequest, THandler>(this IBrighterRegister register, THandler handler)
            where TRequest : class, IRequest
            where THandler : class, IHandleRequests<TRequest>
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IHandleRequests<TRequest>), handler));
        return register;
    }

    public static IBrighterRegister AddHandler<TRequest, THandler>(this IBrighterRegister register, Func<IServiceProvider, THandler> factory, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TRequest : class, IRequest
            where THandler : class, IHandleRequests<TRequest>
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IHandleRequests<TRequest>), factory, lifetime));
        return register;
    }

    public static IBrighterRegister AddHandler(this IBrighterRegister register, Type requestType, Type handlerType, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IHandleRequests<>).MakeGenericType(requestType), handlerType, lifetime));
        return register;
    }

    public static IBrighterRegister AddHandlerAsync<TRequest, THandler>(this IBrighterRegister register, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TRequest : class, IRequest
            where THandler : class, IHandleRequestsAsync<TRequest>
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IHandleRequestsAsync<TRequest>), typeof(THandler), lifetime));
        return register;
    }

    public static IBrighterRegister AddHandlerAsync<TRequest, THandler>(this IBrighterRegister register, THandler handler)
            where TRequest : class, IRequest
            where THandler : class, IHandleRequestsAsync<TRequest>
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IHandleRequestsAsync<TRequest>), handler));
        return register;
    }

    public static IBrighterRegister AddHandlerAsync<TRequest, THandler>(this IBrighterRegister register, Func<IServiceProvider, THandler> factory, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TRequest : class, IRequest
            where THandler : class, IHandleRequestsAsync<TRequest>
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IHandleRequestsAsync<TRequest>), factory, lifetime));
        return register;
    }

    public static IBrighterRegister AddHandlerAsync(this IBrighterRegister register, Type requestType, Type handlerType, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IHandleRequestsAsync<>).MakeGenericType(requestType), handlerType, lifetime));
        return register;
    }
}