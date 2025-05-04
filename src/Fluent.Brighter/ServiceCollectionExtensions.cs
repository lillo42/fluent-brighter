using System;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.ServiceActivator.Extensions.DependencyInjection;

namespace Fluent.Brighter;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBrighter(this IServiceCollection services, Action<IBrighterConfigurator> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var configurator = new BrighterConfigurator(services, new BrighterConfiguratorOptions());
        configure(configurator);

        var brighter = services.AddServiceActivator(opt =>
        {
            opt.UseScoped = configurator.Options.UseScoped;
            opt.CommandProcessorLifetime = configurator.Options.CommandProcessorLifetime;
            opt.HandlerLifetime = configurator.Options.HandlerLifetime;
            opt.MapperLifetime = configurator.Options.MapperLifetime;
            opt.TransformerLifetime = configurator.Options.TransformerLifetime;
            opt.PolicyRegistry = configurator.Options.PolicyRegistry;
            opt.RequestContextFactory = configurator.Options.RequestContextFactory;
            opt.Subscriptions = configurator.Subscriptions;
            opt.ChannelFactory = configurator.ChannelFactory;
        });

        if (configurator.FromAssembly.HasFlag(AutoFromAssembly.All))
        {
            brighter = brighter.AutoFromAssemblies();
        }
        else
        {
            brighter = configurator.FromAssembly.HasFlag(AutoFromAssembly.Handlers)
                ? brighter.HandlersFromAssemblies()
                       .AsyncHandlersFromAssemblies()
                : brighter.Handlers(configurator.HandlerRegistry);

            brighter = configurator.FromAssembly.HasFlag(AutoFromAssembly.Mappers)
                ? brighter.MapperRegistryFromAssemblies()
                : brighter.Handlers(configurator.HandlerRegistry);

            if (configurator.FromAssembly.HasFlag(AutoFromAssembly.Transforms))
            {
                brighter = brighter.TransformsFromAssemblies();
            }
        }

        var producerRegistry = configurator.ProducerRegistry;
        if (producerRegistry != null)
        {
            brighter = brighter.UseExternalBus(producerRegistry);
        }

        return services;
    }
}