using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Fluent.Brighter;

public sealed class MapperBuilder
{
    private List<Assembly> _assemblies = [];

    public MapperBuilder()
    {
        _assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(a =>
            !a.IsDynamic && a.FullName?.StartsWith("Microsoft.", true, CultureInfo.InvariantCulture) != true &&
            a.FullName?.StartsWith("System.", true, CultureInfo.InvariantCulture) != true));
    }
    
    public MapperBuilder FromAssemblies(params Assembly[] assemblies)
    {
        _assemblies = assemblies.ToList();
        return this;
    }

    private Action<ServiceCollectionMessageMapperRegistryBuilder> _registry = _ => { };

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

    public MapperBuilder SetDefaultMapper(Type defaultMapper)
    {
        _defaultMapperSync = defaultMapper;
        _defaultMapperAsync = defaultMapper;
        return this;
    }
    
    public MapperBuilder SetDefaultMapperSync(Type defaultMapper)
    {
        _defaultMapperSync = defaultMapper;
        return this;
    }
    
    public MapperBuilder SetDefaultMapperAsync(Type defaultMapper)
    {
        _defaultMapperAsync = defaultMapper;
        return this;
    }
    
    private Action<ServiceCollectionMessageMapperRegistryBuilder>? _configure;

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