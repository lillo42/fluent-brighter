using System;

using Fluent.Brighter.Redis;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="RedisMessageProducerFactoryBuilder"/> to simplify configuration.
/// These extensions enable fluent configuration of Redis message producer factories, including
/// connection settings and publication mappings.
/// </summary>
public static class RedisMessageProducerFactoryBuilderExtensions
{
    /// <summary>
    /// Sets the Redis messaging gateway configuration using a fluent configuration builder.
    /// This extension method creates a <see cref="RedisMessagingGatewayConfigurationBuilder"/>, applies the provided configuration,
    /// and sets the resulting configuration on the producer factory builder.
    /// </summary>
    /// <param name="builder">The <see cref="RedisMessageProducerFactoryBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RedisMessagingGatewayConfigurationBuilder"/> with connection details and settings.</param>
    /// <returns>The <see cref="RedisMessageProducerFactoryBuilder"/> instance for method chaining.</returns>
    public static RedisMessageProducerFactoryBuilder SetConfiguration(this RedisMessageProducerFactoryBuilder builder,
        Action<RedisMessagingGatewayConfigurationBuilder> configure)
    {
        var configuration = new RedisMessagingGatewayConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
    
    /// <summary>
    /// Adds a Redis publication to the producer factory using a configuration builder.
    /// Publications define how messages are published to Redis, including topic mappings and message metadata.
    /// </summary>
    /// <param name="builder">The <see cref="RedisMessageProducerFactoryBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RedisPublicationBuilder"/> with publication settings.</param>
    /// <returns>The <see cref="RedisMessageProducerFactoryBuilder"/> instance for method chaining.</returns>
    public static RedisMessageProducerFactoryBuilder AddPublication(
        this RedisMessageProducerFactoryBuilder builder,
        Action<RedisPublicationBuilder> configure)
    {
        var publication = new RedisPublicationBuilder();
        configure(publication);
        return builder.AddPublication(publication.Build());
    }

    /// <summary>
    /// Adds a Redis publication for a specific request type to the producer factory.
    /// This method automatically configures the publication with the specified <typeparamref name="TRequest"/> type
    /// and then applies additional configuration provided by the action.
    /// </summary>
    /// <typeparam name="TRequest">The type of request/message that this publication will handle. Must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="builder">The <see cref="RedisMessageProducerFactoryBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RedisPublicationBuilder"/> with additional publication settings.</param>
    /// <returns>The <see cref="RedisMessageProducerFactoryBuilder"/> instance for method chaining.</returns>
    public static RedisMessageProducerFactoryBuilder AddPublication<TRequest>(
        this RedisMessageProducerFactoryBuilder builder,
        Action<RedisPublicationBuilder> configure)
        where TRequest : class, IRequest
    {
        var publication = new RedisPublicationBuilder();
        publication.SetRequestType(typeof(TRequest));
        configure(publication);
        return builder.AddPublication(publication.Build());
    } 
}