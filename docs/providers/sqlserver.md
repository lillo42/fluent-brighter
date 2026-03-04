# SQL Server

Fluent Brighter supports SQL Server both as a **message transport** (queues backed by database tables) and as an **outbox/inbox store** for use with other messaging providers.

## Installation

```bash
dotnet add package Fluent.Brighter.SqlServer
```

## As a Message Transport

Use SQL Server tables as message queues:

```csharp
services.AddFluentBrighter(builder =>
{
    builder
        .UseDbTransactionOutboxArchive()
        .UseOutboxSweeper()
        .UsingMicrosoftSqlServer(cfg =>
        {
            cfg.SetConnection(new RelationalDatabaseConfiguration(
                "Server=localhost;Database=BrighterTests;",
                databaseName: "BrighterTests",
                queueStoreTable: "QueueData"));

            cfg
                .UseOutbox()
                .UseInbox()
                .UseDistributedLock()
                .UsePublications(pp => pp
                    .AddPublication<GreetingEvent>(p => p
                        .SetQueue("greeting.queue"))
                    .AddPublication<FarewellEvent>(p => p
                        .SetQueue("farewell.queue")))
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
| `UseOutbox()` | Enable SQL Server outbox |
| `UseOutbox(Action<RelationalDatabaseConfigurationBuilder>)` | Outbox with custom config |
| `UseInbox()` | Enable SQL Server inbox |
| `UseInbox(Action<RelationalDatabaseConfigurationBuilder>)` | Inbox with custom config |
| `UseDistributedLock()` | Enable distributed locking |
| `UsePublications(...)` | Configure message publications |
| `UseSubscriptions(...)` | Configure message subscriptions |

### Publication Options

| Method | Description |
|--------|-------------|
| `SetQueue(RoutingKey)` | Queue name |
| `SetSource(Uri)` | CloudEvents source |
| `SetDataSchema(Uri)` | CloudEvents data schema |
| `SetRequestType(Type)` | Request type |
| `SetMakeChannels(OnMissingChannel)` | Channel creation mode |

### Subscription Options

| Method | Description |
|--------|-------------|
| `SetSubscription(SubscriptionName)` | Subscription name |
| `SetQueue(ChannelName)` | Queue to consume from |
| `SetMessagePumpType(MessagePumpType)` | `Reactor` or `Proactor` |
| `SetNumberOfPerformers(int)` | Concurrent consumers |
| `SetTimeout(TimeSpan)` | Processing timeout |
| `SetBufferSize(int)` | Internal buffer size |
| `SetRequeueCount(int)` | Max requeue attempts |
| `SetRequeueDelay(TimeSpan)` | Requeue delay |
| `SetMakeChannels(OnMissingChannel)` | Channel creation mode |
| `SetEmptyChannelDelay(TimeSpan)` | Delay when channel is empty |
| `SetChannelFailureDelay(TimeSpan)` | Delay after channel failure |

## As an Outbox/Inbox Store

Use SQL Server for the outbox/inbox pattern alongside another messaging provider:

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingRabbitMq(rabbitmq => /* ... RabbitMQ config ... */)
    .Producers(producer => producer
        .UseMicrosoftSqlServerOutbox(cfg => cfg
            .SetConnectionString("Server=localhost;Database=Brighter;")
            .SetOutboxTableName("Outbox"))
        .UseMicrosoftSqlServerDistributedLock("Server=localhost;Database=Brighter;"))
    .Subscriptions(sub => sub
        .UseMicrosoftSqlServerInbox(cfg => cfg
            .SetConnectionString("Server=localhost;Database=Brighter;")
            .SetInboxTableName("Inbox"))));
```

### Extension Methods on ProducerBuilder

| Method | Description |
|--------|-------------|
| `UseMicrosoftSqlServerOutbox(Action<RelationalDatabaseConfigurationBuilder>)` | Configure outbox via builder |
| `UseMicrosoftSqlServerOutbox(RelationalDatabaseConfiguration)` | Set outbox config directly |
| `UseMicrosoftSqlServerOutbox(Action<SqlServerOutboxBuilder>)` | Advanced outbox config |
| `UseMicrosoftSqlServerDistributedLock(IAmARelationalDatabaseConfiguration)` | Enable distributed lock |
| `UseMicrosoftSqlServerDistributedLock(string connectionString)` | Enable distributed lock with connection string |

### Extension Methods on ConsumerBuilder

| Method | Description |
|--------|-------------|
| `UseMicrosoftSqlServerInbox(Action<RelationalDatabaseConfigurationBuilder>)` | Configure inbox via builder |
| `UseMicrosoftSqlServerInbox(IAmARelationalDatabaseConfiguration)` | Set inbox config directly |
| `UseMicrosoftSqlServerInbox(Action<SqlServerInboxBuilder>)` | Advanced inbox config |
