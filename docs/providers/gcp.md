# Google Cloud Pub/Sub

Fluent Brighter provides full support for Google Cloud Pub/Sub, along with Firestore and Spanner for outbox/inbox, GCS for luggage storage, and Firestore for distributed locking.

## Installation

```bash
dotnet add package Fluent.Brighter.GoogleCloud
```

## Basic Setup

```csharp
using Fluent.Brighter;

services.AddFluentBrighter(brighter => brighter
    .UsingGcp(gcp => gcp
        .SetConnection(c => c
            .SetCredential(GoogleCredential.GetApplicationDefault())
            .SetProjectId("my-gcp-project"))
        .UsePubSubPublication(pb => pb
            .AddPublication<GreetingEvent>(p => p
                .SetTopic("greeting-event-topic")
                .SetSource("https://example.com/greeting")))
        .UsePubSubSubscription(sb => sb
            .AddSubscription<GreetingEvent>(s => s
                .SetSubscriptionName("greeting-handler")
                .SetSubscription("greeting-event-queue")
                .SetTopic("greeting-event-topic")
                .SetNoOfPerformers(1)))));
```

## Connection Configuration

```csharp
.SetConnection(c => c
    .SetCredential(GoogleCredential.GetApplicationDefault())
    .SetProjectId("my-gcp-project")
    .SetPublisherConfiguration(pub => { /* PublisherClientBuilder config */ })
    .SetSubscriptionManagerConfiguration(sub => { /* SubscriberServiceApiClientBuilder config */ })
    .SetStreamConfiguration(stream => { /* SubscriberClientBuilder config */ })
    .SetTopicManagerConfiguration(topic => { /* PublisherServiceApiClientBuilder config */ })
    .SetProjectsClientConfiguration(proj => { /* ProjectsClientBuilder config */ }))
```

### Connection Options

| Method | Description |
|--------|-------------|
| `SetCredential(ICredential)` | GCP credentials |
| `SetProjectId(string)` | GCP project ID |
| `SetPublisherConfiguration(Action<PublisherClientBuilder>)` | Publisher client settings |
| `SetSubscriptionManagerConfiguration(Action<SubscriberServiceApiClientBuilder>)` | Subscription manager settings |
| `SetStreamConfiguration(Action<SubscriberClientBuilder>)` | Stream subscriber settings |
| `SetTopicManagerConfiguration(Action<PublisherServiceApiClientBuilder>)` | Topic manager settings |
| `SetProjectsClientConfiguration(Action<ProjectsClientBuilder>)` | Projects client settings |

## Publications

```csharp
.UsePubSubPublication(pb => pb
    .AddPublication<UserRegisteredEvent>(p => p
        .SetTopic("user-registered-topic")
        .SetSource("https://example.com/users")
        .CreateQueueIfMissing())
    .AddPublication<UserDeletedEvent>(p => p
        .SetTopic("user-deleted-topic")
        .SetSource("https://example.com/users")))
```

### Publication Options

| Method | Description |
|--------|-------------|
| `SetTopic(RoutingKey)` | Pub/Sub topic name |
| `SetTopicAttributes(TopicAttributes)` | Set topic attributes directly |
| `SetTopicAttributes(Action<TopicAttributeBuilder>)` | Configure topic attributes via builder |
| `CreateQueueIfMissing()` | Auto-create topic if missing |
| `ValidIfQueueExists()` | Validate topic exists |
| `AssumeQueueExists()` | Assume topic exists |
| `SetSource(Uri)` / `SetSource(string)` | CloudEvents source URI |
| `SetDataSchema(Uri)` / `SetDataSchema(string)` | CloudEvents data schema |
| `SetSubject(string)` | CloudEvents subject |
| `SetType(CloudEventsType)` | CloudEvents type |
| `SetPublisherClientConfiguration(Action<PublisherClientBuilder>)` | Per-publication publisher config |
| `SetDefaultHeaders(IDictionary)` | Default message headers |
| `SetReplyTo(string)` | Reply-to address |
| `SetRequestType(Type)` | Request type |

### Topic Attributes

For advanced topic configuration:

```csharp
.SetTopicAttributes(t => t
    .SetName("my-topic")
    .SetProjectId("my-project")
    .SetLabels(new Dictionary<string, string> { ["env"] = "prod" })
    .SetMessageRetentionDuration(TimeSpan.FromDays(7))
    .SetKmsKeyName("projects/my-project/locations/global/keyRings/my-ring/cryptoKeys/my-key"))
```

## Subscriptions

```csharp
.UsePubSubSubscription(sb => sb
    .AddSubscription<UserRegisteredEvent>(s => s
        .SetSubscriptionName("user-registration-handler")
        .SetSubscription("user-registered-queue")
        .SetTopic("user-registered-topic")
        .SetNoOfPerformers(5)
        .EnableExactlyOnceDelivery()
        .UseStreamMode()))
```

### Subscription Options

| Method | Description |
|--------|-------------|
| `SetSubscriptionName(SubscriptionName)` | Logical subscription name |
| `SetSubscription(ChannelName)` | Pub/Sub subscription name (channel) |
| `SetTopic(RoutingKey)` | Topic to subscribe to |
| `SetNoOfPerformers(int)` | Number of concurrent handlers |
| `SetMessagePumpType(MessagePumpType)` | `Reactor` or `Proactor` |
| `UseReactor()` / `UseProactor()` | Convenience methods for pump type |
| `UseStreamMode()` / `UsePullMode()` | Subscription mode |
| `SetAckDeadlineSeconds(uint)` | Acknowledgment deadline |
| `EnableMessageOrdering()` / `DisableMessageOrdering()` | Message ordering |
| `EnableExactlyOnceDelivery()` / `DisableExactlyOnceDelivery()` | Exactly-once delivery |
| `SetTopicAttributes(TopicAttributes)` | Topic attributes |
| `SetTopicAttributes(Action<TopicAttributeBuilder>)` | Topic attributes via builder |
| `SetDeadLetter(Action<DeadLetterPolicyBuilder>)` | Dead letter policy |
| `SetProjectId(string)` | Override project ID |
| `SetMakeChannels(OnMissingChannel)` | Channel creation mode |
| `SetEmptyChannelDelay(TimeSpan)` | Delay when channel is empty |
| `SetChannelFailureDelay(TimeSpan)` | Delay after channel failure |
| `SetBufferSize(int)` | Internal buffer size |
| `SetTimeOut(TimeSpan)` | Processing timeout |
| `SetRequeueCount(int)` | Max requeue attempts |
| `SetRequeueDelay(TimeSpan)` | Requeue delay |

## Firestore Outbox & Inbox

```csharp
.UsingGcp(gcp => gcp
    // ... connection and pub/sub config ...
    .SetFirestoreConfiguration("my-firestore-database")
    .UseFirestoreOutbox("outbox")
    .UseFirestoreInbox("inbox")
    .UseFirestoreOutboxArchive())
```

### Firestore Options

| Method | Description |
|--------|-------------|
| `SetFirestoreConfiguration(string)` | Firestore database name |
| `UseFirestoreOutbox()` | Use Firestore outbox with defaults |
| `UseFirestoreOutbox(string)` | Use Firestore outbox with collection name |
| `UseFirestoreOutbox(Action<FirestoreOutboxBuilder>)` | Advanced outbox config |
| `UseFirestoreInbox()` | Use Firestore inbox with defaults |
| `UseFirestoreInbox(string)` | Use Firestore inbox with collection name |
| `UseFirestoreInbox(Action<FirestoreInboxBuilder>)` | Advanced inbox config |
| `UseFirestoreOutboxArchive()` | Enable outbox archiving |
| `UseFirestoreOutboxArchive(Action<TimedOutboxArchiverOptionsBuilder>)` | Configure archive options |
| `UseFirestoreLocking(Action<FirestoreLockingBuilder>)` | Distributed locking with Firestore |

## Spanner Outbox & Inbox

```csharp
.UsingGcp(gcp => gcp
    // ... connection and pub/sub config ...
    .UseSpannerOutbox(cfg => cfg
        .SetConfiguration(spannerConfig))
    .UseSpannerInbox(cfg => cfg
        .SetConfiguration(spannerConfig)))
```

## GCS Luggage Store

Store large message payloads in Google Cloud Storage:

```csharp
.UsingGcp(gcp => gcp
    // ... other config ...
    .UseGcsLuggageStore("my-luggage-bucket"))
```

## Full Example

```csharp
services.AddFluentBrighter(brighter => brighter
    .UseOutboxSweeper()
    .UsingGcp(gcp => gcp
        .SetConnection(c => c
            .SetCredential(GoogleCredential.GetApplicationDefault())
            .SetProjectId(Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT")!))
        .UsePubSubPublication(pb => pb
            .AddPublication<GreetingEvent>(p => p
                .SetTopic("greeting-event-topic")
                .SetSource("https://example.com/greeting"))
            .AddPublication<FarewellEvent>(p => p
                .SetTopic("farewell-event-topic")
                .SetSource("https://example.com/farewell")))
        .UsePubSubSubscription(sb => sb
            .AddSubscription<GreetingEvent>(s => s
                .SetSubscriptionName("paramore.example.greeting")
                .SetSubscription("greeting-event-queue")
                .SetTopic("greeting-event-topic")
                .SetNoOfPerformers(1))
            .AddSubscription<FarewellEvent>(s => s
                .SetSubscriptionName("paramore.example.farewell")
                .SetSubscription("farewell-event-queue")
                .SetTopic("farewell-event-topic")
                .SetNoOfPerformers(1)))
        .SetFirestoreConfiguration("brighter-firestore-database")
        .UseFirestoreOutbox("outbox")
        .UseFirestoreInbox("inbox")
        .UseFirestoreOutboxArchive()));
```
