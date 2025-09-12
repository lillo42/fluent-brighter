using Fluent.Brighter;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Paramore.Brighter;
using Paramore.Brighter.ServiceActivator.Extensions.Hosting;

using RmqTaskQueue.Commands;

var host = new HostBuilder()
    .ConfigureServices((_, services) =>
    {
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
                    )
                ));
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
    await process.PostAsync(new GreetingEvent(name));
    await process.PostAsync(new FarewellEvent(name));
}


await host.StopAsync();