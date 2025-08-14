using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Sync;

namespace Fluent.Brighter.RMQ.Sync;

public class AmqpUriSpecificationBuilder
{
    private Uri? _uri;
    public AmqpUriSpecificationBuilder SetUri(Uri uri)
    {
        _uri = uri;
        return this;
    }

    private int _connectionRetryCount;
    public AmqpUriSpecificationBuilder SetConnectionRetryCount(int count)
    {
        _connectionRetryCount = count;
        return this;
    }

    private int _retryWaitInMilliseconds;
    public AmqpUriSpecificationBuilder SetRetryWaitInMilliseconds(int milliseconds)
    {
        _retryWaitInMilliseconds = milliseconds;
        return this;
    }

    private int _circuitBreakTimeInMilliseconds;
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
