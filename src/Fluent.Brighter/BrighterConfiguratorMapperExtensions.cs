using System;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="IBrighterConfigurator"/> to register message mappers.
/// Supports both strongly-typed and runtime-typed mapper registration scenarios.
/// </summary>
public static class BrighterConfiguratorMapperExtensions
{
    /// <summary>
    /// Registers a strongly-typed message mapper for the specified request type.
    /// </summary>
    /// <typeparam name="TRequest">The request type to map, must implement <see cref="IRequest"/>.</typeparam>
    /// <typeparam name="TMapper">The mapper implementation type implementing <see cref="IAmAMessageMapper{TRequest}"/>.</typeparam>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <returns>The configurator instance for fluent configuration chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="configurator"/> is <see langword="null"/>.</exception>
    public static IBrighterConfigurator AddMapper<TRequest, TMapper>(this IBrighterConfigurator configurator)
        where TRequest : class, IRequest
        where TMapper : class, IAmAMessageMapper<TRequest>
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.MapperRegistry += static registry => registry.Register<TRequest, TMapper>();
        return configurator;
    }

    /// <summary>
    /// Registers a message mapper using runtime-determined types.
    /// </summary>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <param name="message">The request type (must implement <see cref="IRequest"/>).</param>
    /// <param name="mapper">The mapper type (must implement <see cref="IAmAMessageMapper{T}"/> for the corresponding message type).</param>
    /// <returns>The configurator instance for fluent configuration chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="configurator"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown if types don't meet mapping requirements.</exception>
    public static IBrighterConfigurator AddMapper(this IBrighterConfigurator configurator, Type message, Type mapper)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.MapperRegistry += registry => registry.Add(message, mapper);
        return configurator;
    }
}