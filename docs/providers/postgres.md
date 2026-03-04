# PostgreSQL

Fluent Brighter supports PostgreSQL both as a **message transport** (queues backed by database tables) and as an **outbox/inbox store** for use with other messaging providers.

## Installation

```bash
dotnet add package Fluent.Brighter.Postgres
```

## As a Message Transport

Use PostgreSQL tables as message queues:

```csharp
services.AddFluentBrighter(builder =>
{
    builder
        .UseDbTransactionOutboxArchive()
        .UseOutboxSweeper()
        .UsingPostgres(cfg =>
        {
            cfg.SetConnection(new RelationalDatabaseConfiguration(
                "Host=localhost;Username=postgres;Password=password;Database=brighter;"));

            cfg
                .UseOutbox()
                .UseInbox()
                .UseDistributedLock()
                .UsePublications(pp => pp
                    .AddPublication<GreetingEvent>(p => p
                        .SetQueue("greeting.queue")
                        .CreateQueueIfMissing())
                    .AddPublication<FarewellEvent>(p => p
                        .SetQueue("farewell.queue")
                        .CreateQueueIfMissing()))
                .UseSubscriptions(sb => sb
                    .AddSubscription<GreetingEvent>(s => s
                        .SetQueue("greeting.queue")
                        .SetMessagePumpType(MessagePumpType.Reactor))
                    .AddSubscription<FarewellEvent>(s => s
                        .SetQueue("farewell.queue")
                        .SetMessagePumpType(MessagePumpType.Reactor)));
        });
});
```

### Configurator Options

| Method | Description |
|--------|-------------|
| `SetConnection(Action<RelationalDatabaseConfigurationBuilder>)` | Configure connection via builder |
| `SetConnection(RelationalDatabaseConfiguration)` | Set connection directly |
| `UseOutbox()` | Enable PostgreSQL outbox |
| `UseInbox()` | Enable PostgreSQL inbox |
| `UseDistributedLock()` | Enable distributed locking |
| `UsePublications(...)` | Configure message publications |
| `UseSubscriptions(...)` | Configure message subscriptions |

### Publication Options

| Method | Description |
|--------|-------------|
| `SetQueue(RoutingKey)` | Queue name |
| `CreateQueueIfMissing()` | Auto-create queue |
| `ValidIfQueueExists()` | Validate queue exists |
| `AssumeQueueExists()` | Assume queue exists |
| `SetSchemaName(string)` | Database schema name |
| `SetQueueStoreTable(string)` | Queue store table name |
| `EnableBinaryMessagePayload()` / `DisableBinaryMessagePayload()` | Binary payload mode |
| `SetSource(Uri)` | CloudEvents source |
| `SetDataSchema(Uri)` | CloudEvents data schema |
| `SetRequestType(Type)` | Request type |

### Subscription Options

| Method | Description |
|--------|-------------|
| `SetSubscription(SubscriptionName)` | Subscription name |
| `SetQueue(ChannelName)` | Queue to consume from |
| `SetMessagePumpType(MessagePumpType)` | `Reactor` or `Proactor` |
| `UseReactorMode()` / `UseProactorMode()` | Convenience pump type methods |
| `SetNumberOfPerformers(int)` | Concurrent consumers |
| `SetSchemaName(string)` | Database schema name |
| `SetQueueStoreTable(string)` | Queue store table name |
| `SetVisibleTimeout(TimeSpan)` | Message visibility timeout |
| `SetTableWithLargeMessage(bool)` | Enable large message support |
| `EnableBinaryMessagePayload()` / `DisableBinaryMessagePayload()` | Binary payload mode |
| `SetTimeout(TimeSpan)` | Processing timeout |
| `SetBufferSize(int)` | Internal buffer size |
| `SetRequeueCount(int)` | Max requeue attempts |
| `SetRequeueDelay(TimeSpan)` | Requeue delay |
| `SetMakeChannels(OnMissingChannel)` | Channel creation mode |

## As an Outbox/Inbox Store

Use PostgreSQL for the outbox/inbox pattern alongside another messaging provider (e.g., RabbitMQ):

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingRabbitMq(rabbitmq => /* ... RabbitMQ config ... */)
    .Producers(producer => producer
        .UsePostgresOutbox(cfg => cfg
            .SetConnectionString("Host=localhost;Database=brighter;")
            .SetOutboxTableName("outbox"))
        .UsePostgresDistributedLock("Host=localhost;Database=brighter;"))
    .Subscriptions(sub => sub
        .UsePostgresInbox(cfg => cfg
            .SetConnectionString("Host=localhost;Database=brighter;")
            .SetInboxTableName("inbox")))
    .UseOutboxSweeper(cfg => cfg
        .SetTimerInterval(30)
        .SetBatchSize(100)));
```

### Extension Methods on ProducerBuilder

| Method | Description |
|--------|-------------|
| `UsePostgresOutbox(Action<RelationalDatabaseConfigurationBuilder>)` | Configure outbox via builder |
| `UsePostgresOutbox(RelationalDatabaseConfiguration)` | Set outbox config directly |
| `UsePostgresOutbox(Action<PostgresOutboxBuilder>)` | Advanced outbox config |
| `UsePostgresDistributedLock(IAmARelationalDatabaseConfiguration)` | Enable distributed lock |
| `UsePostgresDistributedLock(string connectionString)` | Enable distributed lock with connection string |

### Extension Methods on ConsumerBuilder

| Method | Description |
|--------|-------------|
| `UsePostgresInbox(Action<RelationalDatabaseConfigurationBuilder>)` | Configure inbox via builder |
| `UsePostgresInbox(IAmARelationalDatabaseConfiguration)` | Set inbox config directly |
| `UsePostgresInbox(Action<PostgresInboxBuilder>)` | Advanced inbox config |

### Database Configuration Builder

The `RelationalDatabaseConfigurationBuilder` is used across all relational providers:

| Method | Description |
|--------|-------------|
| `SetConnectionString(string)` | Database connection string |
| `SetDatabaseName(string)` | Database name |
| `SetOutboxTableName(string)` | Outbox table name |
| `SetInboxTableName(string)` | Inbox table name |
| `SetQueueStoreTable(string)` | Queue store table name |
| `SetSchemaName(string)` | Database schema |
| `SetBinaryMessagePayload(bool)` | Enable binary message payload |
