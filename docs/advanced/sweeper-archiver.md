# Outbox Sweeper & Archiver

## Outbox Sweeper

The outbox sweeper is a background service that periodically reads unpublished messages from the outbox and publishes them to the message broker. This is a critical component of the [Transactional Outbox pattern](outbox-inbox.md).

### Basic Setup

```csharp
services.AddFluentBrighter(brighter => brighter
    .UseOutboxSweeper()
    // ... provider config ...
);
```

### With Configuration

```csharp
services.AddFluentBrighter(brighter => brighter
    .UseOutboxSweeper(cfg => cfg
        .SetTimerInterval(30)           // Run every 30 seconds
        .SetBatchSize(100)              // Process 100 messages per sweep
        .SetMinimumMessageAge(TimeSpan.FromSeconds(5))  // Only sweep messages older than 5s
        .SetBulk(true))                 // Use bulk dispatch
    // ... provider config ...
);
```

### Sweeper Options

| Method | Description |
|--------|-------------|
| `SetTimerInterval(int seconds)` | Interval between sweeps in seconds |
| `SetBatchSize(int)` | Number of messages to process per sweep |
| `SetMinimumMessageAge(TimeSpan)` | Minimum age before a message is eligible for sweeping |
| `SetBulk(bool)` | Enable bulk dispatch mode |
| `SetArg(string key, object value)` | Set custom arguments |

---

## Outbox Archiver

The outbox archiver moves old dispatched messages from the outbox to an archive store. This keeps the outbox table small and performant.

### Basic Setup

```csharp
services.AddFluentBrighter(brighter => brighter
    .UseDbTransactionOutboxArchive()
    // ... provider config ...
);
```

### With Configuration

```csharp
services.AddFluentBrighter(brighter => brighter
    .UseDbTransactionOutboxArchive(cfg => cfg
        .SetTimerInterval(60)                    // Run every 60 seconds
        .SetMinimumAge(TimeSpan.FromDays(7))     // Archive messages older than 7 days
        .SetArchiveBatchSize(500))               // Archive 500 messages per run
    // ... provider config ...
);
```

### Archiver Options

| Method | Description |
|--------|-------------|
| `SetTimerInterval(int seconds)` | Interval between archive runs in seconds |
| `SetMinimumAge(TimeSpan)` | Minimum age before a message is archived |
| `SetMinimumAgeInHours(int)` | Minimum age in hours (convenience) |
| `SetArchiveBatchSize(int)` | Number of messages to archive per run |
| `SetInstrumentation(InstrumentationOptions)` | Instrumentation level |

### Provider-Specific Archivers

Some providers have their own archive extensions:

```csharp
// DynamoDB
.UseDynamoDbTransactionOutboxArchive()

// Firestore
.UseFirestoreTransactionOutboxArchive()

// Spanner
.UseSpannerTransactionOutboxArchive()
```

### Custom Archive Provider

You can provide a custom archive provider:

```csharp
services.AddFluentBrighter(brighter => brighter
    .UseOutboxArchiver<DbTransaction>(myArchiveProvider, opts =>
    {
        opts.TimerInterval = 60;
        opts.MinimumAge = TimeSpan.FromDays(7);
    }));
```

## Combining Sweeper and Archiver

A typical production setup uses both:

```csharp
services.AddFluentBrighter(brighter => brighter
    .UseOutboxSweeper(cfg => cfg
        .SetTimerInterval(30)
        .SetBatchSize(100))
    .UseDbTransactionOutboxArchive(cfg => cfg
        .SetTimerInterval(300)
        .SetMinimumAge(TimeSpan.FromDays(7))
        .SetArchiveBatchSize(500))
    .UsingRabbitMq(/* ... */)
    .UsingPostgres(cfg =>
    {
        cfg.SetConnection(new RelationalDatabaseConfiguration("Host=localhost;Database=brighter;"));
        cfg.UseOutbox();
    }));
```
