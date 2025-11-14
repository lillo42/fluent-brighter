using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter.Postgres;

/// <summary>
/// Configurator class for managing PostgreSQL message subscriptions.
/// This class provides a fluent API for adding and configuring multiple subscriptions
/// that consume messages from PostgreSQL-based messaging queues.
/// </summary>
/// <param name="channelFactory">The PostgreSQL channel factory used to create message channels for subscriptions.</param>
public class PostgresSubscriptionConfigurator(PostgresChannelFactory channelFactory)
{
    /// <summary>
    /// Gets the list of configured PostgreSQL subscriptions.
    /// This property is used internally to collect all subscriptions that have been added to the configurator.
    /// </summary>
    internal List<PostgresSubscription> Subscriptions { get; } = [];

    /// <summary>
    /// Adds a pre-configured PostgreSQL subscription to the configurator.
    /// </summary>
    /// <param name="subscription">The <see cref="PostgresSubscription"/> instance to add.</param>
    /// <returns>The current <see cref="PostgresSubscriptionConfigurator"/> instance for method chaining.</returns>
    public PostgresSubscriptionConfigurator AddSubscription(PostgresSubscription subscription)
    {
        Subscriptions.Add(subscription);
        return this;
    }
    
    /// <summary>
    /// Adds a PostgreSQL subscription using a builder configuration action.
    /// This method creates a new <see cref="PostgresSubscriptionBuilder"/>, applies the provided configuration,
    /// and automatically sets the channel factory before building the subscription.
    /// </summary>
    /// <param name="configure">An action that configures the <see cref="PostgresSubscriptionBuilder"/> to define subscription settings.</param>
    /// <returns>The current <see cref="PostgresSubscriptionConfigurator"/> instance for method chaining.</returns>
    public PostgresSubscriptionConfigurator AddSubscription(Action<PostgresSubscriptionBuilder> configure)
    {
        var builder = new PostgresSubscriptionBuilder();
        configure(builder);
        builder.SetChannelFactory(channelFactory);
        return AddSubscription(builder.Build());
    }

    /// <summary>
    /// Adds a PostgreSQL subscription for a specific request type using a builder configuration action.
    /// This method automatically configures the subscription with the specified <typeparamref name="TRequest"/> type
    /// and then applies any additional configuration provided.
    /// </summary>
    /// <typeparam name="TRequest">The type of request/message that this subscription will handle. Must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="configure">An action that configures the <see cref="PostgresSubscriptionBuilder"/> with additional settings.</param>
    /// <returns>The current <see cref="PostgresSubscriptionConfigurator"/> instance for method chaining.</returns>
    public PostgresSubscriptionConfigurator AddSubscription<TRequest>(
        Action<PostgresSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        return AddSubscription(cfg =>
        {
            cfg.SetDataType(typeof(TRequest));
            configure(cfg);
        });
    }
}