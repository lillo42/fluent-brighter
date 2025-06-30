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
            opt.DefaultChannelFactory = configurator.ChannelFactory;
            opt.InboxConfiguration = configurator.InboxConfiguration;
        });

        if (configurator.FromAssembly == AutoFromAssembly.All)
        {
            configurator.Assemblies.TryGetValue(nameof(AutoFromAssembly.All), out var assemblies);
            assemblies ??= [];
            brighter = brighter.AutoFromAssemblies(assemblies.ToArray());
        }
        else
        {
            
            configurator.Assemblies.TryGetValue(nameof(AutoFromAssembly.Handlers), out var assemblies);
            assemblies ??= [];
            brighter = configurator.FromAssembly.HasFlag(AutoFromAssembly.Handlers)
                ? brighter.HandlersFromAssemblies(assemblies.ToArray()).AsyncHandlersFromAssemblies(assemblies.ToArray())
                : brighter.Handlers(configurator.HandlerRegistry).AsyncHandlers(configurator.AsyncHandlerRegistry);

            configurator.Assemblies.TryGetValue(nameof(AutoFromAssembly.Mappers), out assemblies);
            assemblies ??= [];
            brighter = configurator.FromAssembly.HasFlag(AutoFromAssembly.Mappers)
                ? brighter.MapperRegistryFromAssemblies(assemblies.ToArray())
                : brighter.MapperRegistry(configurator.MapperRegistry);

            if (configurator.FromAssembly.HasFlag(AutoFromAssembly.Transforms))
            {
                configurator.Assemblies.TryGetValue(nameof(AutoFromAssembly.Transforms), out assemblies);
                assemblies ??= [];
                brighter = brighter.TransformsFromAssemblies(assemblies.ToArray());
            }
        }

        var producerRegistry = configurator.ProducerRegistry;
        if (producerRegistry != null)
        {
            brighter = brighter.UseExternalBus(cfg =>
            {
                cfg.Outbox = configurator.OutboxConfiguration?.Outbox;
                cfg.MaxOutStandingMessages = configurator.OutboxConfiguration?.MaxOutStandingMessages;
                cfg.MaxOutStandingCheckInterval = configurator.OutboxConfiguration?.MaxOutStandingCheckInterval ?? TimeSpan.Zero;
                cfg.OutboxBulkChunkSize = configurator.OutboxConfiguration?.OutboxBulkChunkSize;
                cfg.OutBoxBag = configurator.OutboxConfiguration?.OutBoxBag;
                cfg.OutboxTimeout = configurator.OutboxConfiguration?.OutboxTimeout;
                cfg.ProducerRegistry = producerRegistry;
                cfg.DistributedLock = configurator.DistributedLockConfiguration;
            });
        }

        return services;
    }
}