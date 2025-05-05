
using System;

namespace Fluent.Brighter;

/// <summary>
/// A flags enumeration to specify which components to search for during assembly registration.
/// This allows combining multiple options using bitwise operations.
/// </summary>
[Flags]
public enum AutoFromAssembly
{
    /// <summary>
    /// No components selected. Default value for unconfigured scenarios.
    /// </summary>
    None = 0,

    /// <summary>
    /// Indicates the registration process should look for mapper components.
    /// </summary>
    Mappers = 1,

    /// <summary>
    /// Indicates the registration process should look for handler components.
    /// </summary>
    Handlers = 2,

    /// <summary>
    /// Indicates the registration process should look for transform components.
    /// </summary>
    Transforms = 4,

    /// <summary>
    /// Combines all available components (Mappers, Handlers, and Transforms) for comprehensive registration.
    /// </summary>
    All = Mappers | Handlers | Transforms
}