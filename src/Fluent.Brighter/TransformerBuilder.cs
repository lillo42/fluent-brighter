using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Paramore.Brighter.Extensions.DependencyInjection;

namespace Fluent.Brighter;

public sealed class TransformerBuilder
{
    private List<Assembly> _assemblies = [];
    
    public TransformerBuilder()
    {
        _assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(a =>
            !a.IsDynamic && a.FullName?.StartsWith("Microsoft.", true, CultureInfo.InvariantCulture) != true &&
            a.FullName?.StartsWith("System.", true, CultureInfo.InvariantCulture) != true));
    }
    
    public TransformerBuilder FromAssemblies(params Assembly[] assemblies)
    {
        _assemblies = assemblies.ToList();
        return this;
    }

    internal void SetTransforms(IBrighterBuilder brighter)
    {
        brighter.TransformsFromAssemblies(_assemblies);
    }
}