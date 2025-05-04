
using System;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class BrighterConfiguratorMapperExtensions
{
    public static IBrighterConfigurator AddMapper<TRequest, TMapper>(this IBrighterConfigurator configurator)
        where TRequest : class, IRequest
        where TMapper : class, IAmAMessageMapper<TRequest>
    {
        configurator.MapperRegistry += static registry => registry.Register<TRequest, TMapper>();
        return configurator;
    }

    public static IBrighterConfigurator AddMapper(this IBrighterConfigurator configurator, Type message, Type mapper)
    {
        configurator.MapperRegistry += registry => registry.Add(message, mapper);
        return configurator;
    }
}