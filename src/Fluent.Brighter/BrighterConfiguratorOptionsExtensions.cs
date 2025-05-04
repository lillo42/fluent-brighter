
using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class BrighterConfiguratorOptionsExtensions
{
    public static IBrighterConfigurator EnableScoped(this IBrighterConfigurator configurator)
    {
        return SetUseScoped(configurator, true);
    }

    public static IBrighterConfigurator DisableScoped(this IBrighterConfigurator configurator)
    {
        return SetUseScoped(configurator, false);
    }

    public static IBrighterConfigurator SetUseScoped(this IBrighterConfigurator configurator, bool useScoped)
    {
        configurator.Options.UseScoped = useScoped;
        return configurator;
    }

    public static IBrighterConfigurator CommandProcessorLifetime(this IBrighterConfigurator configurator, ServiceLifetime lifetime)
    {
        configurator.Options.CommandProcessorLifetime = lifetime;
        return configurator;
    }

    public static IBrighterConfigurator HandlerLifetime(this IBrighterConfigurator configurator, ServiceLifetime lifetime)
    {
        configurator.Options.HandlerLifetime = lifetime;
        return configurator;
    }

    public static IBrighterConfigurator MapperLifetime(this IBrighterConfigurator configurator, ServiceLifetime lifetime)
    {
        configurator.Options.MapperLifetime = lifetime;
        return configurator;
    }

    public static IBrighterConfigurator TransformerLifetime(this IBrighterConfigurator configurator, ServiceLifetime lifetime)
    {
        configurator.Options.TransformerLifetime = lifetime;
        return configurator;
    }

    public static IBrighterConfigurator RequestContextFactory(this IBrighterConfigurator configurator, IAmARequestContextFactory factory)
    {
        configurator.Options.RequestContextFactory = factory;
        return configurator;
    }
}