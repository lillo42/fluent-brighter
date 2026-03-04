# Distributed Locking

Distributed locking coordinates message processing across multiple service instances to prevent concurrent execution of the outbox sweeper or other shared operations.

## Supported Providers

| Provider | Method | Package |
|----------|--------|---------|
| PostgreSQL | `UsePostgresDistributedLock(...)` | `Fluent.Brighter.Postgres` |
| SQL Server | `UseMicrosoftSqlServerDistributedLock(...)` | `Fluent.Brighter.SqlServer` |
| MySQL | `UseMySqlDistributedLock(...)` | `Fluent.Brighter.MySql` |
| MongoDB | `UseMongoDbDistributedLock(...)` | `Fluent.Brighter.MongoDb` |
| DynamoDB | `UseDynamoDbDistributedLock(...)` | `Fluent.Brighter.AWS.V4` |
| Firestore | `UseFirestoreLocking(...)` | `Fluent.Brighter.GoogleCloud` |

## Configuration

### Via Provider Configurator (Integrated)

When using a database provider, enable distributed locking directly:

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingPostgres(cfg =>
    {
        cfg.SetConnection(new RelationalDatabaseConfiguration("Host=localhost;Database=brighter;"));
        cfg.UseDistributedLock();
    }));
```

### Via Producer Builder (Standalone)

When using a separate messaging transport:

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingRabbitMq(rabbitmq => /* ... */)
    .Producers(producer => producer
        .UsePostgresOutbox(/* ... */)
        .UsePostgresDistributedLock("Host=localhost;Database=brighter;")));
```

### Provider-Specific Examples

#### PostgreSQL

```csharp
// With connection string
.UsePostgresDistributedLock("Host=localhost;Database=brighter;")

// With configuration object
.UsePostgresDistributedLock(new RelationalDatabaseConfiguration("Host=localhost;Database=brighter;"))
```

#### SQL Server

```csharp
.UseMicrosoftSqlServerDistributedLock("Server=localhost;Database=Brighter;")
```

#### MySQL

```csharp
.UseMySqlDistributedLock("Server=localhost;Database=brighter;User=root;Password=password;")
```

#### MongoDB

```csharp
// Via configurator
.UsingMongoDb(mongodb => mongodb
    .SetConnection(c => c.SetConnectionString("mongodb://localhost:27017").SetDatabaseName("brighter"))
    .UseDistributedLock("locking"))

// Via producer builder
.Producers(producer => producer
    .UseMongoDbDistributedLock(cfg => { /* MongoDbLockingBuilder config */ }))
```

#### DynamoDB

```csharp
.UsingAws(aws => aws
    // ... connection config ...
    .UseDynamoDbDistributedLock(cfg => { /* DynamoDbLockingProviderOptionsBuilder config */ }))
```

#### Firestore

```csharp
.UsingGcp(gcp => gcp
    // ... connection config ...
    .UseFirestoreLocking(cfg => { /* FirestoreLockingBuilder config */ }))
```
