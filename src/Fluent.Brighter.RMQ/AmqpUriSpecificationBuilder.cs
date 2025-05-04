
using System;

using Paramore.Brighter.MessagingGateway.RMQ;

namespace Fluent.Brighter.RMQ;

public class AmqpUriSpecificationBuilder
{
    private Uri? _uri;
    public AmqpUriSpecificationBuilder Uri(string uri)
    {
        return Uri(new Uri(uri));
    }

    public AmqpUriSpecificationBuilder Uri(Uri uri)
    {
        _uri = uri;
        return this;
    }

    private int _connectionRetryCount = 3;
    public AmqpUriSpecificationBuilder ConnectionRetryCount(int connectionRetryCount)
    {
        _connectionRetryCount = connectionRetryCount;
        return this;
    }

    private int _retryWaitInMilliseconds = 1_000;
    public AmqpUriSpecificationBuilder RetryWaitInMilliseconds(int retryWaitInMilliseconds)
    {
        _retryWaitInMilliseconds = retryWaitInMilliseconds;
        return this;
    }

    private int _circuitBreakTimeInMilliseconds = 60_000;
    public AmqpUriSpecificationBuilder CircuitBreakTimeInMilliseconds(int circuitBreakTimeInMilliseconds)
    {
        _circuitBreakTimeInMilliseconds = circuitBreakTimeInMilliseconds;
        return this;
    }


    internal AmqpUriSpecification Build()
    {
        return new AmqpUriSpecification(_uri, _connectionRetryCount, _retryWaitInMilliseconds, _circuitBreakTimeInMilliseconds);
    }
}