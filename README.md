# fluent-brighter
A Fluent API to configure Brighter


# sample


```c#
services
    .AddHostedService<ServiceActivatorHostedService>()
    .AddBrighter(brighter => brighter
        .AddAllFromAssembly()
        .UsingRabbitMQ(rabbitmq => rabbitmq
            .Connection(conn => conn
                .AmqpUriSpecification(amqp => amqp.Uri("amqp://guest:guest@localhost:5672"))
                .Exchange(exchange => exchange.Name("paramore.brighter.exchange")))
            .Publication(pub => pub
                .CreateExchangeIfMissing()
                .MaxOutStandingMessages(5)
                .MaxOutStandingCheckIntervalMilliSeconds(500)
                .WaitForConfirmsTimeOutInMilliseconds(1_000)
                .Topic("greeting.event"))
            .Publication(pub => pub
                .CreateExchangeIfMissing()
                .MaxOutStandingMessages(5)
                .MaxOutStandingCheckIntervalMilliSeconds(500)
                .WaitForConfirmsTimeOutInMilliseconds(1_000)
                .Topic("farewell.event"))
            .Subscription<GreetingEvent>(sub => sub
                .SubscriptionName("paramore.example.greeting")
                .ChannelName("greeting.event")
                .RoutingKey("greeting.event")
                .TimeoutInMilliseconds(200)
                .EnableDurable()
                .EnableHighAvailability()
                .CreateOrOverrideTopicOrQueueIfMissing())
            .Subscription<FarewellEvent>(sub => sub
                .SubscriptionName("paramore.example.farewell")
                .ChannelName("farewell.event")
                .RoutingKey("farewell.event")
                .TimeoutInMilliseconds(200)
                .EnableDurable()
                .EnableHighAvailability()
                .CreateOrOverrideTopicOrQueueIfMissing())
        ));
```