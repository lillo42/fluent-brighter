using System;

using Fluent.Brighter.Kafka;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="FluentBrighterBuilder"/> to enable
/// Kafka integration using a high-level, fluent configuration API.
/// </summary>
public static class FluentBrighterExtensions
{
    /// <summary>
    /// Configures Kafka messaging (connection, publications, and subscriptions)
    /// for the Brighter application using a dedicated Kafka configurator.
    /// </summary>
    /// <param name="builder">The Brighter application builder instance.</param>
    /// <param name="configure">An action that configures a <see cref="KafkaConfigurator"/>.</param>
    /// <returns>The Brighter builder for chaining.</returns>
    public static FluentBrighterBuilder UsingKafka(this FluentBrighterBuilder builder,
        Action<KafkaConfigurator> configure)
    {
        var configurator = new KafkaConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}