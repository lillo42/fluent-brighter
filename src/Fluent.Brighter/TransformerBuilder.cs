using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Paramore.Brighter.Extensions.DependencyInjection;

namespace Fluent.Brighter;

/// <summary>
/// Builder for configuring message transformers in Brighter's pipeline
/// </summary>
/// <remarks>
/// Provides a fluent interface to specify assemblies containing message transformer implementations.
/// Automatically scans non-Microsoft/non-System assemblies for transformers by default.
/// </remarks>
public sealed class TransformerBuilder
{
    private List<Assembly> _assemblies = [];
    
    /// <summary>
    /// Initializes a new TransformerBuilder instance
    /// </summary>
    /// <remarks>
    /// Automatically includes all non-dynamic assemblies in the current AppDomain that
    /// are not Microsoft or System assemblies for transformer discovery.
    /// </remarks>
    public TransformerBuilder()
    {
        _assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(a =>
            !a.IsDynamic 
            && a.FullName?.StartsWith("Microsoft.", true, CultureInfo.InvariantCulture) == false 
            && a.FullName?.StartsWith("System.", true, CultureInfo.InvariantCulture) == false
            && a.FullName?.StartsWith("Paramore.Brighter", true, CultureInfo.InvariantCulture) == false));
    }
    
    /// <summary>
    /// Specifies specific assemblies to scan for message transformers
    /// </summary>
    /// <param name="assemblies">The assemblies to include in transformer scanning</param>
    /// <returns>The builder instance for fluent chaining</returns>
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