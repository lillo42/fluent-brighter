using Paramore.Brighter.MessageMappers;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for <see cref="MapperBuilder"/> to configure default message mappers
/// </summary>
public static class MapperBuilderExtensions
{
    /// <summary>
    /// Sets CloudEvent JSON as the default message mapper for the Brighter configuration.
    /// CloudEvents is a specification for describing event data in a common way.
    /// </summary>
    /// <param name="mapperBuilder">The mapper builder to configure</param>
    /// <returns>The configured mapper builder for method chaining</returns>
    /// <remarks>
    /// This configures Brighter to use CloudEvent format for message serialization,
    /// which provides a standardized way to describe event data across services.
    /// </remarks>
    public static MapperBuilder SetCloudEventJsonAsDefaultMapper(this MapperBuilder mapperBuilder)
    {
        return mapperBuilder.SetDefaultMapper(typeof(CloudEventJsonMessageMapper<>));
    }
    
    /// <summary>
    /// Sets standard JSON as the default message mapper for the Brighter configuration.
    /// </summary>
    /// <param name="mapperBuilder">The mapper builder to configure</param>
    /// <returns>The configured mapper builder for method chaining</returns>
    /// <remarks>
    /// This configures Brighter to use standard JSON format for message serialization.
    /// Use this for simple JSON serialization without CloudEvent envelope formatting.
    /// </remarks>
    public static MapperBuilder SetJsonAsDefaultMapper(this MapperBuilder mapperBuilder)
    {
        return mapperBuilder.SetDefaultMapper(typeof(JsonMessageMapper<>));
    }
}