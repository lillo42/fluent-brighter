
using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ;

namespace Fluent.Brighter.RMQ;

/// <summary>
/// Fluent builder for configuring RabbitMQ publication settings.
/// Provides a chainable API to define message publishing behavior before creating the final <see cref="RmqPublication"/> instance.
/// </summary>
public class RmqPublicationBuilder
{
    private int _waitForConfirmsTimeOutInMilliseconds = 500;

    /// <summary>
    /// Sets the timeout for waiting for message confirms in milliseconds.
    /// </summary>
    /// <param name="waitForConfirmsTimeOut">Time span representing the timeout duration.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqPublicationBuilder WaitForConfirmsTimeOut(TimeSpan waitForConfirmsTimeOut)
    {
        return WaitForConfirmsTimeOutInMilliseconds(Convert.ToInt32(waitForConfirmsTimeOut.TotalMilliseconds));
    }

    /// <summary>
    /// Sets the timeout for waiting for message confirms in milliseconds.
    /// </summary>
    /// <param name="waitForConfirmsTimeOutInMilliseconds">Timeout in milliseconds (default: 500).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="waitForConfirmsTimeOutInMilliseconds"/> is negative.</exception>
    public RmqPublicationBuilder WaitForConfirmsTimeOutInMilliseconds(int waitForConfirmsTimeOutInMilliseconds)
    {
        if (waitForConfirmsTimeOutInMilliseconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(waitForConfirmsTimeOutInMilliseconds), "Timeout must be non-negative");
        }

        _waitForConfirmsTimeOutInMilliseconds = waitForConfirmsTimeOutInMilliseconds;
        return this;
    }

    private OnMissingChannel _makeChannel = OnMissingChannel.Create;

    /// <summary>
    /// Configures the publisher to create exchanges if they don't exist.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqPublicationBuilder CreateExchangeIfMissing()
    {
        return MakeExchange(OnMissingChannel.Create);
    }

    /// <summary>
    /// Configures the publisher to validate exchanges exist before publishing.
    /// Throws an exception if the exchange is missing.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqPublicationBuilder ValidateIfExchangeExists()
    {
        return MakeExchange(OnMissingChannel.Validate);
    }

    /// <summary>
    /// Configures the publisher to assume exchanges exist without validation.
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqPublicationBuilder AssumeExchangeExists()
    {
        return MakeExchange(OnMissingChannel.Assume);
    }

    /// <summary>
    /// Sets the behavior when an exchange is missing.
    /// </summary>
    /// <param name="makeChannel">The <see cref="OnMissingChannel"/> mode to use.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqPublicationBuilder MakeExchange(OnMissingChannel makeChannel)
    {
        _makeChannel = makeChannel;
        return this;
    }

    private int _maxOutStandingMessages = -1;

    /// <summary>
    /// Sets the maximum number of outstanding messages allowed before blocking.
    /// </summary>
    /// <param name="maxOutStandingMessages">Maximum number of outstanding messages (default: -1 for unlimited).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxOutStandingMessages"/> is less than -1.</exception>
    public RmqPublicationBuilder MaxOutStandingMessages(int maxOutStandingMessages)
    {
        if (maxOutStandingMessages < -1)
        {
            throw new ArgumentOutOfRangeException(nameof(maxOutStandingMessages), "MaxOutStandingMessages must be -1 or greater");
        }

        _maxOutStandingMessages = maxOutStandingMessages;
        return this;
    }

    private int _maxOutStandingCheckIntervalMilliSeconds;
    
    /// <summary>
    /// Sets the interval for checking outstanding message count in milliseconds.
    /// </summary>
    /// <param name="maxOutStandingCheckInterval">Time span representing the interval.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqPublicationBuilder MaxOutStandingCheckInterval(TimeSpan maxOutStandingCheckInterval)
    {
        return MaxOutStandingCheckIntervalMilliSeconds(Convert.ToInt32(maxOutStandingCheckInterval.TotalMilliseconds));
    }

    /// <summary>
    /// Sets the interval for checking outstanding message count in milliseconds.
    /// </summary>
    /// <param name="maxOutStandingCheckIntervalMilliSeconds">Interval in milliseconds.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxOutStandingCheckIntervalMilliSeconds"/> is negative.</exception>
    public RmqPublicationBuilder MaxOutStandingCheckIntervalMilliSeconds(int maxOutStandingCheckIntervalMilliSeconds)
    {
        if (maxOutStandingCheckIntervalMilliSeconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxOutStandingCheckIntervalMilliSeconds), "Interval must be non-negative");
        }

        _maxOutStandingCheckIntervalMilliSeconds = maxOutStandingCheckIntervalMilliSeconds;
        return this;
    }

    private Dictionary<string, object>? _outboxBag;

    /// <summary>
    /// Adds a key-value pair to the outbox context bag for message metadata.
    /// </summary>
    /// <param name="key">The key for the metadata entry.</param>
    /// <param name="value">The value for the metadata entry.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <see langword="null"/>.</exception>
    public RmqPublicationBuilder OutboxBag(string key, object value)
    {
        _outboxBag ??= [];

        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        _outboxBag[key] = value;
        return this;
    }

    /// <summary>
    /// Adds multiple key-value pairs to the outbox context bag for message metadata.
    /// </summary>
    /// <param name="bag">Collection of key-value pairs to add.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="bag"/> is <see langword="null"/>.</exception>
    public RmqPublicationBuilder OutboxBag(IEnumerable<KeyValuePair<string, object>> bag)
    {
        _outboxBag ??= [];
        foreach (var item in bag ?? throw new ArgumentNullException(nameof(bag)))
        {
            _outboxBag[item.Key] = item.Value;
        }

        return this;
    }

    private RoutingKey? _topic;

    public RmqPublicationBuilder Topic(string topic)
    {
        return Topic(new RoutingKey(topic));
    }
    
    public RmqPublicationBuilder Topic(RoutingKey topic)
    {
        _topic = topic;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured <see cref="RmqPublication"/> instance.
    /// </summary>
    /// <returns>A new <see cref="RmqPublication"/> with the specified configuration.</returns>
    internal RmqPublication Build()
    {
        return new RmqPublication
        {
            WaitForConfirmsTimeOutInMilliseconds = _waitForConfirmsTimeOutInMilliseconds,
            MakeChannels = _makeChannel,
            MaxOutStandingMessages = _maxOutStandingMessages,
            MaxOutStandingCheckIntervalMilliSeconds = _maxOutStandingCheckIntervalMilliSeconds,
            OutBoxBag = _outboxBag,
            Topic = _topic
        };
    }
}