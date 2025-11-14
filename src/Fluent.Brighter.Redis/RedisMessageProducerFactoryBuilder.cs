using System.Collections.Generic;
using System.Linq;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Redis;

namespace Fluent.Brighter.Redis;

/// <summary>
/// Builder class for configuring and creating a Redis message producer factory.
/// This factory is responsible for creating message producers that publish messages to Redis-based messaging gateway.
/// </summary>
public sealed class RedisMessageProducerFactoryBuilder
{
    private RedisMessagingGatewayConfiguration? _configuration;

    /// <summary>
    /// Sets the Redis messaging gateway configuration to be used by the message producers.
    /// </summary>
    /// <param name="configuration">The Redis messaging gateway configuration containing connection details and settings.</param>
    /// <returns>The current <see cref="RedisMessageProducerFactoryBuilder"/> instance for method chaining.</returns>
    public RedisMessageProducerFactoryBuilder SetConfiguration(RedisMessagingGatewayConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }
    
    private List<RedisMessagePublication> _publications = [];
   
    /// <summary>
    /// Sets the collection of Redis message publications that define how messages are published.
    /// Publications define the mapping between message types and Redis channels/topics.
    /// This method replaces any previously configured publications.
    /// </summary>
    /// <param name="publications">An array of <see cref="RedisMessagePublication"/> configurations to set.</param>
    /// <returns>The current <see cref="RedisMessageProducerFactoryBuilder"/> instance for method chaining.</returns>
    public RedisMessageProducerFactoryBuilder SetPublications(params RedisMessagePublication[] publications)
    {
        _publications = publications.ToList();
        return this;
    }
    
    /// <summary>
    /// Adds a single publication configuration to the existing collection.
    /// Publications define the mapping between message types and Redis channels/topics.
    /// </summary>
    /// <param name="publications">The <see cref="RedisMessagePublication"/> configuration to add.</param>
    /// <returns>The current <see cref="RedisMessageProducerFactoryBuilder"/> instance for method chaining.</returns>
    public RedisMessageProducerFactoryBuilder AddPublication(RedisMessagePublication publications)
    {
        _publications.Add(publications);
        return this;
    }
    
    /// <summary>
    /// Builds and returns a configured <see cref="RedisMessageProducerFactory"/> instance.
    /// This method is called internally to create the factory with the configured settings.
    /// </summary>
    /// <returns>A configured <see cref="RedisMessageProducerFactory"/> instance.</returns>
    /// <exception cref="ConfigurationException">Thrown when no configuration has been set via <see cref="SetConfiguration"/>.</exception>
    internal RedisMessageProducerFactory Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("The configuration was not set");
        }
        
        return new RedisMessageProducerFactory(_configuration, _publications);
    }
}