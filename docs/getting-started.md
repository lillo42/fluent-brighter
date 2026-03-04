# Getting Started

## Installation

Install the core package and your chosen provider(s) via the .NET CLI:

```bash
# Core package (required)
dotnet add package Fluent.Brighter
```

Then add the packages for your messaging provider and any database integrations you need:

### Messaging Providers

```bash
dotnet add package Fluent.Brighter.RMQ.Async    # RabbitMQ (async)
dotnet add package Fluent.Brighter.RMQ.Sync     # RabbitMQ (sync)
dotnet add package Fluent.Brighter.Kafka         # Apache Kafka
dotnet add package Fluent.Brighter.AWS.V4        # AWS SNS/SQS
dotnet add package Fluent.Brighter.GoogleCloud   # GCP Pub/Sub
dotnet add package Fluent.Brighter.Redis         # Redis Streams
dotnet add package Fluent.Brighter.RocketMQ      # Apache RocketMQ
```

### Database Providers

These packages can be used as message transports and/or for outbox/inbox storage:

```bash
dotnet add package Fluent.Brighter.Postgres      # PostgreSQL
dotnet add package Fluent.Brighter.SqlServer     # SQL Server
dotnet add package Fluent.Brighter.MySql         # MySQL
dotnet add package Fluent.Brighter.Sqlite        # SQLite
dotnet add package Fluent.Brighter.MongoDb       # MongoDB
```

## Basic Setup

All configuration starts with the `AddFluentBrighter` extension method on `IServiceCollection`:

```csharp
using Fluent.Brighter;

services.AddFluentBrighter(brighter =>
{
    // Configure your messaging provider
    brighter.UsingRabbitMq(rabbitmq => { /* ... */ });

    // Optional: configure producers with outbox
    brighter.Producers(producer => { /* ... */ });

    // Optional: configure consumers with inbox
    brighter.Subscriptions(sub => { /* ... */ });

    // Optional: enable outbox sweeper
    brighter.UseOutboxSweeper();
});
```

## Core Builder Options

The `FluentBrighterBuilder` provides the following configuration methods:

| Method | Description |
|--------|-------------|
| `Subscriptions(...)` | Configure message consumers, inbox, and subscription settings |
| `Producers(...)` | Configure message producers, outbox, and distributed locking |
| `RequestHandlers(...)` | Register request handlers from assemblies or individually |
| `Mappers(...)` | Register message mappers |
| `Transformers(...)` | Register message transformers |
| `UseOutboxSweeper(...)` | Enable the background outbox sweeper service |
| `UseOutboxArchive<T>(...)` / `UseDbTransactionOutboxArchive(...)` | Enable outbox archiving |
| `SetLuggageStore(...)` | Configure large message (luggage) storage |
| `SetScheduler(...)` | Configure message scheduling |
| `RegisterServices(...)` | Register additional services in the DI container |

## Message Pump Types

Brighter supports two message pump types:

- **Reactor** — Synchronous, single-threaded message processing
- **Proactor** — Asynchronous, concurrent message processing

## Channel Creation Modes

When configuring subscriptions and publications, you can control how missing infrastructure is handled:

- **Create** — Automatically create topics/queues if they don't exist
- **Validate** — Check that infrastructure exists and throw if missing
- **Assume** — Assume infrastructure already exists (no checks)

## Next Steps

Choose your messaging provider to get started:

- [RabbitMQ](providers/rabbitmq.md)
- [Apache Kafka](providers/kafka.md)
- [AWS SNS/SQS](providers/aws.md)
- [Google Cloud Pub/Sub](providers/gcp.md)
- [Redis](providers/redis.md)
- [Apache RocketMQ](providers/rocketmq.md)
