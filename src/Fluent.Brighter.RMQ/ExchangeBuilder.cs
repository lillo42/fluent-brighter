
using System;

using Paramore.Brighter.MessagingGateway.RMQ;

namespace Fluent.Brighter.RMQ;

/// <summary>
/// Fluent builder for configuring AMQP exchange settings.
/// Provides a chainable API to define exchange properties before creating the final <see cref="Exchange"/> instance.
/// </summary>
public class ExchangeBuilder
{
    private string? _name;

    /// <summary>
    /// Sets the name of the exchange.
    /// </summary>
    /// <param name="name">The exchange name (can be empty but not null).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public ExchangeBuilder Name(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name can't be null or empty", nameof(name));
        }

        _name = name;
        return this;
    }

    private string _type = "direct";

    /// <summary>
    /// Sets the exchange type (e.g., "direct", "fanout", "topic", "headers").
    /// </summary>
    /// <param name="type">The exchange type (default: "direct").</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="type"/> is <see langword="null"/> or empty.</exception>
    public ExchangeBuilder Type(string type)
    {
        if (string.IsNullOrEmpty(type))
        {
            throw new ArgumentException("Type can't be null or empty", nameof(type));
        }

        _type = type;
        return this;
    }

    private bool _durable;

    /// <summary>
    /// Enables durable exchange configuration (survives broker restarts).
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public ExchangeBuilder EnableDurable()
    {
        return Durable(true);
    }

    /// <summary>
    /// Disables durable exchange configuration (non-persistent).
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public ExchangeBuilder DisableDurable()
    {
        return Durable(false);
    }

    /// <summary>
    /// Sets whether the exchange should be durable.
    /// </summary>
    /// <param name="durable">True for durable, false for transient.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public ExchangeBuilder Durable(bool durable)
    {
        _durable = durable;
        return this;
    }

    private bool _supportDelay;

    /// <summary>
    /// Enables support for delayed message delivery.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public ExchangeBuilder EnableSupportDelay()
    {
        return SupportDelay(true);
    }

    /// <summary>
    /// Disables support for delayed message delivery.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public ExchangeBuilder DisableSupportDelay()
    {
        return SupportDelay(false);
    }

    /// <summary>
    /// Sets whether the exchange should support delayed messages.
    /// </summary>
    /// <param name="supportDelay">True to enable delay support, false to disable.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public ExchangeBuilder SupportDelay(bool supportDelay)
    {
        _supportDelay = supportDelay;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured <see cref="Exchange"/> instance.
    /// </summary>
    /// <returns>A new <see cref="Exchange"/> with the specified configuration.</returns>
    internal Exchange Build()
    {
        return new Exchange(_name, _type, _durable, _supportDelay);
    }
}