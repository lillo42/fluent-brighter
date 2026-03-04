# MySQL

Fluent Brighter supports MySQL as an **outbox/inbox store** for use with other messaging providers.

## Installation

```bash
dotnet add package Fluent.Brighter.MySql
```

## Setup

Use MySQL for the outbox/inbox pattern alongside a messaging provider:

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingRabbitMq(rabbitmq => /* ... messaging config ... */)
    .Producers(producer => producer
        .UseMySqlOutbox(cfg => cfg
            .SetConnectionString("Server=localhost;Database=brighter;User=root;Password=password;")
            .SetOutboxTableName("outbox"))
        .UseMySqlDistributedLock("Server=localhost;Database=brighter;User=root;Password=password;"))
    .Subscriptions(sub => sub
        .UseMySqlInbox(cfg => cfg
            .SetConnectionString("Server=localhost;Database=brighter;User=root;Password=password;")
            .SetInboxTableName("inbox")))
    .UseOutboxSweeper());
```

## Integrated Configuration

You can also use the integrated `UsingMySql` configurator:

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingRabbitMq(rabbitmq => /* ... messaging config ... */)
    .UsingMySql(mysql => mysql
        .SetConnection(cfg => cfg
            .SetConnectionString("Server=localhost;Database=brighter;User=root;Password=password;"))
        .UseOutbox()
        .UseInbox()
        .UseDistributedLock()));
```

### Configurator Options

| Method | Description |
|--------|-------------|
| `SetConnection(Action<RelationalDatabaseConfigurationBuilder>)` | Configure connection via builder |
| `SetConnection(RelationalDatabaseConfiguration)` | Set connection directly |
| `UseOutbox()` | Enable MySQL outbox |
| `UseInbox()` | Enable MySQL inbox |
| `UseDistributedLock()` | Enable distributed locking |

## Extension Methods on ProducerBuilder

| Method | Description |
|--------|-------------|
| `UseMySqlOutbox(Action<RelationalDatabaseConfigurationBuilder>)` | Configure outbox via builder |
| `UseMySqlOutbox(RelationalDatabaseConfiguration)` | Set outbox config directly |
| `UseMySqlOutbox(Action<MySqlOutboxBuilder>)` | Advanced outbox config |
| `UseMySqlDistributedLock(IAmARelationalDatabaseConfiguration)` | Enable distributed lock |
| `UseMySqlDistributedLock(string connectionString)` | Enable distributed lock with connection string |

## Extension Methods on ConsumerBuilder

| Method | Description |
|--------|-------------|
| `UseMySqlInbox(Action<RelationalDatabaseConfigurationBuilder>)` | Configure inbox via builder |
| `UseMySqlInbox(IAmARelationalDatabaseConfiguration)` | Set inbox config directly |
| `UseMySqlInbox(Action<MySqlInboxBuilder>)` | Advanced inbox config |

## Database Configuration

All relational providers share the `RelationalDatabaseConfigurationBuilder`:

| Method | Description |
|--------|-------------|
| `SetConnectionString(string)` | Database connection string |
| `SetDatabaseName(string)` | Database name |
| `SetOutboxTableName(string)` | Outbox table name |
| `SetInboxTableName(string)` | Inbox table name |
| `SetQueueStoreTable(string)` | Queue store table name |
| `SetSchemaName(string)` | Database schema |
| `SetBinaryMessagePayload(bool)` | Enable binary message payload |
