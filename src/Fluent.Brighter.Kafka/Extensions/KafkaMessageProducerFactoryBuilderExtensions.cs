using System;

using Fluent.Brighter.Kafka;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="KafkaMessageProducerFactoryBuilder"/>
/// to simplify setting Kafka connection configuration via a fluent builder.
/// </summary>
public static class KafkaMessageProducerFactoryBuilderExtensions
{
    /// <summary>
    /// Configures the Kafka messaging gateway settings for the producer factory
    /// using a builder action.
    /// </summary>
    /// <param name="builder">The Kafka producer factory builder instance.</param>
    /// <param name="configure">An action that configures a <see cref="KafkaMessagingGatewayConfigurationBuilder"/>.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaMessageProducerFactoryBuilder SetConfiguration(this KafkaMessageProducerFactoryBuilder builder,
        Action<KafkaMessagingGatewayConfigurationBuilder> configure)
    {
        var configuration = new KafkaMessagingGatewayConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
    
    /// <summary>
    /// Adds a Kafka publication by configuring it through a builder action.
    /// </summary>
    /// <param name="builder">The Kafka producer factory builder instance.</param>
    /// <param name="configure">An action that configures a <see cref="KafkaPublicationBuilder"/>.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaMessageProducerFactoryBuilder AddPublication(
        this KafkaMessageProducerFactoryBuilder builder,
        Action<KafkaPublicationBuilder> configure)
    {
        var publication = new KafkaPublicationBuilder();
        configure(publication);
        return builder.AddPublication(publication.Build());
    }

    /// <summary>
    /// Adds a Kafka publication for a specific request type with type-safe configuration.
    /// The request type is automatically registered, and additional settings can be applied.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <param name="builder">The Kafka producer factory builder instance.</param>
    /// <param name="configure">An action to further configure the publication builder.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaMessageProducerFactoryBuilder AddPublication<TRequest>(
        this KafkaMessageProducerFactoryBuilder builder,
        Action<KafkaPublicationBuilder> configure)
    {
        var publication = new KafkaPublicationBuilder();
        publication.SetRequestType(typeof(TRequest));
        configure(publication);
        return builder.AddPublication(publication.Build());
    } 
}