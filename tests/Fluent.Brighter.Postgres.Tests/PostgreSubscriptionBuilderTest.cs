using Paramore.Brighter;

using Xunit;

namespace Fluent.Brighter.Postgres.Tests;

public class PostgresSubscriptionBuilderTests
{
    private class TestRequest : IRequest
    {
        public Id Id { get; set; } = Id.Random;
    }

    [Fact]
    public void MessageType_Sets_DataType_Correctly()
    {
        // Arrange
        var expectedType = typeof(TestRequest);

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType(expectedType)
            .Build();

        // Assert
        Assert.Equal(expectedType, subscription.DataType);
    }

    [Fact]
    public void MessageType_Generic_Sets_DataType_Correctly()
    {
        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .Build();

        // Assert
        Assert.Equal(typeof(TestRequest), subscription.DataType);
    }

    [Fact]
    public void SubscriptionName_Sets_SubscriptionName_Correctly()
    {
        // Arrange
        var expectedName = new SubscriptionName("MySubscription");

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .SubscriptionName(expectedName)
            .Build();

        // Assert
        Assert.Equal(expectedName, subscription.Name);
    }

    [Fact]
    public void ChannelName_Sets_ChannelName_Correctly()
    {
        // Arrange
        var expectedChannel = new ChannelName("MyChannel");

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .ChannelName(expectedChannel)
            .Build();

        // Assert
        Assert.Equal(expectedChannel, subscription.ChannelName);
    }

    [Fact]
    public void Queue_Sets_ChannelName_Correctly()
    {
        // Arrange
        var expectedChannel = new ChannelName("MyQueue");

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .Queue(expectedChannel)
            .Build();

        // Assert
        Assert.Equal(expectedChannel, subscription.ChannelName);
    }

    [Fact]
    public void RoutingKey_Sets_RoutingKey_Correctly()
    {
        // Arrange
        var expectedKey = new RoutingKey("MyRoutingKey");

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .RoutingKey(expectedKey)
            .Build();

        // Assert
        Assert.Equal(expectedKey, subscription.RoutingKey);
    }

    [Fact]
    public void Topic_Sets_RoutingKey_Correctly()
    {
        // Arrange
        var expectedKey = new RoutingKey("MyTopic");

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .Topic(expectedKey)
            .Build();

        // Assert
        Assert.Equal(expectedKey, subscription.RoutingKey);
    }

    [Fact]
    public void BufferSize_Sets_BufferSize_Correctly()
    {
        // Arrange
        int expectedSize = 5;

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .BufferSize(expectedSize)
            .Build();

        // Assert
        Assert.Equal(expectedSize, subscription.BufferSize);
    }

    [Fact]
    public void NoOfPerformers_Sets_NoOfPerformers_Correctly()
    {
        // Arrange
        int expectedCount = 4;

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .NoOfPerformers(expectedCount)
            .Build();

        // Assert
        Assert.Equal(expectedCount, subscription.NoOfPerformers);
    }

    [Fact]
    public void Default_NoOfPerformers_Is_ProcessorCount()
    {
        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .Build();

        // Assert
        Assert.Equal(Environment.ProcessorCount, subscription.NoOfPerformers);
    }

    [Fact]
    public void TimeOut_Sets_TimeOut_Correctly()
    {
        // Arrange
        var expectedTimeout = TimeSpan.FromSeconds(10);

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .TimeOut(expectedTimeout)
            .Build();

        // Assert
        Assert.Equal(expectedTimeout, subscription.TimeOut);
    }

    [Fact]
    public void RequeueCount_Sets_RequeueCount_Correctly()
    {
        // Arrange
        int expectedCount = 3;

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .RequeueCount(expectedCount)
            .Build();

        // Assert
        Assert.Equal(expectedCount, subscription.RequeueCount);
    }

    [Fact]
    public void RequeueDelay_Sets_RequeueDelay_Correctly()
    {
        // Arrange
        var expectedDelay = TimeSpan.FromSeconds(5);

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .RequeueDelay(expectedDelay)
            .Build();

        // Assert
        Assert.Equal(expectedDelay, subscription.RequeueDelay);
    }

    [Fact]
    public void UnacceptableMessageLimit_Sets_UnacceptableMessageLimit_Correctly()
    {
        // Arrange
        int expectedLimit = 5;

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .UnacceptableMessageLimit(expectedLimit)
            .Build();

        // Assert
        Assert.Equal(expectedLimit, subscription.UnacceptableMessageLimit);
    }

    [Fact]
    public void MessagePump_Sets_MessagePumpType_Correctly()
    {
        // Arrange
        var expectedType = MessagePumpType.Proactor;

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .MessagePump(expectedType)
            .Build();

        // Assert
        Assert.Equal(expectedType, subscription.MessagePumpType);
    }

    [Fact]
    public void AsProactor_Sets_MessagePumpType_To_Proactor()
    {
        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .AsProactor()
            .Build();

        // Assert
        Assert.Equal(MessagePumpType.Proactor, subscription.MessagePumpType);
    }

    [Fact]
    public void AsReactor_Sets_MessagePumpType_To_Reactor()
    {
        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .AsReactor()
            .Build();

        // Assert
        Assert.Equal(MessagePumpType.Reactor, subscription.MessagePumpType);
    }

    [Fact]
    public void ChannelFactory_Sets_ChannelFactory_Correctly()
    {
        // Arrange
        var mockChannelFactory = NSubstitute.Substitute.For<IAmAChannelFactory>();

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .ChannelFactory(mockChannelFactory)
            .Build();

        // Assert
        Assert.Equal(mockChannelFactory, subscription.ChannelFactory);
    }

    [Fact]
    public void MakeChannels_Sets_MakeChannels_Correctly()
    {
        // Arrange
        var expectedAction = OnMissingChannel.Validate;

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .MakeChannels(expectedAction)
            .Build();

        // Assert
        Assert.Equal(expectedAction, subscription.MakeChannels);
    }

    [Fact]
    public void CreateIfMissing_Sets_MakeChannels_To_Create()
    {
        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .CreateIfMissing()
            .Build();

        // Assert
        Assert.Equal(OnMissingChannel.Create, subscription.MakeChannels);
    }

    [Fact]
    public void Validate_Sets_MakeChannels_To_Validate()
    {
        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .Validate()
            .Build();

        // Assert
        Assert.Equal(OnMissingChannel.Validate, subscription.MakeChannels);
    }

    [Fact]
    public void Assume_Sets_MakeChannels_To_Assume()
    {
        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .Assume()
            .Build();

        // Assert
        Assert.Equal(OnMissingChannel.Assume, subscription.MakeChannels);
    }

    [Fact]
    public void EmptyDelay_Sets_EmptyChannelDelay_Correctly()
    {
        // Arrange
        var expectedDelay = TimeSpan.FromSeconds(2);

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .EmptyDelay(expectedDelay)
            .Build();

        // Assert
        Assert.Equal(expectedDelay, subscription.EmptyChannelDelay);
    }

    [Fact]
    public void FailureDelay_Sets_ChannelFailureDelay_Correctly()
    {
        // Arrange
        var expectedDelay = TimeSpan.FromSeconds(2);

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .FailureDelay(expectedDelay)
            .Build();

        // Assert
        Assert.Equal(expectedDelay, subscription.ChannelFailureDelay);
    }

    [Fact]
    public void SchemaName_Sets_SchemaName_Correctly()
    {
        // Arrange
        var expectedSchema = "MySchema";

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .SchemaName(expectedSchema)
            .Build();

        // Assert
        Assert.Equal(expectedSchema, subscription.SchemaName);
    }

    [Fact]
    public void QueueStoreTable_Sets_QueueStoreTable_Correctly()
    {
        // Arrange
        var expectedTable = "MyTable";

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .QueueStoreTable(expectedTable)
            .Build();

        // Assert
        Assert.Equal(expectedTable, subscription.QueueStoreTable);
    }

    [Fact]
    public void BinaryMessagePayload_Sets_BinaryMessagePayload_Correctly()
    {
        // Arrange
        bool? expected = true;

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .BinaryMessagePayload(expected)
            .Build();

        // Assert
        Assert.Equal(expected, subscription.BinaryMessagePayload);
    }

    [Fact]
    public void EnableBinaryMessagePayload_Sets_True()
    {
        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .EnableBinaryMessagePayload()
            .Build();

        // Assert
        Assert.True(subscription.BinaryMessagePayload == true);
    }

    [Fact]
    public void DisableBinaryMessagePayload_Sets_False()
    {
        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .DisableBinaryMessagePayload()
            .Build();

        // Assert
        Assert.True(subscription.BinaryMessagePayload == false);
    }

    [Fact]
    public void VisibleTimeout_Sets_VisibleTimeout_Correctly()
    {
        // Arrange
        var expected = TimeSpan.FromSeconds(10);

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .VisibleTimeout(expected)
            .Build();

        // Assert
        Assert.Equal(expected, subscription.VisibleTimeout);
    }

    [Fact]
    public void TableWithLargeMessage_Sets_TableWithLargeMessage_Correctly()
    {
        // Arrange
        bool expected = true;

        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .TableWithLargeMessage(expected)
            .Build();

        // Assert
        Assert.Equal(expected, subscription.TableWithLargeMessage);
    }

    [Fact]
    public void EnableTableWithLargeMessage_Sets_True()
    {
        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .EnableTableWithLargeMessage()
            .Build();

        // Assert
        Assert.True(subscription.TableWithLargeMessage);
    }

    [Fact]
    public void DisableTableWithLargeMessage_Sets_False()
    {
        // Act
        var subscription = new PostgresSubscriptionBuilder()
            .MessageType<TestRequest>()
            .DisableTableWithLargeMessage()
            .Build();

        // Assert
        Assert.False(subscription.TableWithLargeMessage);
    }

    [Fact]
    public void Build_Throws_Exception_When_MessageType_Not_Set()
    {
        // Act & Assert
        var ex = Assert.Throws<Paramore.Brighter.ConfigurationException>(() =>
            new PostgresSubscriptionBuilder().Build());

        Assert.Contains("Missing data type", ex.Message);
    }
}