using System;

using Fluent.Brighter.Kafka;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="ProducerBuilder"/> to simplify
/// the registration of Kafka-based message producers in a Brighter application.
/// </summary>
public static class ProducersExtensions
{
    /// <summary>
    /// Adds a Kafka message producer factory to the Brighter producer pipeline,
    /// configured via a fluent builder action.
    /// </summary>
    /// <param name="builder">The Brighter producer builder instance.</param>
    /// <param name="configure">An action that configures a <see cref="KafkaMessageProducerFactoryBuilder"/>.</param>
    /// <returns>The producer builder for chaining.</returns>
    public static ProducerBuilder AddKafkaPublication(
        this ProducerBuilder builder,
        Action<KafkaMessageProducerFactoryBuilder> configure)
    {
        var factory = new KafkaMessageProducerFactoryBuilder();
        configure(factory);
        return builder.AddMessageProducerFactory(factory.Build());
    }
}