using Fluent.Brighter;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MongoDB.Driver;

using MongoDbSample.Commands;

using Paramore.Brighter;
using Paramore.Brighter.ServiceActivator.Extensions.Hosting;

// TODO: this sample isn't working due a bug in Brighter, waiting it to be fixed
var host = new HostBuilder()
    .ConfigureServices((_, services) =>
    {
        services
            .AddHostedService<ServiceActivatorHostedService>()
            .AddFluentBrighter(brighter => brighter
                .UseOutboxSweeper()
                .UseOutboxArchiver<IClientSession>(new NullOutboxArchiveProvider())
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
                            .CreateTopicIfMissing()) 
                    )
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
                            .SetTopic("farewell.event.topic"))
                    ))
                .UsingMongoDb(mongodb => mongodb
                    .SetConnection(c => c
                        .SetConnectionString("mongodb://root:example@localhost:27017")
                        .SetDatabaseName("brighter"))
                    .UseInbox()
                    .UseOutbox()
                    .UseDistributedLock()
                    .UseLuggageStore("bucket"))
            );
    })
    .Build();

await host.StartAsync();

while (true)
{
    await Task.Delay(TimeSpan.FromSeconds(10));
    Console.Write("Say your name (or q to quit): ");
    var name = Console.ReadLine();

    if (string.IsNullOrEmpty(name))
    {
        continue;
    }

    if (name == "q")
    {
        break;
    }

    var process = host.Services.GetRequiredService<IAmACommandProcessor>();
    await process.DepositPostAsync(new GreetingEvent(name));
    await process.DepositPostAsync(new FarewellEvent(name));
}


await host.StopAsync();
