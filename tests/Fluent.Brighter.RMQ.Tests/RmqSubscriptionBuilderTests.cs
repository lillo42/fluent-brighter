using System;

using NSubstitute;

using Paramore.Brighter;

using Xunit;

namespace Fluent.Brighter.RMQ.Tests;

public class RmqSubscriptionBuilderTests
{
    [Fact]
    public void DeadLetter_String_SetsDeadLetterNameAndRoutingKey()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType(typeof(object));
        const string dlqName = "dead.letter.queue";

        // Act
        var result = builder.DeadLetter(dlqName);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RmqSubscriptionBuilder>(result);
        Assert.Same(builder, result);
        var sub = builder.Build();
        Assert.Equal(dlqName, sub.DeadLetterChannelName.Value);
        Assert.Equal(dlqName, sub.DeadLetterRoutingKey);
    }

    [Fact]
    public void DeadLetter_String_ThrowsArgumentExceptionForNullOrEmpty()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder();
    
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.DeadLetter(null!));
        Assert.Throws<ArgumentException>(() => builder.DeadLetter(string.Empty));
    }
    
    [Fact]
    public void DeadLetter_ChannelName_SetsDeadLetterNameAndRoutingKey()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType(typeof(object));
        var dlqName = new ChannelName("dead.letter.queue");
    
        // Act
        var result = builder
            .DeadLetter(dlqName);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        
        var sub = builder.Build();
        Assert.Equal(dlqName, sub.DeadLetterChannelName);
        Assert.Equal(dlqName.Value, sub.DeadLetterRoutingKey);
    }
    
    [Fact]
    public void DeadLetter_ChannelName_DoesNotOverrideExistingRoutingKey()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder()
            .MessageType(typeof(object))
            .DeadLetterRoutingKey("initial.routing.key");
        var dlqName = new ChannelName("dead.letter.queue");
    
        // Act
        var result = builder.DeadLetter(dlqName);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        var sub = builder.Build();
        Assert.Equal(dlqName, sub.DeadLetterChannelName);
        Assert.Equal("initial.routing.key", sub.DeadLetterRoutingKey);
    }
    
    [Fact]
    public void DeadLetterRoutingKey_ValidRoutingKey_SetsDeadLetterRoutingKey()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType(typeof(object));
        const string routingKey = "dlq.routing.key";
    
        // Act
        var result = builder.DeadLetterRoutingKey(routingKey);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(routingKey, builder.Build().DeadLetterRoutingKey);
    }
    
    [Fact]
    public void DeadLetterRoutingKey_ThrowsArgumentExceptionForNullOrEmpty()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType(typeof(object));
    
        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.DeadLetterRoutingKey(null!));
        Assert.Throws<ArgumentException>(() => builder.DeadLetterRoutingKey(string.Empty));
    }
    
    [Fact]
    public void EnableHighAvailability_SetsHighAvailabilityToTrue()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType(typeof(object));
    
        // Act
        var result = builder.EnableHighAvailability();
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.True(builder.Build().HighAvailability);
    }
    
    [Fact]
    public void DisableHighAvailability_SetsHighAvailabilityToFalse()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType(typeof(object));
    
        // Act
        var result = builder.DisableHighAvailability();
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.False(builder.Build().HighAvailability);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void HighAvailability_SetsHighAvailabilityCorrectly(bool highAvailability)
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType(typeof(object));
    
        // Act
        var result = builder.HighAvailability(highAvailability);
    
        // Assert
        Assert.Same(builder, result);
        Assert.Equal(highAvailability, result.Build().HighAvailability);
    }
    
    [Fact]
    public void EnableDurable_SetsDurableToTrue()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType(typeof(object));
    
        // Act
        var result = builder.EnableDurable();
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.True(builder.Build().IsDurable);
    }
    
    [Fact]
    public void DisableDurable_SetsDurableToFalse()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType(typeof(object));
    
        // Act
        var result = builder.DisableDurable();
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.False(builder.Build().IsDurable);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Durable_SetsDurableCorrectly(bool durable)
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType(typeof(object));
    
        // Act
        var result = builder.Durable(durable);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(durable, builder.Build().IsDurable);
    }
    
    // [Fact]
    // public void MaxQueueLenght_ValidValue_SetsMaxQueueLenght()
    // {
    //     // Arrange
    //     var builder = new RmqSubscriptionBuilder();
    //     int? maxLength = 100;
    //
    //     // Act
    //     var result = builder.MaxQueueLenght(maxLength);
    //
    //     // Assert
    //     Assert.NotNull(result);
    //     Assert.IsType<RmqSubscriptionBuilder>(result);
    //     Assert.Equal(maxLength, GetInternalMaxQueueLenght(builder));
    // }
    //
    // [Fact]
    // public void MaxQueueLenght_NullValue_SetsMaxQueueLenghtToNull()
    // {
    //     // Arrange
    //     var builder = new RmqSubscriptionBuilder();
    //     int? maxLength = null;
    //
    //     // Act
    //     var result = builder.MaxQueueLenght(maxLength);
    //
    //     // Assert
    //     Assert.NotNull(result);
    //     Assert.IsType<RmqSubscriptionBuilder>(result);
    //     Assert.Null(GetInternalMaxQueueLenght(builder));
    // }
    
    [Fact]
    public void Ttl_ValidValue_SetsTtl()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType(typeof(object));
        int? ttlValue = 5000;
    
        // Act
        var result = builder.Ttl(ttlValue);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(ttlValue, builder.Build().TTL);
    }
    
    [Fact]
    public void Ttl_NullValue_SetsTtlToNull()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType(typeof(object));
        int? ttlValue = null;
    
        // Act
        var result = builder.Ttl(ttlValue);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Null(builder.Build().TTL);
    }
    
    [Fact]
    public void MessageType_Generic_SetsDataTypeAndChannelAndTopicName()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder();
    
        // Act
        var result = builder.MessageType<ExampleCommand>();
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        var sub = builder.Build();
        Assert.Equal(typeof(ExampleCommand), sub.DataType);
        Assert.Equal(typeof(ExampleCommand).FullName, sub.ChannelName.Value);
        Assert.Equal(typeof(ExampleCommand), sub.DataType);
    }
    
    [Fact]
    public void MessageType_Type_ThrowsArgumentNullExceptionForNullType()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder();
        Type? dataType = null;
    
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.MessageType(dataType!));
    }
    
    [Fact]
    public void SubscriptionName_String_SetsSubscriptionName()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const string subName = "my.subscription";
    
        // Act
        var result = builder.SubscriptionName(subName);
    
        // Assert
        Assert.NotNull(result);
        Assert.IsType<RmqSubscriptionBuilder>(result);
        Assert.Equal(subName, builder.Build().Name.Value);
    }
    
    [Fact]
    public void SubscriptionName_String_ThrowsArgumentExceptionForNullOrEmpty()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
    
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.SubscriptionName(null!));
        Assert.Throws<ArgumentException>(() => builder.SubscriptionName(string.Empty));
    }
    
    [Fact]
    public void SubscriptionName_SubscriptionNameObject_SetsSubscriptionName()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        var subName = new SubscriptionName("my.subscription");
    
        // Act
        var result = builder.SubscriptionName(subName);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(subName, builder.Build().Name);
    }
    
    [Fact]
    public void SubscriptionName_SubscriptionNameObject_ThrowsArgumentNullExceptionForNull()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        SubscriptionName? subName = null;
    
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.SubscriptionName(subName!));
    }
    
    [Fact]
    public void QueueName_SetsChannelName()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const string queueName = "my.queue";
    
        // Act
        var result = builder.QueueName(queueName);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(queueName, builder.Build().ChannelName.Value);
    }
    
    [Fact]
    public void ChannelName_String_SetsChannelName()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const string channelName = "my.channel";
    
        // Act
        var result = builder.ChannelName(channelName);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(channelName, builder.Build().ChannelName.Value);
    }
    
    [Fact]
    public void ChannelName_String_ThrowsArgumentExceptionForNullOrEmpty()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
    
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.ChannelName(null!));
        Assert.Throws<ArgumentException>(() => builder.ChannelName(string.Empty));
    }
    
    [Fact]
    public void ChannelName_ChannelNameObject_SetsChannelName()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        var channelName = new ChannelName("my.channel");
    
        // Act
        var result = builder.ChannelName(channelName);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(channelName, builder.Build().ChannelName);
    }
    
    [Fact]
    public void ChannelName_ChannelNameObject_ThrowsArgumentNullExceptionForNull()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder();
        ChannelName? channelName = null;
    
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.ChannelName(channelName!));
    }
    
    [Fact]
    public void TopicName_SetsRoutingKey()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const string topicName = "my.topic";
    
        // Act
        var result = builder.TopicName(topicName);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(topicName, builder.Build().RoutingKey.Value);
    }
    
    [Fact]
    public void RoutingKey_String_SetsRoutingKey()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const string routingKey = "my.routing.key";
    
        // Act
        var result = builder.RoutingKey(routingKey);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(routingKey, builder.Build().RoutingKey.Value);
    }
    
    [Fact]
    public void RoutingKey_String_ThrowsArgumentExceptionForNullOrEmpty()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
    
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.RoutingKey(null!));
        Assert.Throws<ArgumentException>(() => builder.RoutingKey(string.Empty));
    }
    
    [Fact]
    public void RoutingKey_RoutingKeyObject_SetsRoutingKey()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        var routingKey = new RoutingKey("my.routing.key");
    
        // Act
        var result = builder.RoutingKey(routingKey);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(routingKey, builder.Build().RoutingKey);
    }
    
    [Fact]
    public void RoutingKey_RoutingKeyObject_ThrowsArgumentNullExceptionForNull()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        RoutingKey? routingKey = null;
    
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.RoutingKey(routingKey!));
    }
    
    [Fact]
    public void BufferSize_ValidValue_SetsBufferSize()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const int bufferSize = 5;
    
        // Act
        var result = builder.BufferSize(bufferSize);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(bufferSize,  builder.Build().BufferSize);
    }
    
    [Fact]
    public void BufferSize_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder();
        const int bufferSize = 0;
    
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.BufferSize(bufferSize));
    }
    
    [Fact]
    public void Concurrency_ValidValue_SetsNoOfPerformers()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const int concurrency = 3;
    
        // Act
        var result = builder.Concurrency(concurrency);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(concurrency, builder.Build().NoOfPeformers);
    }
    
    [Fact]
    public void Concurrency_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder();
        const int concurrency = 0;
    
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.Concurrency(concurrency));
    }
    
    [Fact]
    public void Timeout_TimeSpan_SetsTimeoutInMilliseconds()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        var timeout = TimeSpan.FromMilliseconds(500);
        const int expectedMilliseconds = 500;
    
        // Act
        var result = builder.Timeout(timeout);
    
        // Assert
        Assert.NotNull(result);
        Assert.IsType<RmqSubscriptionBuilder>(result);
        Assert.Equal(expectedMilliseconds, builder.Build().TimeoutInMilliseconds);
    }
    
    [Fact]
    public void TimeoutInMilliseconds_ValidValue_SetsTimeoutInMilliseconds()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const int timeoutMilliseconds = 1000;
    
        // Act
        var result = builder.TimeoutInMilliseconds(timeoutMilliseconds);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(timeoutMilliseconds, builder.Build().TimeoutInMilliseconds);
    }
    
    [Fact]
    public void TimeoutInMilliseconds_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder();
        const int timeoutMilliseconds = -100;
    
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.TimeoutInMilliseconds(timeoutMilliseconds));
    }
    
    [Fact]
    public void RequeueCount_ValidValue_SetsRequeueCount()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const int requeueCount = 5;
    
        // Act
        var result = builder.RequeueCount(requeueCount);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(requeueCount, builder.Build().RequeueCount);
    }
    
    [Fact]
    public void RequeueCount_UnlimitedValue_SetsRequeueCountToNegativeOne()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const int requeueCount = -1;
    
        // Act
        var result = builder.RequeueCount(requeueCount);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(requeueCount, builder.Build().RequeueCount);
    }
    
    [Fact]
    public void RequeueCount_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder();
        const int requeueCount = -2;
    
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.RequeueCount(requeueCount));
    }
    
    [Fact]
    public void RequeueDelay_TimeSpan_SetsRequeueDelayInMilliseconds()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        var requeueDelay = TimeSpan.FromMilliseconds(2000);
        const int expectedMilliseconds = 2000;
    
        // Act
        var result = builder.RequeueDelay(requeueDelay);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(expectedMilliseconds,  builder.Build().RequeueDelayInMilliseconds);
    }
    
    [Fact]
    public void RequeueDelayInMilliseconds_ValidValue_SetsRequeueDelayInMilliseconds()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const int requeueDelayMilliseconds = 1500;
    
        // Act
        var result = builder.RequeueDelayInMilliseconds(requeueDelayMilliseconds);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(requeueDelayMilliseconds, builder.Build().RequeueDelayInMilliseconds);
    }
    
    [Fact]
    public void RequeueDelayInMilliseconds_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const int requeueDelayMilliseconds = -100;
    
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.RequeueDelayInMilliseconds(requeueDelayMilliseconds));
    }
    
    [Fact]
    public void UnacceptableMessageLimit_ValidValue_SetsUnacceptableMessageLimit()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const int limit = 10;
    
        // Act
        var result = builder.UnacceptableMessageLimit(limit);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(limit, builder.Build().UnacceptableMessageLimit);
    }
    
    [Fact]
    public void UnacceptableMessageLimit_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder();
        const int limit = -1;
    
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.UnacceptableMessageLimit(limit));
    }
    
    [Fact]
    public void MessagePumpReactor_SetsRunAsyncToFalse()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
    
        // Act
        var result = builder.MessagePumpReactor();
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.False(builder.Build().RunAsync);
    }
    
    [Fact]
    public void MessagePumpProactor_SetsRunAsyncToTrue()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
    
        // Act
        var result = builder.MessagePumpProactor();
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.True(builder.Build().RunAsync);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MessagePump_SetsRunAsyncCorrectly(bool runAsync)
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
    
        // Act
        var result = builder.MessagePump(runAsync);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(runAsync, builder.Build().RunAsync);
    }
    
    [Fact]
    public void ChannelFactory_ValidValue_SetsChannelFactory()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        var factory = Substitute.For<IAmAChannelFactory>();
    
        // Act
        var result = builder.ChannelFactory(factory);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Same(factory, builder.Build().ChannelFactory);
    }
    
    [Fact]
    public void ChannelFactory_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        IAmAChannelFactory? factory = null;
    
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.ChannelFactory(factory!));
    }
    
    [Fact]
    public void EmptyDelay_TimeSpan_SetsEmptyChannelDelayInMilliseconds()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        var delay = TimeSpan.FromMilliseconds(750);
        const int expectedMilliseconds = 750;
    
        // Act
        var result = builder.EmptyDelay(delay);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(expectedMilliseconds, builder.Build().EmptyChannelDelay);
    }
    
    [Fact]
    public void EmptyChannelDelayInMilliseconds_ValidValue_SetsEmptyChannelDelayInMilliseconds()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const int delayMilliseconds = 1200;
    
        // Act
        var result = builder.EmptyChannelDelayInMilliseconds(delayMilliseconds);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(delayMilliseconds, builder.Build().EmptyChannelDelay);
    }
    
    [Fact]
    public void EmptyChannelDelayInMilliseconds_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder();
        const int delayMilliseconds = -50;
    
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.EmptyChannelDelayInMilliseconds(delayMilliseconds));
    }
    
    [Fact]
    public void FailureDelay_TimeSpan_SetsChannelFailureDelayInMilliseconds()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        var delay = TimeSpan.FromMilliseconds(1500);
        const int expectedMilliseconds = 1500;
    
        // Act
        var result = builder.FailureDelay(delay);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(expectedMilliseconds, builder.Build().ChannelFailureDelay);
    }
    
    [Fact]
    public void FailureDelayInMilliseconds_ValidValue_SetsChannelFailureDelayInMilliseconds()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
        const int delayMilliseconds = 2500;
    
        // Act
        var result = builder.FailureDelayInMilliseconds(delayMilliseconds);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(delayMilliseconds, builder.Build().ChannelFailureDelay);
    }
    
    [Fact]
    public void FailureDelayInMilliseconds_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder();
        const int delayMilliseconds = -200;
    
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.FailureDelayInMilliseconds(delayMilliseconds));
    }
    
    [Fact]
    public void CreateOrOverrideTopicOrQueueIfMissing_SetsMakeChannelToCreate()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
    
        // Act
        var result = builder.CreateOrOverrideTopicOrQueueIfMissing();
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(OnMissingChannel.Create, builder.Build().MakeChannels);
    }
    
    [Fact]
    public void ValidateIfTopicAndQueueExists_SetsMakeChannelToValidate()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
    
        // Act
        var result = builder.ValidateIfTopicAndQueueExists();
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(OnMissingChannel.Validate, builder.Build().MakeChannels);
    }
    
    [Fact]
    public void AssumeTopicAndQueueExists_SetsMakeChannelToAssume()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
    
        // Act
        var result = builder.AssumeTopicAndQueueExists();
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(OnMissingChannel.Assume,builder.Build().MakeChannels);
    }
    
    [Theory]
    [InlineData(OnMissingChannel.Validate)]
    [InlineData(OnMissingChannel.Assume)]
    [InlineData(OnMissingChannel.Create)]
    public void MakeTopicOrQueue_SetsMakeChannelCorrectly(OnMissingChannel makeChannels)
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder().MessageType<ExampleCommand>();
    
        // Act
        var result = builder.MakeTopicOrQueue(makeChannels);
    
        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        Assert.Equal(makeChannels, builder.Build().MakeChannels);
    }
    
    [Fact]
    public void Build_WithAllPropertiesSet_ReturnsCorrectRmqSubscriptionInstance()
    {
        // Arrange
        var dlqName = new ChannelName("dead.letter.queue");
        const string dlqRoutingKey = "dlq.routing.key";
        const bool highAvailability = true;
        const bool durable = true;
        int? ttl = 60000;
        var dataType = typeof(ExampleCommand);
        var subName = new SubscriptionName("test.subscription");
        var channelName = new ChannelName("test.queue");
        var routingKey = new RoutingKey("test.topic");
        const int bufferSize = 5;
        const int concurrency = 3;
        const int timeout = 1000;
        const int requeueCount = 2;
        const int requeueDelay = 500;
        const int unacceptableLimit = 10;
        const bool runAsync = true;
        var channelFactory = Substitute.For<IAmAChannelFactory>();
        const OnMissingChannel makeChannel = OnMissingChannel.Create;
        const int emptyDelay = 250;
        const int failureDelay = 750;
    
        var builder = new RmqSubscriptionBuilder()
            .DeadLetter(dlqName)
            .DeadLetterRoutingKey(dlqRoutingKey)
            .HighAvailability(highAvailability)
            .Durable(durable)
            .Ttl(ttl)
            .MessageType(dataType)
            .SubscriptionName(subName)
            .ChannelName(channelName)
            .RoutingKey(routingKey)
            .BufferSize(bufferSize)
            .Concurrency(concurrency)
            .TimeoutInMilliseconds(timeout)
            .RequeueCount(requeueCount)
            .RequeueDelayInMilliseconds(requeueDelay)
            .UnacceptableMessageLimit(unacceptableLimit)
            .MessagePump(runAsync)
            .ChannelFactory(channelFactory)
            .MakeTopicOrQueue(makeChannel)
            .EmptyChannelDelayInMilliseconds(emptyDelay)
            .FailureDelayInMilliseconds(failureDelay);
    
        // Act
        var subscription = builder.Build();
    
        // Assert
        Assert.NotNull(subscription);
        Assert.Equal(dlqName, subscription.DeadLetterChannelName);
        Assert.Equal(dlqRoutingKey, subscription.DeadLetterRoutingKey);
        Assert.True(subscription.HighAvailability);
        Assert.True(subscription.IsDurable);
        Assert.Equal(ttl, subscription.TTL);
        Assert.Equal(dataType, subscription.DataType);
        Assert.Equal(subName, subscription.Name);
        Assert.Equal(channelName, subscription.ChannelName);
        Assert.Equal(routingKey, subscription.RoutingKey);
        Assert.Equal(bufferSize, subscription.BufferSize);
        Assert.Equal(concurrency, subscription.NoOfPeformers);
        Assert.Equal(timeout, subscription.TimeoutInMilliseconds);
        Assert.Equal(requeueCount, subscription.RequeueCount);
        Assert.Equal(requeueDelay, subscription.RequeueDelayInMilliseconds);
        Assert.Equal(unacceptableLimit, subscription.UnacceptableMessageLimit);
        Assert.True(subscription.RunAsync);
        Assert.Same(channelFactory, subscription.ChannelFactory);
        Assert.Equal(makeChannel, subscription.MakeChannels);
        Assert.Equal(emptyDelay, subscription.EmptyChannelDelay);
        Assert.Equal(failureDelay, subscription.ChannelFailureDelay);
    }
    
    [Fact]
    public void Build_WithMinimalPropertiesSet_ReturnsRmqSubscriptionInstanceWithDefaults()
    {
        // Arrange
        var builder = new RmqSubscriptionBuilder()
            .MessageType<ExampleCommand>();
    
        // Act
        var subscription = builder.Build();
    
        // Assert
        Assert.NotNull(subscription);
        Assert.Null(subscription.DeadLetterChannelName);
        Assert.Null(subscription.DeadLetterRoutingKey);
        Assert.False(subscription.HighAvailability);
        Assert.False(subscription.IsDurable);
        Assert.Null(subscription.TTL);
        Assert.Equal(typeof(ExampleCommand), subscription.DataType);
        Assert.Equal(new SubscriptionName(typeof(ExampleCommand).FullName!), subscription.Name);
        Assert.Equal(new ChannelName(typeof(ExampleCommand).FullName!), subscription.ChannelName);
        Assert.Equal(new RoutingKey(typeof(ExampleCommand).FullName!), subscription.RoutingKey);
        Assert.Equal(1, subscription.BufferSize);
        Assert.Equal(Environment.ProcessorCount, subscription.NoOfPeformers);
        Assert.Equal(300, subscription.TimeoutInMilliseconds);
        Assert.Equal(-1, subscription.RequeueCount);
        Assert.Equal(0, subscription.RequeueDelayInMilliseconds);
        Assert.Equal(0, subscription.UnacceptableMessageLimit);
        Assert.False(subscription.RunAsync);
        Assert.Null(subscription.ChannelFactory);
        Assert.Equal(OnMissingChannel.Create, subscription.MakeChannels);
        Assert.Equal(500, subscription.EmptyChannelDelay);
        Assert.Equal(1000, subscription.ChannelFailureDelay);
    }
}