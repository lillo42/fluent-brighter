using System;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring service activation options through <see cref="IBrighterConfigurator"/>.
/// These methods enable fluent configuration of dependency injection lifetimes, scoping, and contextual services.
/// </summary>
public static class BrighterConfiguratorOptionsExtensions
{
    /// <summary>
    /// Enables scoped service registration mode for all Brighter components.
    /// </summary>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="configurator"/> is <see langword="null"/>.</exception>
    public static IBrighterConfigurator EnableScoped(this IBrighterConfigurator configurator)
    {
        return UseScoped(configurator, true);
    }

    /// <summary>
    /// Disables scoped service registration mode for all Brighter components.
    /// </summary>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="configurator"/> is <see langword="null"/>.</exception>
    public static IBrighterConfigurator DisableScoped(this IBrighterConfigurator configurator)
    {
        return UseScoped(configurator, false);
    }

    /// <summary>
    /// Sets whether to use scoped service registration for all Brighter components.
    /// </summary>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <param name="useScoped">True to enable scoped registration, false for transient.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="configurator"/> is <see langword="null"/>.</exception>
    public static IBrighterConfigurator UseScoped(this IBrighterConfigurator configurator, bool useScoped)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Options.UseScoped = useScoped;
        return configurator;
    }

    /// <summary>
    /// Sets the service lifetime for CommandProcessor instances.
    /// </summary>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <param name="lifetime">The service lifetime to apply (<see cref="ServiceLifetime.Singleton"/>, <see cref="ServiceLifetime.Scoped"/>, or <see cref="ServiceLifetime.Transient"/>).</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="configurator"/> is <see langword="null"/>.</exception>
    public static IBrighterConfigurator CommandProcessorLifetime(this IBrighterConfigurator configurator, ServiceLifetime lifetime)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Options.CommandProcessorLifetime = lifetime;
        return configurator;
    }

    /// <summary>
    /// Sets the service lifetime for request handlers.
    /// </summary>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <param name="lifetime">The service lifetime to apply.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="configurator"/> is <see langword="null"/>.</exception>
    public static IBrighterConfigurator HandlerLifetime(this IBrighterConfigurator configurator, ServiceLifetime lifetime)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Options.HandlerLifetime = lifetime;
        return configurator;
    }


    /// <summary>
    /// Sets the service lifetime for message mappers.
    /// </summary>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <param name="lifetime">The service lifetime to apply.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="configurator"/> is <see langword="null"/>.</exception>
    public static IBrighterConfigurator MapperLifetime(this IBrighterConfigurator configurator, ServiceLifetime lifetime)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Options.MapperLifetime = lifetime;
        return configurator;
    }

    /// <summary>
    /// Sets the service lifetime for message transformers.
    /// </summary>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <param name="lifetime">The service lifetime to apply.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="configurator"/> is <see langword="null"/>.</exception>
    public static IBrighterConfigurator TransformerLifetime(this IBrighterConfigurator configurator, ServiceLifetime lifetime)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Options.TransformerLifetime = lifetime;
        return configurator;
    }

    /// <summary>
    /// Sets the factory for creating request context instances during message processing.
    /// </summary>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <param name="factory">The request context factory implementation.</param>
    /// <returns>The configurator instance for fluent chaining.</returns> 
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="configurator"/> is <see langword="null"/>.</exception>
    public static IBrighterConfigurator RequestContextFactory(this IBrighterConfigurator configurator, IAmARequestContextFactory factory)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Options.RequestContextFactory = factory;
        return configurator;
    }
}