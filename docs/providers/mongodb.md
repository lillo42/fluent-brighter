# MongoDB

Fluent Brighter supports MongoDB as an **outbox/inbox store**, **distributed lock provider**, and **luggage store** (via GridFS) for use with other messaging providers.

## Installation

```bash
dotnet add package Fluent.Brighter.MongoDb
```

## Setup

Use MongoDB alongside a messaging provider like RabbitMQ:

```csharp
services.AddFluentBrighter(brighter => brighter
    .UseOutboxSweeper()
    .UseOutboxArchiver<IClientSession>(new NullOutboxArchiveProvider())
    .UsingRabbitMq(rabbitmq => rabbitmq
        .SetConnection(conn => conn
            .SetAmpq(amqp => amqp.SetUri("amqp://guest:guest@localhost:5672"))
            .SetExchange(exchange => exchange.SetName("paramore.brighter.exchange")))
        .UsePublications(pb => pb
            .AddPublication<GreetingEvent>(p => p
                .SetTopic("greeting.event.topic")
                .CreateTopicIfMissing()))
        .UseSubscriptions(sb => sb
            .AddSubscription<GreetingEvent>(s => s
                .SetSubscription("greeting-handler")
                .SetQueue("greeting.event.queue")
                .SetTopic("greeting.event.topic"))))
    .UsingMongoDb(mongodb => mongodb
        .SetConnection(c => c
            .SetConnectionString("mongodb://root:example@localhost:27017")
            .SetDatabaseName("brighter"))
        .UseOutbox("outbox")
        .UseInbox("inbox")
        .UseDistributedLock("locking")
        .UseLuggageStore("bucket")));
```

## Connection Configuration

```csharp
.SetConnection(c => c
    .SetConnectionString("mongodb://root:example@localhost:27017")
    .SetDatabaseName("brighter"))
```

### Connection Options

| Method | Description |
|--------|-------------|
| `SetConnectionString(string)` | MongoDB connection string |
| `SetDatabaseName(string)` | Database name |

## Configurator Options

| Method | Description |
|--------|-------------|
| `SetConnection(Action<MongoDbConfigurationBuilder>)` | Configure connection via builder |
| `SetConnection(IAmAMongoDbConfiguration)` | Set connection directly |
| `UseOutbox()` | Enable outbox with default collection |
| `UseOutbox(string collectionName)` | Enable outbox with specific collection |
| `UseOutbox(Action<MongoDbCollectionConfigurationBuilder>)` | Advanced outbox config |
| `UseInbox()` | Enable inbox with default collection |
| `UseInbox(string collectionName)` | Enable inbox with specific collection |
| `UseInbox(Action<MongoDbCollectionConfigurationBuilder>)` | Advanced inbox config |
| `UseDistributedLock()` | Enable distributed lock with default collection |
| `UseDistributedLock(string collectionName)` | Enable distributed lock with specific collection |
| `UseDistributedLock(Action<MongoDbCollectionConfigurationBuilder>)` | Advanced lock config |
| `UseLuggageStore(string bucketName)` | Enable GridFS luggage store |
| `UseLuggageStore(Action<MongoDbLuggageStoreBuilder>)` | Advanced luggage store config |

## Extension Methods on ProducerBuilder

| Method | Description |
|--------|-------------|
| `UseMongoDbOutbox(Action<MongoDbOutboxBuilder>)` | Configure outbox |
| `UseMongoDbDistributedLock(Action<MongoDbLockingBuilder>)` | Configure distributed lock |

## Extension Methods on ConsumerBuilder

| Method | Description |
|--------|-------------|
| `UseMongoDbInbox(Action<MongoDbInboxBuilder>)` | Configure inbox |

## Features

### Outbox

Stores outgoing messages in a MongoDB collection within the same transaction as your business data, ensuring reliable message publishing.

### Inbox

Tracks processed messages in a MongoDB collection to ensure idempotent message handling.

### Distributed Lock

Uses MongoDB for coordinating message processing across multiple service instances.

### Luggage Store

Uses MongoDB GridFS to store large message payloads that exceed the message broker's size limits.

## Full Example

```csharp
services.AddFluentBrighter(brighter => brighter
    .UseOutboxSweeper()
    .UseOutboxArchiver<IClientSession>(new NullOutboxArchiveProvider())
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
                .SetTopic("farewell.event.topic"))))
    .UsingMongoDb(mongodb => mongodb
        .SetConnection(c => c
            .SetConnectionString("mongodb://root:example@localhost:27017")
            .SetDatabaseName("brighter"))
        .UseInbox("inbox")
        .UseOutbox("outbox")
        .UseDistributedLock("locking")
        .UseLuggageStore("bucket")));
```
