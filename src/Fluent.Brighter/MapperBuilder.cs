using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Fluent.Brighter;

/// <summary>
/// Builder for configuring message mappers used to convert between Brighter requests and transport messages
/// </summary>
/// <remarks>
/// Provides a fluent interface to register message mappers and configure mapper discovery.
/// Automatically scans non-Microsoft/non-System assemblies for mappers by default.
/// </remarks>
public sealed class MapperBuilder
{
    private List<Assembly> _assemblies = [];

    /// <summary>
    /// Initializes a new MapperBuilder instance
    /// </summary>
    /// <remarks>
    /// Automatically includes all non-dynamic assemblies in the current AppDomain that
    /// are not Microsoft or System assemblies for mapper discovery.
    /// </remarks>
    public MapperBuilder()
    {
        _assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(a =>
            !a.IsDynamic 
            && a.FullName?.StartsWith("Microsoft.", true, CultureInfo.InvariantCulture) == false 
            && a.FullName?.StartsWith("System.", true, CultureInfo.InvariantCulture) == false
            && a.FullName?.StartsWith("Paramore.Brighter", true, CultureInfo.InvariantCulture) == false));
    }
    
    /// <summary>
    /// Specifies specific assemblies to scan for message mappers
    /// </summary>
    /// <param name="assemblies">The assemblies to include in mapper scanning</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public MapperBuilder FromAssemblies(params Assembly[] assemblies)
    {
        _assemblies = assemblies.ToList();
        return this;
    }

    private Action<ServiceCollectionMessageMapperRegistryBuilder> _registry = _ => { };

    /// <summary>
    /// Registers a mapper that handles both synchronous and asynchronous mapping for a request type
    /// </summary>
    /// <typeparam name="TRequest">The request type (IRequest)</typeparam>
    /// <typeparam name="TMessageMapper">The mapper implementation</typeparam>
    /// <returns>The builder instance for fluent chaining</returns>
    /// <remarks>
    /// The mapper must implement both IAmAMessageMapper&lt;TRequest&gt; and IAmAMessageMapperAsync&lt;TRequest&gt;
    /// </remarks>
    public MapperBuilder AddMapper<TRequest, TMessageMapper>()
        where TRequest : class, IRequest
        where TMessageMapper : class, IAmAMessageMapper<TRequest>, IAmAMessageMapperAsync<TRequest>
    {
        _registry += registry =>
        {
            registry.Add(typeof(TRequest), typeof(TMessageMapper));
            registry.AddAsync(typeof(TRequest), typeof(TMessageMapper));
        };

        return this;
    }
    
    /// <summary>
    /// Registers a mapper that handles synchronous mapping for a request type
    /// </summary>
    /// <typeparam name="TRequest">The request type (IRequest)</typeparam>
    /// <typeparam name="TMessageMapper">The mapper implementation</typeparam>
    /// <returns>The builder instance for fluent chaining</returns>
    public MapperBuilder AddMapperSync<TRequest, TMessageMapper>()
        where TRequest : class, IRequest
        where TMessageMapper : class, IAmAMessageMapper<TRequest>
    {
        _registry += registry =>
        {
            registry.Add(typeof(TRequest), typeof(TMessageMapper));
        };

        return this;
    }
    
    /// <summary>
    /// Registers a mapper that handles asynchronous mapping for a request type
    /// </summary>
    /// <typeparam name="TRequest">The request type (IRequest)</typeparam>
    /// <typeparam name="TMessageMapper">The mapper implementation</typeparam>
    /// <returns>The builder instance for fluent chaining</returns>
    public MapperBuilder AddMapperAsync<TRequest, TMessageMapper>()
        where TRequest : class, IRequest
        where TMessageMapper : class, IAmAMessageMapperAsync<TRequest>
    {
        _registry += registry =>
        {
            registry.AddAsync(typeof(TRequest), typeof(TMessageMapper));
        };

        return this;
    }
    
    private Type? _defaultMapperSync;
    private Type? _defaultMapperAsync;

    /// <summary>
    /// Sets the default mapper type for both synchronous and asynchronous operations
    /// </summary>
    /// <param name="defaultMapper">The mapper type to use as default</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public MapperBuilder SetDefaultMapper(Type defaultMapper)
    {
        _defaultMapperSync = defaultMapper;
        _defaultMapperAsync = defaultMapper;
        return this;
    }
    
    /// <summary>
    /// Sets the default mapper type for synchronous operations
    /// </summary>
    /// <param name="defaultMapper">The mapper type to use as default for sync</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public MapperBuilder SetDefaultMapperSync(Type defaultMapper)
    {
        _defaultMapperSync = defaultMapper;
        return this;
    }
    
    /// <summary>
    /// Sets the default mapper type for asynchronous operations
    /// </summary>
    /// <param name="defaultMapper">The mapper type to use as default for async</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public MapperBuilder SetDefaultMapperAsync(Type defaultMapper)
    {
        _defaultMapperAsync = defaultMapper;
        return this;
    }
    
    private Action<ServiceCollectionMessageMapperRegistryBuilder>? _configure;

    /// <summary>
    /// Sets a configuration action for the mapper registry
    /// </summary>
    /// <param name="configure">Configuration action for the mapper registry builder</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public MapperBuilder SetConfigure(Action<ServiceCollectionMessageMapperRegistryBuilder> configure)
    {
        _configure = configure;
        return this;
    }

    internal void SetMappers(IBrighterBuilder brighter)
    {
        brighter.MapperRegistryFromAssemblies(_assemblies);
        brighter.MapperRegistry(registry =>
        {
            _registry(registry);
            _configure?.Invoke(registry);
        }, _defaultMapperSync, _defaultMapperSync);
    }
}