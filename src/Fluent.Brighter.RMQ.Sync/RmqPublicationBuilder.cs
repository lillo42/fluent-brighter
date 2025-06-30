using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Sync;

namespace Fluent.Brighter.RMQ.Sync;

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

    private RoutingKey? _topic;

    /// <summary>
    /// Sets the topic
    /// </summary>
    /// <param name="topic">The topic name</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqPublicationBuilder Topic(RoutingKey topic)
    {
        _topic = topic;
        return this;
    }

    private Type? _requestType;

    /// <summary>
    /// Sets the request type
    /// </summary>
    /// <param name="type">The request type.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqPublicationBuilder RequestType(Type? type)
    {
        _requestType = type;
        return this;
    }
    /// <summary>
    /// Sets the request type
    /// </summary>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RmqPublicationBuilder RequestType<TRequest>()
        where TRequest : class, IRequest
    {
        return RequestType(typeof(TRequest));
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
            Topic = _topic,
            RequestType = _requestType
        };
    }
}