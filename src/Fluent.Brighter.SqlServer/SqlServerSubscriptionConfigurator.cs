using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.MsSql;

namespace Fluent.Brighter.SqlServer;

/// <summary>
/// Configures a collection of message subscriptions for consumption from SQL Server-backed channels
/// within the Fluent Brighter messaging framework.
/// </summary>
/// <remarks>
/// This class simplifies the registration of one or more <see cref="Subscription"/> instances,
/// automatically associating them with a provided <see cref="ChannelFactory"/> that handles
/// SQL Server-based message retrieval. 
/// </remarks>
public class SqlServerSubscriptionConfigurator(ChannelFactory channelFactory)
{
    /// <summary>
    /// Gets the list of configured subscriptions that will be used to consume messages.
    /// </summary>
    internal List<Subscription> Subscriptions { get; } = [];

    /// <summary>
    /// Adds an already-built <see cref="Subscription"/> to the configuration.
    /// </summary>
    /// <param name="subscription">The subscription to add.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionConfigurator"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="subscription"/> is null.</exception>
    public SqlServerSubscriptionConfigurator AddSubscription(Subscription subscription)
    {
        Subscriptions.Add(subscription ?? throw new ArgumentNullException(nameof(subscription)));
        return this;
    }

    /// <summary>
    /// Adds a new subscription by configuring it fluently using a <see cref="SqlServerSubscriptionBuilder"/>.
    /// The builder is automatically associated with the internal <see cref="ChannelFactory"/> for SQL Server.
    /// </summary>
    /// <param name="configure">An action that customizes the subscription builder.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionConfigurator"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public SqlServerSubscriptionConfigurator AddSubscription(Action<SqlServerSubscriptionBuilder> configure)
    {
        var builder = new SqlServerSubscriptionBuilder();
        configure?.Invoke(builder);
        builder.SetChannelFactory(channelFactory);
        return AddSubscription(builder.Build());
    }

    /// <summary>
    /// Adds a subscription for a specific request type <typeparamref name="TRequest"/>,
    /// automatically setting the data type and allowing further customization via a builder action.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request/command being consumed. Must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="configure">An optional action to further configure the subscription builder.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionConfigurator"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public SqlServerSubscriptionConfigurator AddSubscription<TRequest>(
        Action<SqlServerSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        return AddSubscription(cfg =>
        {
            cfg.SetDataType(typeof(TRequest));
            configure?.Invoke(cfg);
        });
    }
}