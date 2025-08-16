using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Async;

namespace Fluent.Brighter.RMQ.Async;

/// <summary>
/// Fluent builder for configuring RabbitMQ URI connection settings in Brighter.
/// </summary>
/// <remarks>
/// Provides a fluent interface to configure URI and resilience parameters for RabbitMQ connections.
/// </remarks>
public class AmqpUriSpecificationBuilder
{
    private Uri? _uri;
    
    /// <summary>
    /// Sets the RabbitMQ connection URI
    /// </summary>
    /// <param name="uri">The RabbitMQ connection URI (e.g., amqp://user:pass@host:port/vhost)</param>
    /// <returns>This builder for fluent chaining</returns>
    public AmqpUriSpecificationBuilder SetUri(Uri uri)
    {
        _uri = uri;
        return this;
    }

    private int _connectionRetryCount;
    
    /// <summary>
    /// Sets the maximum number of connection retry attempts
    /// </summary>
    /// <param name="count">Number of retry attempts (0 = no retries)</param>
    /// <returns>This builder for fluent chaining</returns>
    public AmqpUriSpecificationBuilder SetConnectionRetryCount(int count)
    {
        _connectionRetryCount = count;
        return this;
    }

    private int _retryWaitInMilliseconds;
    
    /// <summary>
    /// Sets the delay between connection retry attempts
    /// </summary>
    /// <param name="milliseconds">Wait time in milliseconds between retries</param>
    /// <returns>This builder for fluent chaining</returns>
    public AmqpUriSpecificationBuilder SetRetryWaitInMilliseconds(int milliseconds)
    {
        _retryWaitInMilliseconds = milliseconds;
        return this;
    }
    
    private int _circuitBreakTimeInMilliseconds;
    
    /// <summary>
    /// Sets the duration to break the circuit when connection failures exceed thresholds
    /// </summary>
    /// <param name="milliseconds">Time in milliseconds to keep circuit broken</param>
    /// <returns>This builder for fluent chaining</returns>
    public AmqpUriSpecificationBuilder SetCircuitBreakTimeInMilliseconds(int milliseconds)
    {
        _circuitBreakTimeInMilliseconds = milliseconds;
        return this;
    }

    internal AmqpUriSpecification Build()
    {
        if (_uri == null)
        {
            throw new ConfigurationException("Uri was not specified");
        }
        
        return new AmqpUriSpecification(
            _uri,
            _connectionRetryCount,
            _retryWaitInMilliseconds,
            _circuitBreakTimeInMilliseconds
        );
    }
}
