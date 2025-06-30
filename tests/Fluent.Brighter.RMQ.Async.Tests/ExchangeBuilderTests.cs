using System;

using Xunit;

namespace Fluent.Brighter.RMQ.Async.Tests;

public class ExchangeBuilderTests
{
    [Fact]
    public void Name_ValidName_SetsNameCorrectly()
    {
        // Arrange
        var builder = new ExchangeBuilder();
        const string exchangeName = "my.exchange";

        // Act
        var result = builder.Name(exchangeName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Equal(exchangeName, builder.Build().Name);
    }

    [Fact]
    public void Name_NullName_ThrowsArgumentException()
    {
        // Arrange
        var builder = new ExchangeBuilder();
        string? exchangeName = null;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.Name(exchangeName!));
    }

    [Fact]
    public void Name_EmptyName_ThrowsArgumentException()
    {
        // Arrange
        var builder = new ExchangeBuilder();
        const string exchangeName = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.Name(exchangeName));
    }

    [Fact]
    public void Type_ValidType_SetsTypeCorrectly()
    {
        // Arrange
        var builder = new ExchangeBuilder();
        const string exchangeType = "fanout";

        // Act
        var result = builder.Type(exchangeType);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Equal(exchangeType, builder.Build().Type);
    }

    [Fact]
    public void Type_NullType_ThrowsArgumentException()
    {
        // Arrange
        var builder = new ExchangeBuilder();
        string? exchangeType = null;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.Type(exchangeType!));
    }

    [Fact]
    public void Type_EmptyType_ThrowsArgumentException()
    {
        // Arrange
        var builder = new ExchangeBuilder();
        const string exchangeType = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.Type(exchangeType));
    }

    [Fact]
    public void Type_DefaultValueIsDirect()
    {
        // Arrange
        var builder = new ExchangeBuilder();

        // Act & Assert
        Assert.Equal("direct", builder.Build().Type);
    }

    [Fact]
    public void EnableDurable_SetsDurableToTrue()
    {
        // Arrange
        var builder = new ExchangeBuilder();

        // Act
        var result = builder.EnableDurable();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.True(builder.Build().Durable);
    }

    [Fact]
    public void DisableDurable_SetsDurableToFalse()
    {
        // Arrange
        var builder = new ExchangeBuilder();

        // Act
        var result = builder.DisableDurable();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ExchangeBuilder>(result); // Check for fluent chaining
        Assert.Equal(builder, result);
        Assert.False(builder.Build().Durable);
    }

    [Fact]
    public void Durable_True_SetsDurableToTrue()
    {
        // Arrange
        var builder = new ExchangeBuilder();

        // Act
        var result = builder.Durable(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.True(builder.Build().Durable);
    }

    [Fact]
    public void Durable_False_SetsDurableToFalse()
    {
        // Arrange
        var builder = new ExchangeBuilder();

        // Act
        var result = builder.Durable(false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.False(builder.Build().Durable);
    }

    [Fact]
    public void EnableSupportDelay_SetsSupportDelayToTrue()
    {
        // Arrange
        var builder = new ExchangeBuilder();

        // Act
        var result = builder.EnableSupportDelay();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ExchangeBuilder>(result); // Check for fluent chaining
        Assert.Equal(builder, result);
        Assert.True(builder.Build().SupportDelay);
    }

    [Fact]
    public void DisableSupportDelay_SetsSupportDelayToFalse()
    {
        // Arrange
        var builder = new ExchangeBuilder();

        // Act
        var result = builder.DisableSupportDelay();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.False(builder.Build().SupportDelay);
    }

    [Fact]
    public void SupportDelay_True_SetsSupportDelayToTrue()
    {
        // Arrange
        var builder = new ExchangeBuilder();

        // Act
        var result = builder.SupportDelay(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.True(builder.Build().SupportDelay);
    }

    [Fact]
    public void SupportDelay_False_SetsSupportDelayToFalse()
    {
        // Arrange
        var builder = new ExchangeBuilder();

        // Act
        var result = builder.SupportDelay(false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.False(builder.Build().SupportDelay);
    }

    [Fact]
    public void Build_WithAllPropertiesSet_ReturnsCorrectExchangeInstance()
    {
        // Arrange
        const string exchangeName = "test.exchange";
        const string exchangeType = "topic";
        const bool durable = true;
        const bool supportDelay = true;

        var builder = new ExchangeBuilder()
            .Name(exchangeName)
            .Type(exchangeType)
            .Durable(durable)
            .SupportDelay(supportDelay);

        // Act
        var exchange = builder.Build();

        // Assert
        Assert.NotNull(exchange);
        Assert.Equal(exchangeName, exchange.Name);
        Assert.Equal(exchangeType, exchange.Type);
        Assert.Equal(durable, exchange.Durable);
        Assert.Equal(supportDelay, exchange.SupportDelay);
    }

    [Fact]
    public void Build_WithDefaultProperties_ReturnsExchangeInstanceWithDefaults()
    {
        // Arrange
        const string exchangeName = "default.exchange";
        var builder = new ExchangeBuilder()
            .Name(exchangeName);

        // Act
        var exchange = builder.Build();

        // Assert
        Assert.NotNull(exchange);
        Assert.Equal(exchangeName, exchange.Name);
        Assert.Equal("direct", exchange.Type);
        Assert.False(exchange.Durable);
        Assert.False(exchange.SupportDelay);
    }
}