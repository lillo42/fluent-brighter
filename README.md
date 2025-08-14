# fluent-brighter
A Fluent API to configure Brighter


# sample

```c#
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
                    .SetSubscription("paramore.example.greeting")
                    .SetQueue("greeting.event.queue")
                    .SetTopic("greeting.event.topic")
                    .SetTimeout(TimeSpan.FromSeconds(200))
                    .EnableDurable()
                    .EnableHighAvailability())
                .AddSubscription<FarewellEvent>(s => s
                    .SetSubscription("paramore.example.farewell")
                    .SetQueue("farewell.event.queue")
                    .SetTopic("farewell.event.topic")))
        ));
```