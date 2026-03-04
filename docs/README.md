# Fluent Brighter

A fluent, type-safe configuration API for [Paramore.Brighter](https://github.com/BrighterCommand/Brighter) — the command processor and message-oriented middleware framework for .NET.

## Why Fluent Brighter?

Paramore.Brighter is a powerful framework, but configuring its messaging infrastructure can involve significant boilerplate. Fluent Brighter provides an intuitive, strongly-typed builder pattern that makes complex configurations easier to read, write, and maintain.

### Key Features

- **Type-Safe Configuration** — IntelliSense-driven API with compile-time safety
- **Multiple Message Brokers** — RabbitMQ, Kafka, AWS SNS/SQS, GCP Pub/Sub, Redis, RocketMQ
- **Database Transports** — PostgreSQL, SQL Server, MySQL, SQLite, MongoDB
- **Transactional Outbox Pattern** — DynamoDB, Firestore, Spanner, MongoDB, SQL Server, PostgreSQL, MySQL, SQLite
- **Idempotent Inbox Pattern** — Prevent duplicate message processing
- **Distributed Locking** — Coordinate across multiple instances
- **Large Message Storage** — S3, GCS, MongoDB GridFS for oversized payloads
- **Fluent Syntax** — Readable, maintainable configuration code

## Quick Example

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingRabbitMq(rabbitmq => rabbitmq
        .SetConnection(conn => conn
            .SetAmpq(amqp => amqp.SetUri("amqp://guest:guest@localhost:5672"))
            .SetExchange(exchange => exchange.SetName("my.exchange")))
        .UsePublications(pb => pb
            .AddPublication<GreetingEvent>(p => p
                .SetTopic("greeting.topic")
                .CreateTopicIfMissing()))
        .UseSubscriptions(sb => sb
            .AddSubscription<GreetingEvent>(s => s
                .SetSubscription("greeting-handler")
                .SetQueue("greeting.queue")
                .SetTopic("greeting.topic")))));
```

## Supported Platforms

- .NET 8.0
- .NET 9.0
