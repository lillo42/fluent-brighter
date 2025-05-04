
using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class RegisterMapperExtensions
{
    public static IBrighterRegister AddMapper<TRequest, TMapper>(this IBrighterRegister register, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TRequest : class, IRequest
        where TMapper : class, IAmAMessageMapper<TRequest>
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IAmAMessageMapper<TRequest>), typeof(TMapper), lifetime));
        return register;
    }

    public static IBrighterRegister AddMapper<TRequest, TMapper>(this IBrighterRegister register, Func<IServiceProvider, TMapper> factory, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TRequest : class, IRequest
            where TMapper : class, IAmAMessageMapper<TRequest>
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IAmAMessageMapper<TRequest>), factory, lifetime));
        return register;
    }

    public static IBrighterRegister AddMapper<TRequest, TMapper>(this IBrighterRegister register, TMapper mapper)
                where TRequest : class, IRequest
                where TMapper : class, IAmAMessageMapper<TRequest>
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IAmAMessageMapper<TRequest>), mapper));
        return register;
    }

    public static IBrighterRegister AddMapper(this IBrighterRegister register, Type requestType, Type mapperType, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IAmAMessageMapper<>).MakeGenericType(requestType), mapperType, lifetime));
        return register;
    }
}