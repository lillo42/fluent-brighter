using System;

using Paramore.Brighter.MessagingGateway.RMQ.Async;

namespace Fluent.Brighter.RMQ.Async;

/// <summary>
/// Fluent builder for configuring RabbitMQ messaging gateway connections.
/// Provides a chainable API to define connection settings before creating the final <see cref="RmqMessagingGatewayConnection"/> instance.
/// </summary>
public class RmqConnectionBuilder
{
    private string _name = Environment.MachineName;

    /// <summary>
    /// Sets the name of the connection (default: machine name).
    /// </summary>
    /// <param name="name">The connection name (can be empty but not null).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <see langword="null"/>.</exception>
    public RmqConnectionBuilder Name(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name can't be null or empty", nameof(name));
        }

        _name = name;
        return this;
    }

    private AmqpUriSpecification? _amqpUri;

    /// <summary>
    /// Configures the AMQP connection settings using a specification builder.
    /// </summary>
    /// <param name="configure">Action to customize the AMQP URI specification.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is <see langword="null"/>.</exception>
    public RmqConnectionBuilder AmqpUriSpecification(Action<AmqpUriSpecificationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = new AmqpUriSpecificationBuilder();
        configure(builder);
        return AmqpUriSpecification(builder.Build());
    }

    /// <summary>
    /// Sets an existing AMQP URI specification for the connection.
    /// </summary>
    /// <param name="specification">The pre-configured AMQP URI specification.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="specification"/> is <see langword="null"/>.</exception>
    public RmqConnectionBuilder AmqpUriSpecification(AmqpUriSpecification specification)
    {
        _amqpUri = specification ?? throw new ArgumentNullException(nameof(specification));
        return this;
    }

    private Exchange? _exchange;

    /// <summary>
    /// Configures the primary exchange using an exchange builder.
    /// </summary>
    /// <param name="configure">Action to customize the exchange settings.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is <see langword="null"/>.</exception>
    public RmqConnectionBuilder Exchange(Action<ExchangeBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = new ExchangeBuilder();
        configure(builder);
        return Exchange(builder.Build());
    }

    /// <summary>
    /// Sets an existing exchange configuration for the primary exchange.
    /// </summary>
    /// <param name="exchange">The pre-configured exchange instance.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exchange"/> is <see langword="null"/>.</exception>
    public RmqConnectionBuilder Exchange(Exchange exchange)
    {
        _exchange = exchange ?? throw new ArgumentNullException(nameof(exchange));
        return this;
    }

    private Exchange? _deadLetterExchange;

    /// <summary>
    /// Configures the dead-letter exchange using an exchange builder.
    /// </summary>
    /// <param name="configure">Action to customize the dead-letter exchange settings.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is <see langword="null"/>.</exception>
    public RmqConnectionBuilder DeadLetterExchange(Action<ExchangeBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = new ExchangeBuilder();
        configure(builder);
        return DeadLetterExchange(builder.Build());
    }

    /// <summary>
    /// Sets an existing exchange configuration for the dead-letter exchange.
    /// </summary>
    /// <param name="exchange">The pre-configured dead-letter exchange instance.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exchange"/> is <see langword="null"/>.</exception>
    public RmqConnectionBuilder DeadLetterExchange(Exchange exchange)
    {
        _deadLetterExchange = exchange ?? throw new ArgumentNullException(nameof(exchange));
        return this;
    }

    private ushort _heartbeat = 20;

    /// <summary>
    /// Sets the heartbeat interval in seconds for connection health monitoring.
    /// </summary>
    /// <param name="heartbeat">Heartbeat interval (default: 20 seconds).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqConnectionBuilder HeartBeat(ushort heartbeat)
    {
        _heartbeat = heartbeat;
        return this;
    }

    private ushort _continuationTimeout = 20;

    /// <summary>
    /// Sets the continuation timeout in seconds for AMQP protocol operations.
    /// </summary>
    /// <param name="continuationTimeout">Timeout duration (default: 20 seconds).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqConnectionBuilder ContinuationTimeout(ushort continuationTimeout)
    {
        _continuationTimeout = continuationTimeout;
        return this;
    }

    private bool _persistMessages;

    /// <summary>
    /// Enables message persistence to survive broker restarts.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqConnectionBuilder EnablePersistMessage()
    {
        return PersistMessage(true);
    }

    /// <summary>
    /// Disables message persistence (messages will be lost on broker restart).
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqConnectionBuilder DisablePersistMessage()
    {
        return PersistMessage(false);
    }

    /// <summary>
    /// Sets whether messages should be persisted to disk.
    /// </summary>
    /// <param name="persistMessage">True for durable messages, false for transient.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqConnectionBuilder PersistMessage(bool persistMessage)
    {
        _persistMessages = persistMessage;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured <see cref="RmqMessagingGatewayConnection"/> instance.
    /// </summary>
    /// <returns>A new <see cref="RmqMessagingGatewayConnection"/> with the specified configuration.</returns>
    internal RmqMessagingGatewayConnection Build()
    {
        return new RmqMessagingGatewayConnection
        {
            Name = _name,
            AmpqUri = _amqpUri,
            Exchange = _exchange,
            DeadLetterExchange = _deadLetterExchange,
            Heartbeat = _heartbeat,
            PersistMessages = _persistMessages,
            ContinuationTimeout = _continuationTimeout
        };
    }
}