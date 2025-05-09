using System.Collections.Generic;
using System.Reflection;

namespace Fluent.Brighter;

public static class BrighterConfiguratorFromAssemblyExtensions
{
    public static IBrighterConfigurator NoneFromAssembly(this IBrighterConfigurator configurator)
    {
        configurator.FromAssembly = AutoFromAssembly.None;
        configurator.Assemblies = new Dictionary<string, List<Assembly>>();
        return configurator;
    }
    
    public static IBrighterConfigurator AddMapperFromAssembly<T>(this IBrighterConfigurator configurator)
    {
        return AddMapperFromAssembly(configurator, typeof(T).Assembly);
    }
    
    public static IBrighterConfigurator AddMapperFromAssembly(this IBrighterConfigurator configurator, params Assembly[] assemblies)
    {
        configurator.FromAssembly |= AutoFromAssembly.Mappers;
        
        if (!configurator.Assemblies.TryGetValue(nameof(AutoFromAssembly.Mappers), out var mappers))
        {
            configurator.Assemblies[nameof(AutoFromAssembly.Mappers)] = mappers = [];
        }
        
        mappers.AddRange(assemblies);
        return configurator;
    }
    
    public static IBrighterConfigurator AddHandlersFromAssembly<T>(IBrighterConfigurator configurator)
    {
        return AddHandlersFromAssembly(configurator, typeof(T).Assembly);
    }
    
    public static IBrighterConfigurator AddHandlersFromAssembly(this IBrighterConfigurator configurator, params Assembly[] assemblies)
    {
        configurator.FromAssembly |= AutoFromAssembly.Handlers;
        
        if (!configurator.Assemblies.TryGetValue(nameof(AutoFromAssembly.Handlers), out var handlers))
        {
            configurator.Assemblies[nameof(AutoFromAssembly.Handlers)] = handlers = [];
        }
        
        handlers.AddRange(assemblies);
        return configurator;
    }
    
    public static IBrighterConfigurator AddTransformsFromAssembly<T>(this IBrighterConfigurator configurator)
    {
        return AddTransformsFromAssembly(configurator, typeof(T).Assembly);
    }
    
    public static IBrighterConfigurator AddTransformsFromAssembly(this IBrighterConfigurator configurator, params Assembly[] assemblies)
    {
        configurator.FromAssembly |= AutoFromAssembly.Transforms;
        
        if (!configurator.Assemblies.TryGetValue(nameof(AutoFromAssembly.Transforms), out var transforms))
        {
            configurator.Assemblies[nameof(AutoFromAssembly.Transforms)] = transforms = [];
        }
        
        transforms.AddRange(assemblies);
        return configurator;
    }
    
    public static IBrighterConfigurator AddAllFromAssembly<T>(this IBrighterConfigurator configurator)
    {
        return AddAllFromAssembly(configurator, typeof(T).Assembly);
    }
    
    public static IBrighterConfigurator AddAllFromAssembly(this IBrighterConfigurator configurator, params Assembly[] assemblies)
    {
        configurator.FromAssembly |= AutoFromAssembly.All;
        
        if (!configurator.Assemblies.TryGetValue(nameof(AutoFromAssembly.All), out var transforms))
        {
            configurator.Assemblies[nameof(AutoFromAssembly.All)] = transforms = [];
        }
        
        transforms.AddRange(assemblies);
        return configurator;
    }

    public static IBrighterConfigurator AddFromAssembly<T>(this IBrighterConfigurator configurator, AutoFromAssembly fromAssembly)
    {
        return AddFromAssembly(configurator, fromAssembly, typeof(T).Assembly);
    }

    public static IBrighterConfigurator AddFromAssembly(this IBrighterConfigurator configurator, AutoFromAssembly fromAssembly, params Assembly[] assemblies)
    {
        configurator.FromAssembly = fromAssembly;
        
        if (!configurator.Assemblies.TryGetValue(fromAssembly.ToString(), out var registerAssemblies))
        {
            configurator.Assemblies[fromAssembly.ToString()] = registerAssemblies = [];
        }
        
        registerAssemblies.AddRange(assemblies);
        return configurator;
    }
}