# Outbox & Inbox Patterns

## Outbox Pattern

The Transactional Outbox pattern ensures reliable message publishing by storing messages in a database transaction alongside your business data, then publishing them asynchronously via the [Outbox Sweeper](sweeper-archiver.md).

This guarantees that messages are published if and only if the business transaction succeeds — even if the message broker is temporarily unavailable.

### How It Works

1. Your application writes business data and an outbox message in the **same database transaction**
2. The outbox sweeper periodically reads unpublished messages from the outbox table
3. Messages are published to the message broker
4. Published messages are marked as dispatched

### Supported Outbox Stores

| Provider | Method | Package |
|----------|--------|---------|
| PostgreSQL | `UsePostgresOutbox(...)` | `Fluent.Brighter.Postgres` |
| SQL Server | `UseMicrosoftSqlServerOutbox(...)` | `Fluent.Brighter.SqlServer` |
| MySQL | `UseMySqlOutbox(...)` | `Fluent.Brighter.MySql` |
| SQLite | `UseSqliteOutbox(...)` | `Fluent.Brighter.Sqlite` |
| MongoDB | `UseMongoDbOutbox(...)` | `Fluent.Brighter.MongoDb` |
| DynamoDB | `UseDynamoDbOutbox(...)` | `Fluent.Brighter.AWS.V4` |
| Firestore | `UseFirestoreOutbox(...)` | `Fluent.Brighter.GoogleCloud` |
| Spanner | `UseSpannerOutbox(...)` | `Fluent.Brighter.GoogleCloud` |

### Configuration Approaches

There are two ways to configure the outbox:

#### 1. Via the Provider Configurator (Integrated)

When using a database as both transport and outbox:

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingPostgres(cfg =>
    {
        cfg.SetConnection(new RelationalDatabaseConfiguration("Host=localhost;Database=brighter;"));
        cfg.UseOutbox();
    }));
```

#### 2. Via the Producer Builder (Standalone)

When using a separate messaging transport with a database outbox:

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingRabbitMq(rabbitmq => /* ... */)
    .Producers(producer => producer
        .UsePostgresOutbox(cfg => cfg
            .SetConnectionString("Host=localhost;Database=brighter;")
            .SetOutboxTableName("outbox"))));
```

---

## Inbox Pattern

The Inbox pattern prevents duplicate message processing by tracking received messages in a database. When a message arrives, the inbox is checked to determine if it has already been processed.

### Supported Inbox Stores

| Provider | Method | Package |
|----------|--------|---------|
| PostgreSQL | `UsePostgresInbox(...)` | `Fluent.Brighter.Postgres` |
| SQL Server | `UseMicrosoftSqlServerInbox(...)` | `Fluent.Brighter.SqlServer` |
| MySQL | `UseMySqlInbox(...)` | `Fluent.Brighter.MySql` |
| SQLite | `UseSqliteInbox(...)` | `Fluent.Brighter.Sqlite` |
| MongoDB | `UseMongoDbInbox(...)` | `Fluent.Brighter.MongoDb` |
| DynamoDB | `UseDynamoDbInbox(...)` | `Fluent.Brighter.AWS.V4` |
| Firestore | `UseFirestoreInbox(...)` | `Fluent.Brighter.GoogleCloud` |
| Spanner | `UseSpannerInbox(...)` | `Fluent.Brighter.GoogleCloud` |

### Configuration

#### Via the Provider Configurator

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingPostgres(cfg =>
    {
        cfg.SetConnection(new RelationalDatabaseConfiguration("Host=localhost;Database=brighter;"));
        cfg.UseInbox();
    }));
```

#### Via the Consumer Builder

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingRabbitMq(rabbitmq => /* ... */)
    .Subscriptions(sub => sub
        .UsePostgresInbox(cfg => cfg
            .SetConnectionString("Host=localhost;Database=brighter;")
            .SetInboxTableName("inbox"))));
```

#### Inbox Configuration Options

You can also configure inbox behavior directly on the consumer builder:

```csharp
.Subscriptions(sub => sub
    .SetInbox(inbox => inbox
        .SetInbox(myInboxInstance)
        .SetScope(InboxScope.Commands)
        .SetOnceOnly(true)
        .SetActionOnExists(OnceOnlyAction.Throw)))
```

| Method | Description |
|--------|-------------|
| `SetInbox(IAmAnInbox)` | Set the inbox implementation |
| `SetScope(InboxScope)` | Scope: `Commands`, `All` |
| `SetOnceOnly(bool)` | Enable once-only processing |
| `SetActionOnExists(OnceOnlyAction)` | Action when duplicate: `Throw`, `Warn` |
| `SetContext(Func<Type, string>)` | Context key function |
