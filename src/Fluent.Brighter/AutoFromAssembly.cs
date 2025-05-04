
using System;

namespace Fluent.Brighter;

[Flags]
public enum AutoFromAssembly
{
    None = 0,
    Mappers = 1,
    Handlers = 2,
    Transforms = 4,
    All = Mappers | Handlers | Transforms
}