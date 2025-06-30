using Fluent.Brighter;
using Fluent.Brighter.RMQ.Sync;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Paramore.Brighter;
using Paramore.Brighter.ServiceActivator.Extensions.Hosting;

using RmqTaskQueue.Commands;

Console.WriteLine("Hello, World!");

var host = new HostBuilder()
    .ConfigureServices((_, services) =>
    {
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
                        .Topic("greeting.event"))
                    .Publication(pub => pub
                        .CreateExchangeIfMissing()
                        .Topic("farewell.event"))
                    .Subscription<GreetingEvent>(sub => sub
                        .SubscriptionName("paramore.example.greeting")
                        .ChannelName("greeting.event")
                        .RoutingKey("greeting.event")
                        .TimeOut(TimeSpan.FromSeconds(200))
                        .EnableDurable()
                        .EnableHighAvailability()
                        .CreateIfMissing())
                    .Subscription<FarewellEvent>(sub => sub
                        .SubscriptionName("paramore.example.farewell")
                        .ChannelName("farewell.event")
                        .RoutingKey("farewell.event")
                        .TimeOut(TimeSpan.FromSeconds(200))
                        .EnableDurable()
                        .EnableHighAvailability()
                        .CreateIfMissing())
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