using System;
using System.Collections.Generic;

using Paramore.Brighter;

using Xunit;

namespace Fluent.Brighter.RMQ.Tests;

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
        var channelMode = OnMissingChannel.Validate;

        // Act
        var result = builder.MakeExchange(channelMode);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(channelMode, builder.Build().MakeChannels);
    }

    [Fact]
    public void MaxOutStandingMessages_ValidValue_SetsMaxOutStandingMessagesCorrectly()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        const int maxMessages = 100;

        // Act
        var result = builder.MaxOutStandingMessages(maxMessages);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(maxMessages, builder.Build().MaxOutStandingMessages);
    }

    [Fact]
    public void MaxOutStandingMessages_DefaultValue_SetsMaxOutStandingMessagesToNegativeOne()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        const int maxMessages = -1;

        // Act
        var result = builder.MaxOutStandingMessages(maxMessages);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(maxMessages, builder.Build().MaxOutStandingMessages);
    }

    [Fact]
    public void MaxOutStandingMessages_NegativeValueLessThanMinusOne_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        const int maxMessages = -2;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.MaxOutStandingMessages(maxMessages));
    }

    [Fact]
    public void MaxOutStandingCheckInterval_TimeSpan_SetsIntervalInMillisecondsCorrectly()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        var interval = TimeSpan.FromMilliseconds(750);
        const int expectedMilliseconds = 750;

        // Act
        var result = builder.MaxOutStandingCheckInterval(interval);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(expectedMilliseconds, builder.Build().MaxOutStandingCheckIntervalMilliSeconds);
    }

    [Fact]
    public void MaxOutStandingCheckIntervalMilliSeconds_ValidValue_SetsIntervalCorrectly()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        const int intervalMilliseconds = 1200;

        // Act
        var result = builder.MaxOutStandingCheckIntervalMilliSeconds(intervalMilliseconds);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(intervalMilliseconds, builder.Build().MaxOutStandingCheckIntervalMilliSeconds);
    }

    [Fact]
    public void MaxOutStandingCheckIntervalMilliSeconds_NegativeValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        const int intervalMilliseconds = -50;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.MaxOutStandingCheckIntervalMilliSeconds(intervalMilliseconds));
    }

    [Fact]
    public void OutboxBag_SingleKeyValuePair_AddsToOutboxBag()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        var value = Guid.NewGuid();
        const string key = "correlationId";

        // Act
        var result = builder.OutboxBag(key, value);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        var outboxBag = builder.Build().OutBoxBag;
        Assert.NotNull(outboxBag);
        Assert.Single(outboxBag);
        Assert.True(outboxBag.ContainsKey(key));
        Assert.Equal(value, outboxBag[key]);
    }

    [Fact]
    public void OutboxBag_SingleKeyValuePair_NullKey_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        string? key = null;
        const string value = "test";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.OutboxBag(key!, value));
    }

    [Fact]
    public void OutboxBag_MultipleKeyValuePairs_AddsAllToOutboxBag()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        var bag = new Dictionary<string, object>
        {
            {"messageType", "orderCreated"},
            {"userId", 123}
        };

        // Act
        var result = builder.OutboxBag(bag);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        var outboxBag = builder.Build().OutBoxBag; 
        Assert.NotNull(outboxBag);
        Assert.Equal(2, outboxBag.Count);
        Assert.True(outboxBag.ContainsKey("messageType"));
        Assert.Equal("orderCreated", outboxBag["messageType"]);
        Assert.True(outboxBag.ContainsKey("userId"));
        Assert.Equal(123, outboxBag["userId"]);
    }

    [Fact]
    public void OutboxBag_MultipleKeyValuePairs_NullBag_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new RmqPublicationBuilder();
        IEnumerable<KeyValuePair<string, object>>? bag = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.OutboxBag(bag!));
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
        const int maxOutstandingMessages = 50;
        const int maxOutstandingCheckInterval = 200;
        var outboxBag = new Dictionary<string, object> { { "key1", "value1" } };
        var topic = new RoutingKey("test.topic");

        var builder = new RmqPublicationBuilder()
            .WaitForConfirmsTimeOutInMilliseconds(waitForConfirmsTimeout)
            .MakeExchange(makeChannel)
            .MaxOutStandingMessages(maxOutstandingMessages)
            .MaxOutStandingCheckIntervalMilliSeconds(maxOutstandingCheckInterval)
            .OutboxBag(outboxBag)
            .Topic(topic);

        // Act
        var publication = builder.Build();

        // Assert
        Assert.NotNull(publication);
        Assert.Equal(waitForConfirmsTimeout, publication.WaitForConfirmsTimeOutInMilliseconds);
        Assert.Equal(makeChannel, publication.MakeChannels);
        Assert.Equal(maxOutstandingMessages, publication.MaxOutStandingMessages);
        Assert.Equal(maxOutstandingCheckInterval, publication.MaxOutStandingCheckIntervalMilliSeconds);
        Assert.NotNull(publication.OutBoxBag);
        Assert.Single(publication.OutBoxBag);
        Assert.True(publication.OutBoxBag.ContainsKey("key1"));
        Assert.Equal("value1", publication.OutBoxBag["key1"]);
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
        Assert.Equal(-1, publication.MaxOutStandingMessages);
        Assert.Equal(0, publication.MaxOutStandingCheckIntervalMilliSeconds);
        Assert.Null(publication.OutBoxBag);
        Assert.Null(publication.Topic);
    }
}