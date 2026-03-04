# Apache RocketMQ

Fluent Brighter provides support for Apache RocketMQ as a messaging transport.

## Installation

```bash
dotnet add package Fluent.Brighter.RocketMQ
```

## Basic Setup

```csharp
using Fluent.Brighter;

services.AddFluentBrighter(brighter => brighter
    .UsingRocketMq(rocket => rocket
        .SetConnection(conn => conn
            .SetClient(c => c
                .SetEndpoints("localhost:8081")
                .EnableSsl(false)
                .SetRequestTimeout(TimeSpan.FromSeconds(10))
                .Build()))
        .UsePublications(pub => pub
            .AddPublication<GreetingEvent>(p => p
                .SetTopic("greeting")))
        .UseSubscriptions(sub => sub
            .AddSubscription<GreetingEvent>(s => s
                .SetSubscriptionName("greeting-sub-name")
                .SetTopic("greeting")
                .SetConsumerGroup("greeting-consumer-group")
                .UseReactorMode()))));
```

## Connection Configuration

```csharp
.SetConnection(conn => conn
    .SetClient(c => c
        .SetEndpoints("localhost:8081")
        .EnableSsl(false)
        .SetRequestTimeout(TimeSpan.FromSeconds(10))
        .Build())
    .SetMaxAttempts(3)
    .SetInstrumentation(InstrumentationOptions.All))
```

### Connection Options

| Method | Description |
|--------|-------------|
| `SetClient(ClientConfig)` | RocketMQ client configuration |
| `SetTimerProvider(TimeProvider)` | Custom time provider |
| `SetMaxAttempts(int)` | Maximum retry attempts |
| `SetTransactionChecker(ITransactionChecker)` | Transaction checker |
| `SetInstrumentation(InstrumentationOptions)` | Instrumentation level |

## Publications

```csharp
.UsePublications(pub => pub
    .AddPublication<GreetingEvent>(p => p
        .SetTopic("greeting")
        .SetTags("greeting-tag")
        .AssumeTopicExists()))
```

### Publication Options

| Method | Description |
|--------|-------------|
| `SetTopic(RoutingKey)` | RocketMQ topic name |
| `SetTags(string)` | Message tags for filtering |
| `ValidIfTopicExists()` | Validate topic exists |
| `AssumeTopicExists()` | Assume topic exists |
| `SetInstrumentation(InstrumentationOptions)` | Per-publication instrumentation |
| `SetSource(Uri)` | CloudEvents source |
| `SetDataSchema(Uri)` | CloudEvents data schema |
| `SetSubject(string)` | CloudEvents subject |
| `SetType(CloudEventsType)` | CloudEvents type |
| `SetDefaultHeaders(IDictionary)` | Default message headers |
| `SetReplyTo(string)` | Reply-to address |
| `SetRequestType(Type)` | Request type |
| `SetMakeChannels(OnMissingChannel)` | Channel creation mode |

## Subscriptions

```csharp
.UseSubscriptions(sub => sub
    .AddSubscription<GreetingEvent>(s => s
        .SetSubscriptionName("greeting-sub-name")
        .SetTopic("greeting")
        .SetConsumerGroup("greeting-consumer-group")
        .UseReactorMode()
        .SetNoOfPerformers(5)
        .SetFilter(new FilterExpression("greeting-tag"))))
```

### Subscription Options

| Method | Description |
|--------|-------------|
| `SetSubscriptionName(SubscriptionName)` | Unique subscription name |
| `SetTopic(RoutingKey)` | RocketMQ topic to consume |
| `SetConsumerGroup(string)` | Consumer group name |
| `SetNoOfPerformers(int)` | Number of concurrent handlers |
| `SetMessagePumpType(MessagePumpType)` | `Reactor` or `Proactor` |
| `UseReactorMode()` / `UseProactorMode()` | Convenience methods for pump type |
| `SetFilter(FilterExpression)` | Message filter expression |
| `SetTimeout(TimeSpan)` | Processing timeout |
| `SetBufferSize(int)` | Internal buffer size |
| `SetRequeueCount(int)` | Max requeue attempts |
| `SetRequeueDelay(TimeSpan)` | Requeue delay |
| `SetUnacceptableMessageLimit(int)` | Max unacceptable messages |
| `SetMakeChannels(OnMissingChannel)` | Channel creation mode |
| `CreateInfrastructureIfMissing()` | Auto-create infrastructure |
| `ValidIfInfrastructureExists()` | Validate infrastructure exists |
| `AssumeInfrastructureExists()` | Assume infrastructure exists |
| `SetReceiveMessageTimeout(TimeSpan)` | Receive timeout |
| `SetInvisibilityTimeout(TimeSpan)` | Message invisibility timeout |
| `SetEmptyChannelDelay(TimeSpan)` | Delay when channel is empty |
| `SetChannelFailureDelay(TimeSpan)` | Delay after channel failure |

## Full Example

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingRocketMq(rocket => rocket
        .SetConnection(conn => conn
            .SetClient(c => c
                .SetEndpoints("localhost:8081")
                .EnableSsl(false)
                .SetRequestTimeout(TimeSpan.FromSeconds(10))
                .Build()))
        .UsePublications(pub => pub
            .AddPublication<GreetingEvent>(p => p
                .SetTopic("greeting")))
        .UseSubscriptions(sub => sub
            .AddSubscription<GreetingEvent>(s => s
                .SetSubscriptionName("greeting-sub-name")
                .SetTopic("greeting")
                .SetConsumerGroup("greeting-consumer-group")
                .UseReactorMode()))));
```
