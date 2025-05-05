using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Fluent.Brighter;

/// <summary>
/// Interface for configuring Brighter messaging and command processing services.
/// Provides access to core configuration options, registries, and service collection.
/// </summary>
public interface IBrighterConfigurator
{
    /// <summary>
    /// Gets the service collection used for dependency injection registration.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Gets or sets which assembly components (Mappers/Handlers/Transforms) to scan for automatically.
    /// Uses bitwise flags to allow combination of multiple component types.
    /// </summary>
    AutoFromAssembly FromAssembly { get; set; }

    /// <summary>
    /// Gets the Brighter configuration options for service activation and lifetimes.
    /// </summary>
    BrighterConfiguratorOptions Options { get; }

    /// <summary>
    /// Gets or sets the registry for asynchronous request handlers.
    /// </summary>
    Action<IAmAnAsyncSubcriberRegistry> AsyncHandlerRegistry { get; set; }

    /// <summary>
    /// Gets or sets the registry for asynchronous request handlers.
    /// </summary>
    Action<IAmASubscriberRegistry> HandlerRegistry { get; set; }

    /// <summary>
    /// Gets or sets the registry for message mappers.
    /// </summary>
    Action<ServiceCollectionMessageMapperRegistry> MapperRegistry { get; set; }

    /// <summary>
    /// Adds an external message bus to the configuration using the specified producer factory.
    /// </summary>
    /// <param name="messageProducerFactory">The factory to create message producers for the external bus.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="messageProducerFactory"/> is <see langword="null"/>.</exception>
    IBrighterConfigurator AddExternalBus(IAmAMessageProducerFactory messageProducerFactory);

    /// <summary>
    /// Registers a channel factory for the specified subscriptions.
    /// </summary>
    /// <param name="channelFactory">The channel factory to register.</param>
    /// <param name="subscriptions">The subscriptions to associate with the channel factory.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="channelFactory"/> or <paramref name="subscriptions"/> is <see langword="null"/>.</exception>
    IBrighterConfigurator AddChannelFactory(IAmAChannelFactory channelFactory, IEnumerable<Subscription> subscriptions);
}