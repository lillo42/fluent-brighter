using System;

using Paramore.Brighter.MessagingGateway.RMQ;

using Xunit;

namespace Fluent.Brighter.RMQ.Tests;

public class RmqConnectionBuilderTests
{
    [Fact]
    public void Name_ValidName_SetsNameCorrectly()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        const string connectionName = "test-connection";

        // Act
        var result = builder.Name(connectionName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Equal(connectionName, builder.Build().Name);
    }

    [Fact]
    public void Name_NullName_ThrowsArgumentException()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        string? connectionName = null;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.Name(connectionName!));
    }

    [Fact]
    public void Name_EmptyName_ThrowsArgumentException()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        const string connectionName = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.Name(connectionName));
    }

    [Fact]
    public void Name_DefaultValueIsMachineName()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();

        // Act & Assert
        Assert.Equal(Environment.MachineName, builder.Build().Name);
    }

    [Fact]
    public void AmqpUriSpecification_ConfigureAction_SetsAmqpUriSpecificationCorrectly()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        const string uriString = "amqp://user:pass@host:123/vhost";

        // Act
        var result = builder.AmqpUriSpecification(config => config.Uri(uriString));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        
        var ampq = builder.Build().AmpqUri;
        Assert.NotNull(ampq);
        Assert.Equal(new Uri(uriString), ampq.Uri);
    }

    [Fact]
    public void AmqpUriSpecification_ConfigureAction_ThrowsArgumentNullExceptionForNullAction()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        Action<AmqpUriSpecificationBuilder>? configureAction = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AmqpUriSpecification(configureAction!));
    }

    [Fact]
    public void AmqpUriSpecification_SpecificationInstance_SetsAmqpUriSpecificationCorrectly()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        var specification = new AmqpUriSpecification(new Uri("amqp://test"));

        // Act
        var result = builder.AmqpUriSpecification(specification);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Same(specification, builder.Build().AmpqUri);
    }

    [Fact]
    public void AmqpUriSpecification_SpecificationInstance_ThrowsArgumentNullExceptionForNullSpecification()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        AmqpUriSpecification? specification = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AmqpUriSpecification(specification!));
    }

    [Fact]
    public void Exchange_ConfigureAction_SetsExchangeCorrectly()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        const string exchangeName = "test.exchange";
        const string exchangeType = "direct";

        // Act
        var result = builder.Exchange(config => config.Name(exchangeName).Type(exchangeType));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        var exchange = builder.Build().Exchange;
        Assert.NotNull(exchange);
        Assert.Equal(exchangeName, exchange.Name);
        Assert.Equal(exchangeType, exchange.Type);
    }

    [Fact]
    public void Exchange_ConfigureAction_ThrowsArgumentNullExceptionForNullAction()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        Action<ExchangeBuilder>? configureAction = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.Exchange(configureAction!));
    }

    [Fact]
    public void Exchange_ExchangeInstance_SetsExchangeCorrectly()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        var exchange = new Exchange("test.exchange", "topic", true);

        // Act
        var result = builder.Exchange(exchange);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Same(exchange, builder.Build().Exchange);
    }

    [Fact]
    public void Exchange_ExchangeInstance_ThrowsArgumentNullExceptionForNullException()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        Exchange? exchange = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.Exchange(exchange!));
    }

    [Fact]
    public void DeadLetterExchange_ConfigureAction_SetsDeadLetterExchangeCorrectly()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        const string exchangeName = "dl.exchange";
        const string exchangeType = "fanout";

        // Act
        var result = builder.DeadLetterExchange(config => config.Name(exchangeName).Type(exchangeType));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        var deadLetter = builder.Build().DeadLetterExchange;
        Assert.NotNull(deadLetter);
        Assert.Equal(exchangeName, deadLetter.Name);
        Assert.Equal(exchangeType, deadLetter.Type);
    }

    [Fact]
    public void DeadLetterExchange_ConfigureAction_ThrowsArgumentNullExceptionForNullAction()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        Action<ExchangeBuilder>? configureAction = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.DeadLetterExchange(configureAction!));
    }

    [Fact]
    public void DeadLetterExchange_ExchangeInstance_SetsDeadLetterExchangeCorrectly()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        var exchange = new Exchange("dl.exchange", "fanout", false, true);

        // Act
        var result = builder.DeadLetterExchange(exchange);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Same(exchange, builder.Build().DeadLetterExchange);
    }

    [Fact]
    public void DeadLetterExchange_ExchangeInstance_ThrowsArgumentNullExceptionForNullException()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        Exchange? exchange = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.DeadLetterExchange(exchange!));
    }

    [Fact]
    public void HeartBeat_SetsHeartBeatCorrectly()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        const ushort heartbeatValue = 30;

        // Act
        var result = builder.HeartBeat(heartbeatValue);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Equal(heartbeatValue, builder.Build().Heartbeat);
    }

    [Fact]
    public void ContinuationTimeout_SetsContinuationTimeoutCorrectly()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();
        const ushort timeoutValue = 15;

        // Act
        var result = builder.ContinuationTimeout(timeoutValue);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.Equal(timeoutValue, builder.Build().ContinuationTimeout);
    }

    [Fact]
    public void EnablePersistMessage_SetsPersistMessagesToTrue()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();

        // Act
        var result = builder.EnablePersistMessage();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.True(builder.Build().PersistMessages);
    }

    [Fact]
    public void DisablePersistMessage_SetsPersistMessagesToFalse()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();

        // Act
        var result = builder.DisablePersistMessage();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.False(builder.Build().PersistMessages);
    }

    [Fact]
    public void PersistMessage_True_SetsPersistMessagesToTrue()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();

        // Act
        var result = builder.PersistMessage(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.True(builder.Build().PersistMessages);
    }

    [Fact]
    public void PersistMessage_False_SetsPersistMessagesToFalse()
    {
        // Arrange
        var builder = new RmqConnectionBuilder();

        // Act
        var result = builder.PersistMessage(false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder, result);
        Assert.False(builder.Build().PersistMessages);
    }

    [Fact]
    public void Build_WithAllPropertiesSet_ReturnsCorrectRmqMessagingGatewayConnectionInstance()
    {
        // Arrange
        const string connectionName = "full-config-connection";
        const string uriString = "amqp://user:pass@localhost:5672/testvhost";
        const string exchangeName = "primary.exchange";
        const string dlExchangeName = "deadletter.exchange";
        const ushort heartbeat = 25;
        const ushort timeout = 18;
        const bool persist = true;

        var builder = new RmqConnectionBuilder()
            .Name(connectionName)
            .AmqpUriSpecification(config => config.Uri(uriString))
            .Exchange(config => config.Name(exchangeName).Type("direct"))
            .DeadLetterExchange(config => config.Name(dlExchangeName).Type("fanout"))
            .HeartBeat(heartbeat)
            .ContinuationTimeout(timeout)
            .PersistMessage(persist);

        // Act
        var connection = builder.Build();

        // Assert
        Assert.NotNull(connection);
        Assert.Equal(connectionName, connection.Name);
        Assert.NotNull(connection.AmpqUri);
        Assert.Equal(new Uri(uriString), connection.AmpqUri.Uri);
        Assert.NotNull(connection.Exchange);
        Assert.Equal(exchangeName, connection.Exchange.Name);
        Assert.Equal("direct", connection.Exchange.Type);
        Assert.NotNull(connection.DeadLetterExchange);
        Assert.Equal(dlExchangeName, connection.DeadLetterExchange.Name);
        Assert.Equal("fanout", connection.DeadLetterExchange.Type);
        Assert.Equal(heartbeat, connection.Heartbeat);
        Assert.Equal(timeout, connection.ContinuationTimeout);
        Assert.True(connection.PersistMessages);
    }

    [Fact]
    public void Build_WithMinimalPropertiesSet_ReturnsRmqMessagingGatewayConnectionInstanceWithDefaults()
    {
        // Arrange
        const string connectionName = "minimal-config-connection";
        var builder = new RmqConnectionBuilder()
            .Name(connectionName);

        // Act
        var connection = builder.Build();

        // Assert
        Assert.NotNull(connection);
        Assert.Equal(connectionName, connection.Name);
        Assert.Null(connection.AmpqUri);
        Assert.Null(connection.Exchange);
        Assert.Null(connection.DeadLetterExchange);
        Assert.Equal((ushort)20, connection.Heartbeat);
        Assert.Equal((ushort)20, connection.ContinuationTimeout);
        Assert.False(connection.PersistMessages);
    }
}