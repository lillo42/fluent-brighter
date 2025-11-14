using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Redis;

namespace Fluent.Brighter.Redis.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ConsumerBuilder"/> to configure Redis-based message consumers.
/// These extensions enable easy setup of Redis subscriptions and channel factories.
/// </summary>
public static class ConsumerBuilderExtensions
{
    /// <summary>
    /// Adds a pre-configured Redis subscription to the consumer builder.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> instance to configure.</param>
    /// <param name="subscription">The pre-configured <see cref="RedisSubscription"/> to add.</param>
    /// <returns>The <see cref="ConsumerBuilder"/> instance for method chaining.</returns>
    public static ConsumerBuilder AddRedisSubscription(this ConsumerBuilder builder, RedisSubscription subscription)
    {
        return builder.AddSubscription(subscription);
    }

    /// <summary>
    /// Adds a Redis subscription to the consumer builder using a configuration action.
    /// This method creates a new <see cref="RedisSubscriptionBuilder"/>, applies the configuration, and builds the subscription.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RedisSubscriptionBuilder"/> with subscription settings.</param>
    /// <returns>The <see cref="ConsumerBuilder"/> instance for method chaining.</returns>
    public static ConsumerBuilder AddRedisSubscription(this ConsumerBuilder builder,
        Action<RedisSubscriptionBuilder> configure)
    {
        var sub = new RedisSubscriptionBuilder();
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }
    
    /// <summary>
    /// Adds a Redis subscription for a specific request type to the consumer builder.
    /// This method automatically configures the subscription with the specified <typeparamref name="TRequest"/> type
    /// and then applies additional configuration provided by the action.
    /// </summary>
    /// <typeparam name="TRequest">The type of request/message that this subscription will handle. Must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RedisSubscriptionBuilder"/> with additional settings.</param>
    /// <returns>The <see cref="ConsumerBuilder"/> instance for method chaining.</returns>
    public static ConsumerBuilder AddRedisSubscription<TRequest>(this ConsumerBuilder builder, 
        Action<RedisSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        var sub = new RedisSubscriptionBuilder();
        sub.SetDataType(typeof(TRequest));
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }

    /// <summary>
    /// Adds a Redis channel factory to the consumer builder using a configuration builder.
    /// Channel factories are responsible for creating message channels that consume messages from Redis.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RedisMessagingGatewayConfigurationBuilder"/> with connection details.</param>
    /// <returns>The <see cref="ConsumerBuilder"/> instance for method chaining.</returns>
    public static ConsumerBuilder AdRedisChannelFactory(this ConsumerBuilder builder,
        Action<RedisMessagingGatewayConfigurationBuilder> configure)
    {
        var connection = new RedisMessagingGatewayConfigurationBuilder();
        configure(connection);
        return builder.AddRedisChannelFactory(connection.Build());
    }
    
    /// <summary>
    /// Adds a Redis channel factory to the consumer builder using a pre-configured Redis messaging gateway configuration.
    /// Channel factories are responsible for creating message channels that consume messages from Redis.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> instance to configure.</param>
    /// <param name="connection">The pre-configured Redis messaging gateway configuration containing connection details.</param>
    /// <returns>The <see cref="ConsumerBuilder"/> instance for method chaining.</returns>
    public static ConsumerBuilder AddRedisChannelFactory(this ConsumerBuilder builder, RedisMessagingGatewayConfiguration connection)
    {
        return builder
            .AddChannelFactory(new ChannelFactory(new RedisMessageConsumerFactory(connection)));
    }
}