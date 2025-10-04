using System;

using Fluent.Brighter.Kafka;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="ConsumerBuilder"/> to simplify
/// the registration of Kafka-based subscriptions and channel factories
/// in a Brighter application.
/// </summary>
public static class ConsumerBuilderExtensions
{
    /// <summary>
    /// Adds a pre-configured Kafka subscription to the Brighter consumer pipeline.
    /// </summary>
    /// <param name="builder">The Brighter consumer builder instance.</param>
    /// <param name="subscription">The Kafka subscription to add.</param>
    /// <returns>The consumer builder for chaining.</returns>
    public static ConsumerBuilder AddKafkaSubscription(this ConsumerBuilder builder, KafkaSubscription subscription)
        => builder.AddSubscription(subscription);
    
    /// <summary>
    /// Adds a Kafka subscription by configuring it through a builder action.
    /// </summary>
    /// <param name="builder">The Brighter consumer builder instance.</param>
    /// <param name="configure">An action that configures a <see cref="KafkaSubscriptionBuilder"/>.</param>
    /// <returns>The consumer builder for chaining.</returns>
    public static ConsumerBuilder AddKafkaSubscription(this ConsumerBuilder builder, 
        Action<KafkaSubscriptionBuilder> configure)
    {
        var sub = new KafkaSubscriptionBuilder();
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }
    
    /// <summary>
    /// Adds a Kafka subscription for a specific request type with type-safe configuration.
    /// The request type is automatically registered, and additional settings can be applied.
    /// </summary>
    /// <typeparam name="TRequest">The request type, which must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="builder">The Brighter consumer builder instance.</param>
    /// <param name="configure">An action to further configure the subscription builder.</param>
    /// <returns>The consumer builder for chaining.</returns>
    public static ConsumerBuilder AddKafkaSubscription<TRequest>(this ConsumerBuilder builder, 
        Action<KafkaSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        var sub = new KafkaSubscriptionBuilder();
        sub.SetDataType(typeof(TRequest));
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }

    /// <summary>
    /// Adds a Kafka channel factory using a connection configuration built via a fluent action.
    /// </summary>
    /// <param name="builder">The Brighter consumer builder instance.</param>
    /// <param name="configure">An action that configures a <see cref="KafkaMessagingGatewayConfigurationBuilder"/>.</param>
    /// <returns>The consumer builder for chaining.</returns>
    public static ConsumerBuilder AddKafkaChannelFactory(this ConsumerBuilder builder,
        Action<KafkaMessagingGatewayConfigurationBuilder> configure)
    {
        var connection = new KafkaMessagingGatewayConfigurationBuilder();
        configure(connection);
        return builder.AddKafkaChannelFactory(connection.Build());
    }
    
    /// <summary>
    /// Adds a Kafka channel factory using a pre-built connection configuration.
    /// </summary>
    /// <param name="builder">The Brighter consumer builder instance.</param>
    /// <param name="connection">The Kafka messaging gateway configuration.</param>
    /// <returns>The consumer builder for chaining.</returns>
    public static ConsumerBuilder AddKafkaChannelFactory(this ConsumerBuilder builder, KafkaMessagingGatewayConfiguration connection)
    {
        return builder
            .AddChannelFactory(new ChannelFactory(new KafkaMessageConsumerFactory(connection)));
    }
}
