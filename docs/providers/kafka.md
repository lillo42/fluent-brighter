# Apache Kafka

Fluent Brighter provides support for Apache Kafka as a messaging transport.

## Installation

```bash
dotnet add package Fluent.Brighter.Kafka
```

## Basic Setup

```csharp
using Fluent.Brighter;

services.AddFluentBrighter(brighter => brighter
    .UsingKafka(kafka => kafka
        .SetConnection(c => c
            .SetBootstrapServers("localhost:9092"))
        .UsePublications(pb => pb
            .AddPublication<GreetingEvent>(p => p
                .SetTopic("greeting")))
        .UseSubscriptions(sb => sb
            .AddSubscription<GreetingEvent>(s => s
                .SetTopic("greeting")
                .SetConsumerGroupId("greeting.consumer")
                .SetMessagePumpType(MessagePumpType.Reactor)))));
```

## Connection Configuration

```csharp
.SetConnection(c => c
    .SetBootstrapServers("broker1:9092", "broker2:9092")
    .SetName("my-kafka-connection")
    .SetSecurityProtocol(SecurityProtocol.SaslSsl)
    .SetSaslMechanisms(SaslMechanism.Plain)
    .SetSaslUsername("admin")
    .SetSaslPassword("admin-secret")
    .SetSslCaLocation("/path/to/ca.pem")
    .SetSslKeystoreLocation("/path/to/keystore.p12")
    .SetSslKeystorePassword("keystore-password"))
```

### Connection Options

| Method | Description |
|--------|-------------|
| `SetBootstrapServers(string)` | Comma-separated or single bootstrap server |
| `SetBootstrapServers(params string[])` | Multiple bootstrap servers |
| `SetName(string)` | Connection name |
| `SetSecurityProtocol(SecurityProtocol)` | Security protocol (`Plaintext`, `Ssl`, `SaslPlaintext`, `SaslSsl`) |
| `SetSaslMechanisms(SaslMechanism)` | SASL mechanism (`Plain`, `ScramSha256`, `ScramSha512`, etc.) |
| `SetSaslUsername(string)` | SASL username |
| `SetSaslPassword(string)` | SASL password |
| `SetSaslKerberosPrincipal(string)` | Kerberos principal |
| `SetSslCaLocation(string)` | CA certificate path |
| `SetSslKeystoreLocation(string)` | Keystore path |
| `SetSslKeystorePassword(string)` | Keystore password |
| `SetDebug(string)` | librdkafka debug flags |

## Publications

```csharp
.UsePublications(pb => pb
    .AddPublication<GreetingEvent>(p => p
        .SetTopic("greeting")
        .CreateTopicIfMissing()
        .SetNumPartitions(3)
        .SetReplicationFactor(2)
        .SetReplication(Acks.All)
        .EnableIdempotence()))
```

### Publication Options

| Method | Description |
|--------|-------------|
| `SetTopic(RoutingKey)` | Kafka topic name |
| `CreateTopicIfMissing()` | Auto-create topic if missing |
| `ValidIfTopicExists()` | Validate topic exists |
| `AssumeTopicExists()` | Assume topic exists |
| `SetNumPartitions(int)` | Number of partitions (for auto-creation) |
| `SetReplicationFactor(short)` | Replication factor (for auto-creation) |
| `SetReplication(Acks)` | Acknowledgment level (`None`, `Leader`, `All`) |
| `EnableIdempotence()` / `DisableIdempotence()` | Toggle idempotent producer |
| `SetBatchNumberMessages(int)` | Batch size |
| `SetLingerMs(int)` | Linger time in ms |
| `SetMessageSendMaxRetries(int)` | Max send retries |
| `SetMessageTimeoutMs(int)` | Message timeout in ms |
| `SetMaxInFlightRequestsPerConnection(int)` | Max in-flight requests |
| `SetPartitioner(Partitioner)` | Partitioning strategy |
| `SetQueueBufferingMaxMessages(int)` | Max buffered messages |
| `SetQueueBufferingMaxKbytes(int)` | Max buffer size in KB |
| `SetRetryBackoff(int)` | Retry backoff in ms |
| `SetRequestTimeoutMs(int)` | Request timeout in ms |
| `SetTopicFindTimeoutMs(int)` | Topic discovery timeout in ms |
| `SetTransactionalId(string)` | Transactional ID |
| `SetMessageHeaderBuilder(IKafkaMessageHeaderBuilder)` | Custom header builder |
| `SetSource(Uri)` | CloudEvents source |
| `SetDataSchema(Uri)` | CloudEvents data schema |

## Subscriptions

```csharp
.UseSubscriptions(sb => sb
    .AddSubscription<GreetingEvent>(s => s
        .SetTopic("greeting")
        .SetConsumerGroupId("greeting.consumer")
        .SetMessagePumpType(MessagePumpType.Reactor)
        .SetNumberOfPerformers(5)
        .SetOffsetDefault(AutoOffsetReset.Earliest)
        .SetCommitBatchSize(10)))
```

### Subscription Options

| Method | Description |
|--------|-------------|
| `SetSubscription(SubscriptionName)` | Unique subscription identifier |
| `SetTopic(RoutingKey)` | Kafka topic to consume |
| `SetConsumerGroupId(string)` | Consumer group ID |
| `SetMessagePumpType(MessagePumpType)` | `Reactor` or `Proactor` |
| `UseReactorMode()` / `UseProactorMode()` | Convenience methods for pump type |
| `SetNumberOfPerformers(int)` | Number of concurrent consumers |
| `SetOffsetDefault(AutoOffsetReset)` | Default offset (`Earliest`, `Latest`) |
| `SetCommitBatchSize(long)` | Commit after N messages |
| `SetSessionTimeout(TimeSpan)` | Consumer session timeout |
| `SetMaxPollInterval(TimeSpan)` | Max poll interval |
| `SetIsolationLevel(IsolationLevel)` | Read isolation level |
| `SetPartitionAssignmentStrategy(PartitionAssignmentStrategy)` | Partition assignment strategy |
| `SetNumPartitions(int)` | Number of partitions |
| `SetReplicationFactor(short)` | Replication factor |
| `SetConfigHook(Action<ConsumerConfig>)` | Direct access to consumer config |
| `SetTimeout(TimeSpan)` | Processing timeout |
| `SetBufferSize(int)` | Internal buffer size |
| `SetRequeueCount(int)` | Max requeue attempts |
| `SetRequeueDelay(TimeSpan)` | Requeue delay |
| `SetMakeChannels(OnMissingChannel)` | Channel creation mode |
| `CreateInfrastructureIfMissing()` | Auto-create infrastructure |
| `ValidIfInfrastructureExists()` | Validate infrastructure exists |
| `AssumeInfrastructureExists()` | Assume infrastructure exists |
| `SetSweepUncommittedOffsetsInterval(TimeSpan)` | Uncommitted offset sweep interval |
| `SetTopicFindTimeout(TimeSpan)` | Topic discovery timeout |
| `SetReadCommittedOffsetsTimeOut(TimeSpan)` | Committed offsets read timeout |

## Full Example

```csharp
services.AddFluentBrighter(builder =>
{
    builder
        .UseDbTransactionOutboxArchive()
        .UseOutboxSweeper()
        .UsingKafka(cfg =>
        {
            cfg.SetConnection(c => c
                .SetBootstrapServers("localhost:9092")
                .SetName("sample")
                .SetSaslUsername("admin")
                .SetSaslPassword("admin-secret")
                .SetSecurityProtocol(SecurityProtocol.Plaintext)
                .SetSaslMechanisms(SaslMechanism.Plain));

            cfg
                .UsePublications(pp => pp
                    .AddPublication<GreetingEvent>(p => p.SetTopic("greeting"))
                    .AddPublication<FarewellEvent>(p => p.SetTopic("farewell")))
                .UseSubscriptions(sb => sb
                    .AddSubscription<GreetingEvent>(s => s
                        .SetTopic("greeting")
                        .SetConsumerGroupId("greeting.consumer")
                        .SetMessagePumpType(MessagePumpType.Reactor))
                    .AddSubscription<FarewellEvent>(s => s
                        .SetTopic("farewell")
                        .SetConsumerGroupId("farewell.consumer")
                        .SetMessagePumpType(MessagePumpType.Reactor)));
        });
});
```
