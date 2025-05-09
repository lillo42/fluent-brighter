using System;

using Paramore.Brighter.MessagingGateway.RMQ;

namespace Fluent.Brighter.RMQ;

/// <summary>
/// Builder class for constructing <see cref="AmqpUriSpecification"/> instances with a fluent API.
/// Provides configuration options for AMQP connection settings and retry policies.
/// </summary>
public class AmqpUriSpecificationBuilder
{
    private Uri? _uri;
    public AmqpUriSpecificationBuilder Uri(string uri)
    {
        if(string.IsNullOrEmpty(uri))
        {}
        return Uri(new Uri(uri));
    }

    /// <summary>
    /// Sets the AMQP endpoint URI using a <see cref="System.Uri"/> object.
    /// </summary>
    /// <param name="uri">The pre-constructed <see cref="System.Uri"/> instance.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri"/> is <see langword="null"/>.</exception>
    public AmqpUriSpecificationBuilder Uri(Uri uri)
    {
        _uri = uri ?? throw new ArgumentNullException(nameof(uri));
        return this;
    }

    private int _connectionRetryCount = 3;

    /// <summary>
    /// Sets the number of connection retry attempts before failing.
    /// </summary>
    /// <param name="connectionRetryCount">The retry count (default: 3).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="connectionRetryCount"/> is less than 0.</exception>
    public AmqpUriSpecificationBuilder ConnectionRetryCount(int connectionRetryCount)
    {
        if (connectionRetryCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(connectionRetryCount), connectionRetryCount, "need to be equal or large than 0");
        }

        _connectionRetryCount = connectionRetryCount;
        return this;
    }

    private int _retryWaitInMilliseconds = 1_000;

    /// <summary>
    /// Sets the wait time between retry attempts using a <see cref="TimeSpan"/>.
    /// Converts the time span to total milliseconds for internal storage.
    /// </summary>
    /// <param name="retryWait">The time span representing wait duration between retries.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public AmqpUriSpecificationBuilder RetryWait(TimeSpan retryWait)
    {
        return RetryWaitInMilliseconds(Convert.ToInt32(retryWait.TotalMilliseconds));
    }

    /// <summary>
    /// Sets the wait time between retry attempts in milliseconds.
    /// </summary>
    /// <param name="retryWaitInMilliseconds">The wait time in milliseconds (default: 1000).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="retryWaitInMilliseconds"/> is less than 0.</exception>
    public AmqpUriSpecificationBuilder RetryWaitInMilliseconds(int retryWaitInMilliseconds)
    {
        if (retryWaitInMilliseconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(retryWaitInMilliseconds), retryWaitInMilliseconds, "need to be equal or large than 0");
        }

        _retryWaitInMilliseconds = retryWaitInMilliseconds;
        return this;
    }

    private int _circuitBreakTimeInMilliseconds = 60_000;

    /// <summary>
    /// Sets the duration that the circuit breaker remains open after failures using a <see cref="TimeSpan"/>.
    /// Converts the time span to total milliseconds for internal storage.
    /// </summary>
    /// <param name="circuitBreakTime">The time span representing how long the circuit breaker should remain open.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public AmqpUriSpecificationBuilder CircuitBreakTime(TimeSpan circuitBreakTime)
    {
        return CircuitBreakTimeInMilliseconds(Convert.ToInt32(circuitBreakTime.TotalMilliseconds));
    }

    /// <summary>
    /// Sets the duration in milliseconds that the circuit breaker remains open after failures.
    /// </summary>
    /// <param name="circuitBreakTimeInMilliseconds">The circuit break duration in milliseconds (default: 60000).</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="circuitBreakTimeInMilliseconds"/> is less than 0.</exception>
    public AmqpUriSpecificationBuilder CircuitBreakTimeInMilliseconds(int circuitBreakTimeInMilliseconds)
    {
        if (circuitBreakTimeInMilliseconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(circuitBreakTimeInMilliseconds), circuitBreakTimeInMilliseconds, "need to be equal or large than 0");
        }

        _circuitBreakTimeInMilliseconds = circuitBreakTimeInMilliseconds;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured <see cref="AmqpUriSpecification"/> instance.
    /// </summary>
    /// <returns>A new <see cref="AmqpUriSpecification"/> with the configured settings.</returns>
    internal AmqpUriSpecification Build()
    {
        return new AmqpUriSpecification(_uri, _connectionRetryCount, _retryWaitInMilliseconds, _circuitBreakTimeInMilliseconds);
    }
}