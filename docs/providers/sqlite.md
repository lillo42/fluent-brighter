# SQLite

Fluent Brighter supports SQLite as an **outbox/inbox store** for use with other messaging providers. This is particularly useful for development and testing scenarios.

## Installation

```bash
dotnet add package Fluent.Brighter.Sqlite
```

## Setup

Use SQLite for the outbox/inbox pattern alongside a messaging provider:

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingRabbitMq(rabbitmq => /* ... messaging config ... */)
    .Producers(producer => producer
        .UseSqliteOutbox(cfg => cfg
            .SetConnectionString("Data Source=brighter.db")
            .SetOutboxTableName("outbox")))
    .Subscriptions(sub => sub
        .UseSqliteInbox(cfg => cfg
            .SetConnectionString("Data Source=brighter.db")
            .SetInboxTableName("inbox")))
    .UseOutboxSweeper());
```

## Integrated Configuration

You can also use the integrated `UsingSqlite` configurator:

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingRabbitMq(rabbitmq => /* ... messaging config ... */)
    .UsingSqlite(sqlite => sqlite
        .SetConnection(cfg => cfg
            .SetConnectionString("Data Source=brighter.db"))
        .UseOutbox()
        .UseInbox()));
```

### Configurator Options

| Method | Description |
|--------|-------------|
| `SetConnection(Action<RelationalDatabaseConfigurationBuilder>)` | Configure connection via builder |
| `SetConnection(RelationalDatabaseConfiguration)` | Set connection directly |
| `UseOutbox()` | Enable SQLite outbox |
| `UseInbox()` | Enable SQLite inbox |

> **Note:** SQLite does not support distributed locking due to its single-writer nature.

## Extension Methods on ProducerBuilder

| Method | Description |
|--------|-------------|
| `UseSqliteOutbox(Action<RelationalDatabaseConfigurationBuilder>)` | Configure outbox via builder |
| `UseSqliteOutbox(RelationalDatabaseConfiguration)` | Set outbox config directly |
| `UseSqliteOutbox(Action<SqliteOutboxBuilder>)` | Advanced outbox config |

## Extension Methods on ConsumerBuilder

| Method | Description |
|--------|-------------|
| `UseSqliteInbox(Action<RelationalDatabaseConfigurationBuilder>)` | Configure inbox via builder |
| `UseSqliteInbox(IAmARelationalDatabaseConfiguration)` | Set inbox config directly |
| `UseSqliteInbox(Action<SqliteInboxBuilder>)` | Advanced inbox config |

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
