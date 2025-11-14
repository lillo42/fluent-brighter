using System;

using Fluent.Brighter.Redis;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="ProducerBuilder"/> to configure Redis-based message producers.
/// These extensions enable easy setup of Redis publications for message production.
/// </summary>
public static class ProducersExtensions
{
    /// <summary>
    /// Adds a Redis publication to the producer builder using a configuration action.
    /// Publications define how messages are published to Redis, including topic mappings and message metadata.
    /// </summary>
    /// <param name="builder">The <see cref="ProducerBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RedisMessageProducerFactoryBuilder"/> with publication settings.</param>
    /// <returns>The <see cref="ProducerBuilder"/> instance for method chaining.</returns>
    public static ProducerBuilder AddRedisPublication(
        this ProducerBuilder builder,
        Action<RedisMessageProducerFactoryBuilder> configure)
    {
        var factory = new RedisMessageProducerFactoryBuilder();
        configure(factory);
        return builder.AddMessageProducerFactory(factory.Build());
    }
}