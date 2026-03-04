# Redis

Fluent Brighter provides support for Redis Streams as a messaging transport.

## Installation

```bash
dotnet add package Fluent.Brighter.Redis
```

## Basic Setup

```csharp
using Fluent.Brighter;

services.AddFluentBrighter(brighter => brighter
    .UsingRedis(redis => redis
        .SetConnection(c => c
            .SetRedisConnectionString("redis://localhost:6379")
            .SetMaxPoolSize(10)
            .SetMessageTimeToLive(TimeSpan.FromMinutes(10)))
        .UsePublications(pb => pb
            .AddPublication<GreetingEvent>(p => p
                .SetTopic("greeting.topic")))
        .UseSubscriptions(sb => sb
            .AddSubscription<GreetingEvent>(s => s
                .SetTopic("greeting.topic")
                .SetQueue("greeting.queue")
                .SetMessagePumpType(MessagePumpType.Reactor)))));
```

## Connection Configuration

```csharp
.SetConnection(c => c
    .SetRedisConnectionString("redis://localhost:6379?ConnectTimeout=1000&SendTimeout=1000")
    .SetMaxPoolSize(10)
    .SetMessageTimeToLive(TimeSpan.FromMinutes(10))
    .SetDefaultRetryTimeout(3_000)
    .SetDefaultConnectTimeout(5_000)
    .SetDefaultSendTimeout(5_000)
    .SetDefaultReceiveTimeout(5_000))
```

### Connection Options

| Method | Description |
|--------|-------------|
| `SetRedisConnectionString(string)` | Redis connection string |
| `SetMaxPoolSize(int)` | Maximum connection pool size |
| `SetMessageTimeToLive(TimeSpan)` | Default message TTL |
| `SetDefaultConnectTimeout(int)` | Connect timeout in ms |
| `SetDefaultSendTimeout(int)` | Send timeout in ms |
| `SetDefaultReceiveTimeout(int)` | Receive timeout in ms |
| `SetDefaultIdleTimeOutSecs(int)` | Idle timeout in seconds |
| `SetDefaultRetryTimeout(int)` | Retry timeout in ms |
| `SetBufferPoolMaxSize(int)` | Buffer pool max size |
| `SetHostLookupTimeoutMs(int)` | Host lookup timeout in ms |
| `SetAssumeServerVersion(int)` | Assume Redis server version |
| `SetDeactivatedClientsExpiry(TimeSpan)` | Deactivated client expiry |
| `SetDisableVerboseLogging(bool)` | Disable verbose logging |
| `SetBackoffMultiplier(int)` | Backoff multiplier |
| `SetVerifyMasterConnections(bool)` | Verify master connections |

## Publications

```csharp
.UsePublications(pb => pb
    .AddPublication<GreetingEvent>(p => p
        .SetTopic("greeting.topic")
        .CreateTopicIfMissing())
    .AddPublication<FarewellEvent>(p => p
        .SetTopic("farewell.topic")))
```

### Publication Options

| Method | Description |
|--------|-------------|
| `SetTopic(RoutingKey)` | Redis stream/topic name |
| `CreateTopicIfMissing()` | Auto-create topic if missing |
| `ValidIfTopicExists()` | Validate topic exists |
| `AssumeTopicExists()` | Assume topic exists |
| `SetSource(Uri)` | CloudEvents source |
| `SetDataSchema(Uri)` | CloudEvents data schema |
| `SetSubject(string)` | CloudEvents subject |
| `SetType(CloudEventsType)` | CloudEvents type |
| `SetDefaultHeaders(IDictionary)` | Default message headers |
| `SetReplyTo(string)` | Reply-to address |
| `SetRequestType(Type)` | Request type |

## Subscriptions

```csharp
.UseSubscriptions(sb => sb
    .AddSubscription<GreetingEvent>(s => s
        .SetTopic("greeting.topic")
        .SetQueue("greeting.queue")
        .SetMessagePumpType(MessagePumpType.Reactor)
        .SetNumberOfPerformers(3)
        .SetTimeout(TimeSpan.FromSeconds(30))))
```

### Subscription Options

| Method | Description |
|--------|-------------|
| `SetSubscription(SubscriptionName)` | Unique subscription identifier |
| `SetQueue(ChannelName)` | Consumer group / queue name |
| `SetTopic(RoutingKey)` | Redis stream to consume |
| `SetMessagePumpType(MessagePumpType)` | `Reactor` or `Proactor` |
| `UseReactorMode()` / `UseProactorMode()` | Convenience methods for pump type |
| `SetNumberOfPerformers(int)` | Number of concurrent consumers |
| `SetTimeout(TimeSpan)` | Processing timeout |
| `SetBufferSize(int)` | Internal buffer size |
| `SetRequeueCount(int)` | Max requeue attempts |
| `SetRequeueDelay(TimeSpan)` | Requeue delay |
| `SetMakeChannels(OnMissingChannel)` | Channel creation mode |
| `CreateInfrastructureIfMissing()` | Auto-create infrastructure |
| `ValidIfInfrastructureExists()` | Validate infrastructure exists |
| `AssumeInfrastructureExists()` | Assume infrastructure exists |
| `SetEmptyChannelDelay(TimeSpan)` | Delay when channel is empty |
| `SetChannelFailureDelay(TimeSpan)` | Delay after channel failure |
| `SetUnacceptableMessageLimit(int)` | Max unacceptable messages |

## Full Example

```csharp
services.AddFluentBrighter(builder =>
{
    builder
        .UsingRedis(cfg =>
        {
            cfg.SetConnection(c => c
                .SetRedisConnectionString("redis://localhost:6379?ConnectTimeout=1000&SendTimeout=1000")
                .SetMaxPoolSize(10)
                .SetMessageTimeToLive(TimeSpan.FromMinutes(10))
                .SetDefaultRetryTimeout(3_000));

            cfg
                .UsePublications(pp => pp
                    .AddPublication<GreetingEvent>(p => p.SetTopic("greeting.topic"))
                    .AddPublication<FarewellEvent>(p => p.SetTopic("farewell.topic")))
                .UseSubscriptions(sb => sb
                    .AddSubscription<GreetingEvent>(s => s
                        .SetTopic("greeting.topic")
                        .SetQueue("greeting.queue")
                        .SetMessagePumpType(MessagePumpType.Reactor))
                    .AddSubscription<FarewellEvent>(s => s
                        .SetTopic("farewell.topic")
                        .SetQueue("farewell.queue")
                        .SetMessagePumpType(MessagePumpType.Reactor)));
        });
});
```
