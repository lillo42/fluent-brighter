# RabbitMQ

Fluent Brighter provides full support for RabbitMQ via the `Fluent.Brighter.RMQ.Async` (recommended) and `Fluent.Brighter.RMQ.Sync` packages.

## Installation

```bash
# Async (recommended)
dotnet add package Fluent.Brighter.RMQ.Async

# Sync (if needed)
dotnet add package Fluent.Brighter.RMQ.Sync
```

## Basic Setup

```csharp
using Fluent.Brighter;

services.AddFluentBrighter(brighter => brighter
    .UsingRabbitMq(rabbitmq => rabbitmq
        .SetConnection(conn => conn
            .SetAmpq(amqp => amqp.SetUri("amqp://guest:guest@localhost:5672"))
            .SetExchange(exchange => exchange.SetName("my.exchange")))
        .UsePublications(pb => pb
            .AddPublication<GreetingEvent>(p => p
                .SetTopic("greeting.event.topic")
                .CreateTopicIfMissing()))
        .UseSubscriptions(sb => sb
            .AddSubscription<GreetingEvent>(s => s
                .SetSubscription("greeting-handler")
                .SetQueue("greeting.event.queue")
                .SetTopic("greeting.event.topic")))));
```

## Connection Configuration

Configure the RabbitMQ connection using `SetConnection`:

```csharp
.SetConnection(conn => conn
    .SetAmpq(amqp => amqp
        .SetUri("amqp://guest:guest@localhost:5672")
        .SetConnectionRetryCount(3)
        .SetRetryWaitInMilliseconds(500)
        .SetCircuitBreakTimeInMilliseconds(60000))
    .SetExchange(exchange => exchange
        .SetName("my.exchange")
        .SetType("topic")
        .SetDurable(true))
    .SetName("my-connection")
    .SetHeartbeat(60)
    .SetPersistMessages(true)
    .SetContinuationTimeout(120))
```

### Connection Options

| Method | Description |
|--------|-------------|
| `SetAmpq(...)` | Configure the AMQP URI and retry settings |
| `SetExchange(...)` | Configure the exchange name, type, and durability |
| `SetDeadLetterExchange(...)` | Configure a dead letter exchange |
| `SetName(string)` | Set the connection name |
| `SetHeartbeat(ushort)` | Set heartbeat interval in seconds |
| `SetPersistMessages(bool)` | Enable/disable message persistence |
| `SetContinuationTimeout(ushort)` | Set the continuation timeout |

### AMQP URI Options

| Method | Description |
|--------|-------------|
| `SetUri(Uri)` | The AMQP connection URI |
| `SetConnectionRetryCount(int)` | Number of connection retry attempts |
| `SetRetryWaitInMilliseconds(int)` | Delay between retries |
| `SetCircuitBreakTimeInMilliseconds(int)` | Circuit breaker timeout |

### Exchange Options

| Method | Description |
|--------|-------------|
| `SetName(string)` | Exchange name |
| `SetType(string)` | Exchange type (e.g., `"topic"`, `"direct"`, `"fanout"`) |
| `SetDurable(bool)` | Survive broker restarts |
| `SetSupportDelay(bool)` | Enable delayed message support |

## Publications

Configure message publications with `UsePublications`:

```csharp
.UsePublications(pb => pb
    .AddPublication<GreetingEvent>(p => p
        .SetTopic("greeting.event.topic")
        .CreateTopicIfMissing()
        .SetWaitForConfirmsTimeOutInMilliseconds(5000))
    .AddPublication<FarewellEvent>(p => p
        .SetTopic("farewell.event.topic")
        .CreateTopicIfMissing()))
```

### Publication Options

| Method | Description |
|--------|-------------|
| `SetTopic(RoutingKey)` | The routing key / topic name |
| `CreateTopicIfMissing()` | Auto-create the topic if it doesn't exist |
| `SetMakeChannels(OnMissingChannel)` | Channel creation mode |
| `SetSource(Uri)` | CloudEvents source URI |
| `SetDataSchema(Uri)` | CloudEvents data schema |
| `SetSubject(string)` | CloudEvents subject |
| `SetType(CloudEventsType)` | CloudEvents type |
| `SetDefaultHeaders(IDictionary)` | Default message headers |
| `SetReplyTo(string)` | Reply-to address |
| `SetWaitForConfirmsTimeOutInMilliseconds(int)` | Publisher confirm timeout |
| `SetRequestType(Type)` | The request type for this publication |

## Subscriptions

Configure message subscriptions with `UseSubscriptions`:

```csharp
.UseSubscriptions(sb => sb
    .AddSubscription<GreetingEvent>(s => s
        .SetSubscription("paramore.example.greeting")
        .SetQueue("greeting.event.queue")
        .SetTopic("greeting.event.topic")
        .SetTimeout(TimeSpan.FromSeconds(200))
        .EnableDurable()
        .EnableHighAvailability())
    .AddSubscription<FarewellEvent>(s => s
        .SetSubscription("paramore.example.farewell")
        .SetQueue("farewell.event.queue")
        .SetTopic("farewell.event.topic")))
```

### Subscription Options

| Method | Description |
|--------|-------------|
| `SetSubscription(SubscriptionName)` | Unique subscription identifier |
| `SetQueue(ChannelName)` | RabbitMQ queue name |
| `SetTopic(RoutingKey)` | Routing key to bind to |
| `SetTimeout(TimeSpan)` | Message processing timeout |
| `SetNumberOfPerformers(int)` | Number of concurrent handlers |
| `SetMessagePumpType(MessagePumpType)` | `Reactor` or `Proactor` |
| `SetBufferSize(int)` | Internal buffer size |
| `SetRequeueCount(int)` | Max requeue attempts |
| `SetRequeueDelay(TimeSpan)` | Delay between requeues |
| `SetUnacceptableMessageLimit(int)` | Max unacceptable messages before stopping |
| `SetMakeChannels(OnMissingChannel)` | Channel creation mode |
| `SetEmptyChannelDelay(TimeSpan)` | Delay when channel is empty |
| `SetChannelFailureDelay(TimeSpan)` | Delay after channel failure |
| `SetIsDurable(bool)` | Queue survives broker restart |
| `SethHighAvailability(bool)` | Enable HA mirroring |
| `SetMaxQueueLenght(int)` | Maximum queue length |
| `SetQueueType(QueueType)` | Queue type (classic, quorum) |
| `SetDeadLetterChannel(ChannelName)` | Dead letter queue |
| `SetDeadLetterRoutingKey(RoutingKey)` | Dead letter routing key |
| `SetTTL(TimeSpan)` | Message time-to-live |

### Convenience Extension Methods

```csharp
// Enable durable queue
.EnableDurable()

// Enable high availability
.EnableHighAvailability()
```

## Full Example

```csharp
services
    .AddHostedService<ServiceActivatorHostedService>()
    .AddFluentBrighter(brighter => brighter
        .UsingRabbitMq(rabbitmq => rabbitmq
            .SetConnection(conn => conn
                .SetAmpq(amqp => amqp.SetUri("amqp://guest:guest@localhost:5672"))
                .SetExchange(exchange => exchange.SetName("paramore.brighter.exchange")))
            .UsePublications(pb => pb
                .AddPublication<GreetingEvent>(p => p
                    .SetTopic("greeting.event.topic")
                    .CreateTopicIfMissing())
                .AddPublication<FarewellEvent>(p => p
                    .SetTopic("farewell.event.topic")
                    .CreateTopicIfMissing()))
            .UseSubscriptions(sb => sb
                .AddSubscription<GreetingEvent>(s => s
                    .SetSubscription("paramore.example.greeting")
                    .SetQueue("greeting.event.queue")
                    .SetTopic("greeting.event.topic")
                    .SetTimeout(TimeSpan.FromSeconds(200))
                    .EnableDurable()
                    .EnableHighAvailability())
                .AddSubscription<FarewellEvent>(s => s
                    .SetSubscription("paramore.example.farewell")
                    .SetQueue("farewell.event.queue")
                    .SetTopic("farewell.event.topic")))));
```

## Async vs Sync

The `Fluent.Brighter.RMQ.Async` package uses the asynchronous RabbitMQ client and is recommended for most applications. The `Fluent.Brighter.RMQ.Sync` package provides the same API but uses the synchronous client. Both packages have identical builder APIs.
