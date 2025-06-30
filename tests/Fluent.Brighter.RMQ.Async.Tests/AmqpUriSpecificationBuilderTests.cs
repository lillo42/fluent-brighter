using System;

using Xunit;

namespace Fluent.Brighter.RMQ.Async.Tests;

public class AmqpUriSpecificationBuilderTests
{
    [Fact]
    public void When_SetUriString()
    {
        // Arrange
        var builder = new AmqpUriSpecificationBuilder();
        const string uriString = "amqp://user:pass@host:123";

        // Act
        var result = builder.Uri(uriString);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Equal(new Uri(uriString), builder.Build().Uri);
    }
    
    [Fact]
    public void When_SetUri()
    {
        // Arrange
        var builder = new AmqpUriSpecificationBuilder();
        var uri = new Uri("amqp://user:pass@host:123/vhost");

        // Act
        var result = builder.Uri(uri);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Equal(uri, builder.Build().Uri);
    }

    [Fact]
    public void When_SetNulUri_Should_Throw()
    {
        // Arrange
        var builder = new AmqpUriSpecificationBuilder();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.Uri((string)null!));
        Assert.Throws<ArgumentNullException>(() => builder.Uri((Uri)null!));
    }
    
    [Fact]
    public void When_SetConnectionRetryCount()
    {
        // Arrange
        var builder = new AmqpUriSpecificationBuilder();
        const int retryCount = 5;

        // Act
        var result = builder.ConnectionRetryCount(retryCount);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Equal(retryCount, builder.Build().ConnectionRetryCount);
    }
    
    [Fact]
    public void When_SetConnectionRetryCount_Should_ThrowIfRetryIsLessThan0()
    {
        // Arrange
        var builder = new AmqpUriSpecificationBuilder();
        const int retryCount = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.ConnectionRetryCount(retryCount));
    }
    
    [Fact]
    public void When_SetRetryWaitWithTimeSpan()
    {
        // Arrange
        var builder = new AmqpUriSpecificationBuilder();
        var retryWait = TimeSpan.FromSeconds(2);
        const int expectedMilliseconds = 2000;

        // Act
        var result = builder.RetryWait(retryWait);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Equal(expectedMilliseconds, builder.Build().RetryWaitInMilliseconds);
    }

    [Fact]
    public void When_SetRetryWaitInMilliseconds()
    {
        // Arrange
        var builder = new AmqpUriSpecificationBuilder();
        const int retryWaitMilliseconds = 1500;

        // Act
        var result = builder.RetryWaitInMilliseconds(retryWaitMilliseconds);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Equal(retryWaitMilliseconds,builder.Build().RetryWaitInMilliseconds);
    }

    [Fact]
    public void When_SetNegativeValueRetryWaitInMilliseconds_Should_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new AmqpUriSpecificationBuilder();
        const int retryWaitMilliseconds = -500;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.RetryWaitInMilliseconds(retryWaitMilliseconds));
    }
    
    [Fact]
    public void When_SetCircuitBreakTimeWithTimeSpan()
    {
        // Arrange
        var builder = new AmqpUriSpecificationBuilder();
        var circuitBreakTime = TimeSpan.FromSeconds(2);
        const long expectedMilliseconds = 2_000;

        // Act
        var result = builder.CircuitBreakTime(circuitBreakTime);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Equal(expectedMilliseconds, builder.Build().CircuitBreakTimeInMilliseconds);
    }

    [Fact]
    public void When_SetCircuitBreakTimeInMilliseconds()
    {
        // Arrange
        var builder = new AmqpUriSpecificationBuilder();
        const int circuitBreakTimeMilliseconds = 75000;

        // Act
        var result = builder.CircuitBreakTimeInMilliseconds(circuitBreakTimeMilliseconds);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Equal(circuitBreakTimeMilliseconds, builder.Build().CircuitBreakTimeInMilliseconds);
    }

    [Fact]
    public void When_SetNegativeValueForCircuitBreakTimeInMilliseconds_Should_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new AmqpUriSpecificationBuilder();
        const int circuitBreakTimeMilliseconds = -1000;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.CircuitBreakTimeInMilliseconds(circuitBreakTimeMilliseconds));
    }

    [Fact]
    public void Build_ReturnsAmqpUriSpecificationWithCorrectValues()
    {
        // Arrange
        const string uriString = "amqp://test:test@localhost:5672/myvhost";
        const int retryCount = 4;
        const int retryWaitMilliseconds = 2500;
        const int circuitBreakTimeMilliseconds = 90000;

        var builder = new AmqpUriSpecificationBuilder()
            .Uri(uriString)
            .ConnectionRetryCount(retryCount)
            .RetryWaitInMilliseconds(retryWaitMilliseconds)
            .CircuitBreakTimeInMilliseconds(circuitBreakTimeMilliseconds);

        // Act
        var specification = builder.Build();

        // Assert
        Assert.NotNull(specification);
        Assert.Equal(new Uri(uriString), specification.Uri);
        Assert.Equal(retryCount, specification.ConnectionRetryCount);
        Assert.Equal(retryWaitMilliseconds, specification.RetryWaitInMilliseconds);
        Assert.Equal(circuitBreakTimeMilliseconds, specification.CircuitBreakTimeInMilliseconds);
    }
}
