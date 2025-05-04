using Microsoft.Extensions.DependencyInjection;

namespace Fluent.Brighter;

public class BrighterOptions
{
    public ServiceLifetime CommandProcessorLifetime { get; set; } = ServiceLifetime.Transient;
}