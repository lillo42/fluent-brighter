# AWS SNS/SQS

Fluent Brighter provides full support for AWS SNS (publishing) and SQS (subscribing), along with DynamoDB for outbox/inbox, S3 for luggage storage, and EventBridge for scheduling.

## Installation

```bash
dotnet add package Fluent.Brighter.AWS.V4
```

> There is also a legacy `Fluent.Brighter.AWS` package. New projects should use `AWS.V4`.

## Basic Setup

```csharp
using Fluent.Brighter;

services.AddFluentBrighter(brighter => brighter
    .UsingAws(aws => aws
        .SetConnection(conn => conn
            .SetCredentials(new BasicAWSCredentials("key", "secret"))
            .SetRegion(RegionEndpoint.USEast1))
        .UseSnsPublication(pb => pb
            .AddPublication<GreetingEvent>(p => p
                .SetTopic("greeting-event-topic")
                .CreateTopicIfMissing()))
        .UseSqsSubscription(sb => sb
            .AddSubscription<GreetingEvent>(s => s
                .SetSubscriptionName("paramore.example.greeting")
                .SetQueue("greeting-event-queue")
                .SetTopic("greeting-event-topic")
                .SetMessagePumpType(MessagePumpType.Proactor)
                .SetMakeChannels(OnMissingChannel.Create)))));
```

## Connection Configuration

```csharp
.SetConnection(conn => conn
    .SetCredentials(new BasicAWSCredentials("key", "secret"))
    .SetRegion(RegionEndpoint.USEast1)
    .SetClientConfigAction(cfg => cfg.ServiceURL = "http://localhost:4566"))
```

### Connection Options

| Method | Description |
|--------|-------------|
| `SetCredentials(AWSCredentials)` | AWS credentials |
| `SetRegion(RegionEndpoint)` | AWS region |
| `SetClientConfigAction(Action<ClientConfig>)` | Custom client configuration (e.g., LocalStack endpoint) |

## SNS Publications

Publish messages to SNS topics:

```csharp
.UseSnsPublication(pb => pb
    .AddPublication<OrderCreatedEvent>(p => p
        .SetTopic("orders-created")
        .CreateTopicIfMissing()
        .FindTopicByName()
        .SetTopicAttributes(attrs => attrs
            .UseStandardSns())))
```

### SNS Publication Options

| Method | Description |
|--------|-------------|
| `SetTopic(RoutingKey)` | SNS topic name |
| `CreateTopicIfMissing()` | Auto-create topic |
| `ValidateIfNotExists()` | Validate topic exists |
| `AssumeIfNotExists()` | Assume topic exists |
| `FindTopicByName()` | Find topic by name |
| `FindTopicByArn()` | Find topic by ARN |
| `FindTopicByConvention()` | Find topic by convention |
| `SetTopicArn(string)` | Set explicit topic ARN |
| `SetTopicAttributes(Action<SnsAttributesBuilder>)` | Configure SNS attributes |
| `SetSource(Uri)` / `SetSource(string)` | CloudEvents source |
| `SetDataSchema(Uri)` / `SetDataSchema(string)` | CloudEvents data schema |

### SNS Attributes

```csharp
.SetTopicAttributes(attrs => attrs
    .UseStandardSns()         // or .UseFifoSns()
    .SetDeliveryPolicy("...")
    .SetPolicy("...")
    .EnableContentBasedDeduplication())
```

## SQS Publications

Publish messages directly to SQS queues (point-to-point):

```csharp
.UseSqsPublication(pb => pb
    .AddPublication<FarewellEvent>(p => p
        .SetQueue("farewell-event-queue")
        .CreateIfMissing()
        .UsePointToPoint()))
```

### SQS Publication Options

| Method | Description |
|--------|-------------|
| `SetQueue(RoutingKey)` | SQS queue name |
| `SetQueueUrl(ChannelName)` | Explicit queue URL |
| `UsePointToPoint()` | Point-to-point channel type |
| `UsePubSub()` | Pub/sub channel type |
| `CreateIfMissing()` | Auto-create queue |
| `FindQueueByName()` | Find queue by name |
| `FindQueueByUrl()` | Find queue by URL |
| `SetQueueAttributes(Action<SqsAttributesBuilder>)` | Configure SQS attributes |

### SQS Attributes

```csharp
.SetQueueAttributes(attrs => attrs
    .UseStandardQueue()       // or .UseFifoQueue()
    .SetLockTimeout(TimeSpan.FromSeconds(30))
    .SetMessageRetentionPeriod(TimeSpan.FromDays(14))
    .EnableRawMessageDelivery()
    .SetRedrivePolicy(redrive => redrive
        .SetDeadLetterQueue("my-dlq")
        .SetMaxReceiveCount(5)))
```

## SQS Subscriptions

```csharp
.UseSqsSubscription(sb => sb
    .AddSubscription<OrderCreatedEvent>(s => s
        .SetSubscriptionName("order-processor")
        .SetQueue("order-processing-queue")
        .SetTopic("orders-created")
        .SetMessagePumpType(MessagePumpType.Proactor)
        .SetMakeChannels(OnMissingChannel.Create)
        .SetNoOfPerformers(5)))
```

### SQS Subscription Options

| Method | Description |
|--------|-------------|
| `SetSubscriptionName(SubscriptionName)` | Unique subscription name |
| `SetQueue(ChannelName)` | SQS queue name |
| `SetTopic(RoutingKey)` | SNS topic to subscribe to |
| `SetChannelType(ChannelType)` | Channel type |
| `SetNoOfPerformers(int)` | Number of concurrent handlers |
| `SetMessagePumpType(MessagePumpType)` | `Reactor` or `Proactor` |
| `SetMakeChannels(OnMissingChannel)` | Channel creation mode |
| `SetTimeOut(TimeSpan)` | Processing timeout |
| `SetBufferSize(int)` | Internal buffer size |
| `SetRequeueCount(int)` | Max requeue attempts |
| `SetRequeueDelay(TimeSpan)` | Requeue delay |
| `SetFindTopicBy(TopicFindBy)` | Topic discovery strategy |
| `SetFindQueueBy(QueueFindBy)` | Queue discovery strategy |
| `SetQueueAttributes(SqsAttributes)` | SQS queue attributes |
| `SetTopicAttributes(SnsAttributes)` | SNS topic attributes |
| `SetEmptyChannelDelay(TimeSpan)` | Delay when channel is empty |
| `SetChannelFailureDelay(TimeSpan)` | Delay after channel failure |

## DynamoDB Outbox & Inbox

```csharp
.UsingAws(aws => aws
    .SetConnection(/* ... */)
    .UseSnsPublication(/* ... */)
    .UseSqsSubscription(/* ... */)
    .UseDynamoDbOutbox("outbox-table")
    .UseDynamoDbInbox("inbox-table")
    .UseDynamoDbOutboxArchive())
```

### DynamoDB Options

| Method | Description |
|--------|-------------|
| `UseDynamoDbOutbox()` | Use DynamoDB outbox with default table |
| `UseDynamoDbOutbox(string tableName)` | Use DynamoDB outbox with specific table |
| `UseDynamoDbOutbox(Action<DynamoDbOutboxConfigurationBuilder>)` | Advanced outbox configuration |
| `UseDynamoDbInbox()` | Use DynamoDB inbox with default table |
| `UseDynamoDbInbox(string tableName)` | Use DynamoDB inbox with specific table |
| `UseDynamoDbOutboxArchive()` | Enable outbox archiving |
| `UseDynamoDbOutboxArchive(Action<TimedOutboxArchiverOptionsBuilder>)` | Configure archive options |
| `UseDynamoDbDistributedLock(Action<DynamoDbLockingProviderOptionsBuilder>)` | Enable distributed locking |

## S3 Luggage Store

Store large message payloads in S3:

```csharp
.UsingAws(aws => aws
    // ... other config ...
    .UseS3LuggageStore("my-luggage-bucket"))
```

## EventBridge Scheduler

```csharp
.UsingAws(aws => aws
    // ... other config ...
    .UseScheduler())
```

## Full Example

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingAws(aws => aws
        .SetConnection(conn => conn
            .SetCredentials(new BasicAWSCredentials("key", "secret"))
            .SetRegion(RegionEndpoint.USEast1))
        .UseSnsPublication(pb => pb
            .AddPublication<GreetingEvent>(p => p
                .SetTopic("greeting-event-topic")
                .CreateTopicIfMissing()))
        .UseSqsPublication(pb => pb
            .AddPublication<FarewellEvent>(p => p
                .SetQueue("farewell-event-queue")
                .CreateIfMissing()))
        .UseSqsSubscription(sb => sb
            .AddSubscription<GreetingEvent>(s => s
                .SetSubscriptionName("paramore.example.greeting")
                .SetQueue("greeting-event-queue")
                .SetTopic("greeting-event-topic")
                .SetMessagePumpType(MessagePumpType.Proactor)
                .SetMakeChannels(OnMissingChannel.Create))
            .AddSubscription<FarewellEvent>(s => s
                .SetSubscriptionName("paramore.example.farewell")
                .SetQueue("farewell-event-queue")
                .SetMessagePumpType(MessagePumpType.Proactor)
                .SetMakeChannels(OnMissingChannel.Create)))
        .UseDynamoDbOutbox("outbox-table")
        .UseDynamoDbInbox("inbox-table")
        .UseDynamoDbOutboxArchive()));
```
