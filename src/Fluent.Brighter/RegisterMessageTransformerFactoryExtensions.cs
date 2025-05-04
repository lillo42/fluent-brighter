
using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Paramore.Brighter;
using Paramore.Brighter.Transforms.Transformers;

namespace Fluent.Brighter;

public static class RegisterMessageTransformExtensions
{
    public static IBrighterRegister AddMessageTransform<TTransform>(this IBrighterRegister register, ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TTransform : class, IAmAMessageTransformAsync
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IAmAMessageTransformAsync), typeof(TTransform), lifetime));
        return register;
    }

    public static IBrighterRegister AddMessageTransform<TTransform>(this IBrighterRegister register, Func<ServiceProvider, TTransform> factory, ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TTransform : class, IAmAMessageTransformAsync
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IAmAMessageTransformAsync), factory, lifetime));
        return register;
    }

    public static IBrighterRegister AddMessageTransform<TTransform>(this IBrighterRegister register, TTransform transform)
            where TTransform : class, IAmAMessageTransformAsync
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IAmAMessageTransformAsync), transform));
        return register;
    }

    public static IBrighterRegister AddMessageTransform(this IBrighterRegister register, Type messageTransformHandler, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        register.Services.TryAdd(new ServiceDescriptor(typeof(IAmAMessageTransformAsync), messageTransformHandler, lifetime));
        return register;
    }
}
