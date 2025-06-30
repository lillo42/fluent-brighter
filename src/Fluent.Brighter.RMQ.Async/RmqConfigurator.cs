using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Async;

namespace Fluent.Brighter.RMQ.Async;

/// <summary>
/// Configures RabbitMQ integration for Brighter messaging.
/// Provides methods to define connections, subscriptions, and publications through a fluent API.
/// </summary>
public class RmqConfigurator
{
    private readonly List<RmqPublication> _publications = [];
    private readonly List<Subscription> _subscriptions = [];
    private RmqMessagingGatewayConnection? _connection;

    /// <summary>
    /// Configures the RabbitMQ connection using a builder pattern.
    /// </summary>
    /// <param name="configure">Action to customize the connection settings.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is <see langword="null"/>.</exception>
    public RmqConfigurator Connection(Action<RmqConnectionBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = new RmqConnectionBuilder();
        configure(builder);
        _connection = builder.Build();
        return this;
    }

    /// <summary>
    /// Adds a subscription configuration to the RabbitMQ setup.
    /// </summary>
    /// <param name="configure">Action to customize the subscription settings.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is <see langword="null"/>.</exception>
    public RmqConfigurator Subscription(Action<RmqSubscriptionBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = new RmqSubscriptionBuilder();
        configure(builder);
        _subscriptions.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds a strongly-typed subscription configuration for a specific request type.
    /// </summary>
    /// <typeparam name="T">The request type that this subscription will handle.</typeparam>
    /// <param name="configure">Action to customize the subscription settings.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is <see langword="null"/>.</exception>
    public RmqConfigurator Subscription<T>(Action<RmqSubscriptionBuilder> configure)
        where T : class, IRequest
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = new RmqSubscriptionBuilder()
            .MessageType<T>();
        configure(builder);
        _subscriptions.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds a publication configuration to the RabbitMQ setup.
    /// </summary>
    /// <param name="configure">Action to customize the publication settings.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is <see langword="null"/>.</exception>
    public RmqConfigurator Publication(Action<RmqPublicationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = new RmqPublicationBuilder();
        configure(builder);
        _publications.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Internal method to apply RabbitMQ configuration to the Brighter configurator.
    /// </summary>
    /// <param name="register">The Brighter configurator to extend.</param>
    /// <returns>The updated Brighter configurator instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when connection is not configured.</exception>
    internal IBrighterConfigurator AddRabbitMq(IBrighterConfigurator register)
    {
        if (_publications.Count == 0 && _subscriptions.Count == 0)
        {
            return register;
        }

        if (_connection == null)
        {
            throw new ConfigurationException("no connection setup");
        }

        if (_publications.Count > 0)
        {
            _ = register
                 .AddExternalBus(new RmqMessageProducerFactory(_connection, _publications));
        }

        if (_subscriptions.Count > 0)
        {
            _ = register
                .AddChannelFactory(new ChannelFactory(new RmqMessageConsumerFactory(_connection)), _subscriptions);
        }

        return register;
    }
}