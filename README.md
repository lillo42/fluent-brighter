# Fluent Brighter

[![License](https://img.shields.io/badge/license-GPL-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0-512BD4)](https://dotnet.microsoft.com/)

A fluent, type-safe configuration API for [Paramore.Brighter](https://github.com/BrighterCommand/Brighter) - the command processor and message-oriented middleware framework for .NET.

## 📋 Overview

Fluent Brighter provides an intuitive, strongly-typed builder pattern for configuring Paramore.Brighter's messaging infrastructure, eliminating boilerplate code and making complex configurations easier to read and maintain.

### ✨ Key Features

- **🎯 Type-Safe Configuration** - IntelliSense-driven API with compile-time safety
- **🔌 Multiple Message Brokers** - RabbitMQ, Kafka, AWS SNS/SQS, GCP Pub/Sub, Redis, RocketMQ
- **💾 Transactional Outbox Pattern** - DynamoDB, Firestore, Spanner, MongoDB, SQL Server, PostgreSQL, MySQL, SQLite
- **📥 Idempotent Inbox Pattern** - Prevent duplicate message processing
- **🔒 Distributed Locking** - Coordinate across multiple instances
- **📦 Large Message Storage** - S3, GCS for oversized payloads
- **⏰ Message Scheduling** - AWS EventBridge, Hangfire, Quartz integration
- **🎨 Fluent Syntax** - Readable, maintainable configuration code

## 📦 Installation

Install the base package and your chosen messaging provider(s):

```bash
# Core package (required)
dotnet add package Fluent.Brighter

# Choose your provider(s)
dotnet add package Fluent.Brighter.RMQ.Async        # RabbitMQ
dotnet add package Fluent.Brighter.Kafka            # Apache Kafka
dotnet add package Fluent.Brighter.AWS.V4           # AWS SNS/SQS
dotnet add package Fluent.Brighter.GoogleCloud      # GCP Pub/Sub
dotnet add package Fluent.Brighter.Redis            # Redis Streams
dotnet add package Fluent.Brighter.RocketMQ         # Apache RocketMQ
dotnet add package Fluent.Brighter.Postgres         # PostgreSQL
dotnet add package Fluent.Brighter.SqlServer        # SQL Server
dotnet add package Fluent.Brighter.MySql            # MySQL
dotnet add package Fluent.Brighter.Sqlite           # SQLite
dotnet add package Fluent.Brighter.MongoDb          # MongoDB
```

## 🚀 Quick Start

### RabbitMQ Example

```csharp
using Fluent.Brighter;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter.ServiceActivator.Extensions.Hosting;

services
    .AddHostedService<ServiceActivatorHostedService>()
    .AddFluentBrighter(brighter => brighter
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
                    .SetSubscriptionName("paramore.example.greeting")
                    .SetQueue("greeting.event.queue")
                    .SetTopic("greeting.event.topic")
                    .SetTimeout(TimeSpan.FromSeconds(200))
                    .EnableDurable()
                    .EnableHighAvailability())
                .AddSubscription<FarewellEvent>(s => s
                    .SetSubscriptionName("paramore.example.farewell")
                    .SetQueue("farewell.event.queue")
                    .SetTopic("farewell.event.topic")))
        ));
```

## 📚 More Examples

### AWS SNS/SQS with DynamoDB Outbox

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingAws(aws => aws
        .SetConnection(conn => conn
            .SetCredentials(new BasicAWSCredentials("key", "secret"))
            .SetRegion(RegionEndpoint.USEast1))
        .UseSnsPublication(pb => pb
            .AddPublication<OrderCreatedEvent>(p => p
                .SetTopic("orders-created")
                .CreateTopicIfMissing()))
        .UseSqsSubscription(sb => sb
            .AddSubscription<OrderCreatedEvent>(s => s
                .SetSubscriptionName("order-processor")
                .SetQueue("order-processing-queue")
                .SetTopic("orders-created")
                .SetMessagePumpType(MessagePumpType.Proactor)
                .SetMakeChannels(OnMissingChannel.Create)))
        .UseDynamoDbOutbox("outbox-table")
        .UseDynamoDbInbox("inbox-table")
        .UseDynamoDbOutboxArchive()));
```

### Google Cloud Platform (GCP) with Firestore

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingGcp(gcp => gcp
        .SetProjectId("my-gcp-project")
        .UsePubSubPublication(pb => pb
            .AddPublication<UserRegisteredEvent>(p => p
                .SetTopicAttributes(t => t.SetName("user-registered-topic"))
                .SetSource("https://example.com/users"))
            .AddPublication<UserDeletedEvent>(p => p
                .SetTopicAttributes(t => t.SetName("user-deleted-topic"))
                .SetSource("https://example.com/users")))
        .UsePubSubSubscription(sb => sb
            .AddSubscription<UserRegisteredEvent>(s => s
                .SetSubscriptionName("user-registration-handler")
                .SetTopicAttributes(t => t.SetName("user-registered-topic"))
                .SetNoOfPerformers(5))
            .AddSubscription<UserDeletedEvent>(s => s
                .SetSubscriptionName("user-deletion-handler")
                .SetTopicAttributes(t => t.SetName("user-deleted-topic"))
                .SetNoOfPerformers(3)))
        .UseFirestoreOutbox("outbox")
        .UseFirestoreInbox("inbox")
        .UseFirestoreOutboxArchive(cfg => cfg
            .SetMinimumAge(TimeSpan.FromDays(7))
            .SetBatchSize(100))));
```

### Apache Kafka

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingKafka(kafka => kafka
        .SetBootstrapServers("localhost:9092")
        .UsePublications(pb => pb
            .AddPublication<PaymentProcessedEvent>(p => p
                .SetTopic("payments-processed")
                .SetMessageIdHeaderKey("message-id")))
        .UseSubscriptions(sb => sb
            .AddSubscription<PaymentProcessedEvent>(s => s
                .SetSubscriptionName("payment-notification-service")
                .SetTopic("payments-processed")
                .SetGroupId("payment-consumers")
                .SetNoOfPerformers(10)))));
```

### PostgreSQL with Outbox Pattern

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingRabbitMq(rabbitmq => /* ... RabbitMQ config ... */)
    .Producers(producer => producer
        .UsePostgresOutbox(cfg => cfg
            .SetConnectionString("Host=localhost;Database=brighter;")
            .SetTableName("outbox")))
    .Subscriptions(sub => sub
        .UsePostgresInbox(cfg => cfg
            .SetConnectionString("Host=localhost;Database=brighter;")
            .SetTableName("inbox")))
    .UseOutboxSweeper(cfg => cfg
        .SetTimerInterval(TimeSpan.FromSeconds(30))
        .SetBatchSize(100)));
```

### SQL Server with Distributed Locking

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingRabbitMq(rabbitmq => /* ... RabbitMQ config ... */)
    .Producers(producer => producer
        .UseSqlServerOutbox(cfg => cfg
            .SetConnectionString("Server=localhost;Database=Brighter;")
            .SetTableName("Outbox"))
        .UseSqlServerDistributedLock(cfg => cfg
            .SetConnectionString("Server=localhost;Database=Brighter;")
            .SetTableName("DistributedLock"))));
```

## 🎯 Key Concepts

### Outbox Pattern
Ensures reliable message publishing by storing messages in a database transaction with your business data, then publishing them asynchronously.

### Inbox Pattern
Prevents duplicate message processing by tracking received messages in a database, ensuring idempotency.

### Distributed Locking
Coordinates message processing across multiple service instances to prevent concurrent execution.

### Outbox Sweeper
Background service that publishes messages from the outbox and archives old messages.

### Large Message Storage
Stores large message payloads (luggage) in cloud storage (S3, GCS) and passes references through the message broker.

## 🔧 Configuration Options

### Message Pump Types
- **Reactor** - Synchronous, single-threaded message processing
- **Proactor** - Asynchronous, concurrent message processing

### Channel Creation
- **Create** - Automatically create topics/queues if missing
- **Assume** - Assume topics/queues already exist
- **Validate** - Validate existence and throw if missing

### Subscription Options
- Number of performers (concurrent handlers)
- Timeout settings
- Retry policies
- Dead letter queues
- Message ordering
- Acknowledgment modes

## 📖 Documentation

For detailed documentation on Paramore.Brighter, visit:
- [Paramore.Brighter Documentation](https://brightercommand.github.io/Brighter/)
- [Paramore.Brighter GitHub](https://github.com/BrighterCommand/Brighter)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the GPL 3 License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

Built on top of the excellent [Paramore.Brighter](https://github.com/BrighterCommand/Brighter) framework by Ian Cooper and contributors.

