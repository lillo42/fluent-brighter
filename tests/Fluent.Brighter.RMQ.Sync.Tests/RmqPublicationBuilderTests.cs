using System;

using Paramore.Brighter;

using Xunit;

namespace Fluent.Brighter.RMQ.Sync.Tests;

public class RmqPublicationBuilderTests
{
    [Fact]
    public void WaitForConfirmsTimeOut_TimeSpan_SetsTimeoutInMillisecondsCorrectly()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        var timeout = TimeSpan.FromSeconds(2);
        const int expectedMilliseconds = 2000;

        // Act
        var result = builder.WaitForConfirmsTimeOut(timeout);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(expectedMilliseconds, builder.Build().WaitForConfirmsTimeOutInMilliseconds);
    }

    [Fact]
    public void WaitForConfirmsTimeOutInMilliseconds_ValidValue_SetsTimeoutCorrectly()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        const int timeoutMilliseconds = 1500;

        // Act
        var result = builder.WaitForConfirmsTimeOutInMilliseconds(timeoutMilliseconds);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(timeoutMilliseconds, builder.Build().WaitForConfirmsTimeOutInMilliseconds);
    }

    [Fact]
    public void WaitForConfirmsTimeOutInMilliseconds_NegativeValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        const int timeoutMilliseconds = -100;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.WaitForConfirmsTimeOutInMilliseconds(timeoutMilliseconds));
    }

    [Fact]
    public void CreateExchangeIfMissing_SetsMakeChannelToCreate()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();

        // Act
        var result = builder.CreateExchangeIfMissing();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(OnMissingChannel.Create, builder.Build().MakeChannels);
    }

    [Fact]
    public void ValidateIfExchangeExists_SetsMakeChannelToValidate()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();

        // Act
        var result = builder.ValidateIfExchangeExists();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(OnMissingChannel.Validate, builder.Build().MakeChannels);
    }

    [Fact]
    public void AssumeExchangeExists_SetsMakeChannelToAssume()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();

        // Act
        var result = builder.AssumeExchangeExists();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(OnMissingChannel.Assume, builder.Build().MakeChannels);
    }

    [Fact]
    public void MakeExchange_SetsMakeChannelCorrectly()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        const OnMissingChannel channelMode = OnMissingChannel.Validate;

        // Act
        var result = builder.MakeExchange(channelMode);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(channelMode, builder.Build().MakeChannels);
    }

    [Fact]
    public void Topic_String_SetsTopicCorrectly()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        const string topicString = "my.routing.key";

        // Act
        var result = builder.Topic(topicString);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        var topic = builder.Build().Topic;
        Assert.NotNull(topic);
        Assert.Equal(topicString, topic.Value);
    }

    [Fact]
    public void Topic_RoutingKey_SetsTopicCorrectly()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        var routingKey = new RoutingKey("another.key");

        // Act
        var result = builder.Topic(routingKey);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Same(routingKey, builder.Build().Topic);
    }

    [Fact]
    public void Build_WithAllPropertiesSet_ReturnsCorrectRmqPublicationInstance()
    {
        // Arrange
        const int waitForConfirmsTimeout = 1000;
        const OnMissingChannel makeChannel = OnMissingChannel.Validate;
        var topic = new RoutingKey("test.topic");

        var builder = new RmqPublicationBuilder()
            .WaitForConfirmsTimeOutInMilliseconds(waitForConfirmsTimeout)
            .MakeExchange(makeChannel)
            .Topic(topic);

        // Act
        var publication = builder.Build();

        // Assert
        Assert.NotNull(publication);
        Assert.Equal(waitForConfirmsTimeout, publication.WaitForConfirmsTimeOutInMilliseconds);
        Assert.Equal(makeChannel, publication.MakeChannels);
        Assert.Same(topic, publication.Topic);
    }

    [Fact]
    public void Build_WithDefaultProperties_ReturnsRmqPublicationInstanceWithDefaults()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();

        // Act
        var publication = builder.Build();

        // Assert
        Assert.NotNull(publication);
        Assert.Equal(500, publication.WaitForConfirmsTimeOutInMilliseconds);
        Assert.Equal(OnMissingChannel.Create, publication.MakeChannels);
        Assert.Null(publication.Topic);
    }
}