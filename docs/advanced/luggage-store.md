# Large Message Storage (Luggage Store)

When message payloads exceed the size limits of your message broker, Fluent Brighter can automatically store the payload in a cloud storage service and pass a reference through the broker instead.

## Supported Providers

| Provider | Method | Package |
|----------|--------|---------|
| AWS S3 | `UseS3LuggageStore(...)` | `Fluent.Brighter.AWS.V4` |
| Google Cloud Storage | `UseGcsLuggageStore(...)` | `Fluent.Brighter.GoogleCloud` |
| MongoDB GridFS | `UseLuggageStore(...)` | `Fluent.Brighter.MongoDb` |

## Configuration

### AWS S3

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingAws(aws => aws
        .SetConnection(/* ... */)
        // ... publications and subscriptions ...
        .UseS3LuggageStore("my-luggage-bucket")));
```

Or with advanced configuration:

```csharp
.UseS3LuggageStore(cfg => { /* S3LuggageStoreBuilder config */ })
```

### Google Cloud Storage

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingGcp(gcp => gcp
        .SetConnection(/* ... */)
        // ... publications and subscriptions ...
        .UseGcsLuggageStore("my-luggage-bucket")));
```

Or with advanced configuration:

```csharp
.UseGcsLuggageStore(cfg => { /* GcsLuggageStoreBuilder config */ })
```

### MongoDB GridFS

```csharp
services.AddFluentBrighter(brighter => brighter
    .UsingMongoDb(mongodb => mongodb
        .SetConnection(c => c
            .SetConnectionString("mongodb://localhost:27017")
            .SetDatabaseName("brighter"))
        .UseLuggageStore("my-bucket")));
```

Or with advanced configuration:

```csharp
.UseLuggageStore(cfg => { /* MongoDbLuggageStoreBuilder config */ })
```

### Generic Luggage Store

You can also configure a luggage store using a custom `IAmAStorageProvider` implementation:

```csharp
services.AddFluentBrighter(brighter => brighter
    .SetLuggageStore(store => store
        .UseLuggageStore<MyCustomStorageProvider>()));
```

Or provide a factory:

```csharp
.SetLuggageStore(store => store
    .UseLuggageStore(sp => new MyCustomStorageProvider(sp.GetRequiredService<IConfiguration>())))
```

## How It Works

1. When publishing a message, if the payload exceeds the broker's size limit, the payload is uploaded to the configured storage provider
2. A reference (claim check) is placed in the message instead of the full payload
3. When consuming the message, the reference is used to retrieve the full payload from storage
4. The consumer receives the fully hydrated message transparently
