using System;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.ServiceActivator.Extensions.DependencyInjection;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring Brighter services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Brighter messaging and command processing services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configure">An action to configure the Brighter services.</param>
    /// <returns>The original service collection for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is <see langword="null"/>.</exception>
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