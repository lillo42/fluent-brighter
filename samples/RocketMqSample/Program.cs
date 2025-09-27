// See https://aka.ms/new-console-template for more information


using Fluent.Brighter;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Paramore.Brighter;
using Paramore.Brighter.ServiceActivator.Extensions.Hosting;

using RocketMqSample;

var host = new HostBuilder()
    .ConfigureServices(service =>
    {
        service
            .AddHostedService<ServiceActivatorHostedService>()
            .AddFluentBrighter(brighter => brighter
                .UsingRocketMq(rocket => rocket
                    .SetConnection(conn => conn
                        .SetClient(c => c
                            .SetEndpoints("localhost:8081")
                            .EnableSsl(false)
                            .SetRequestTimeout(TimeSpan.FromSeconds(10))
                            .Build()))
                    .UsePublications(pub => pub
                        .AddPublication<GreetingEvent>(p => p
                            .SetTopic("greeting")))
                    .UseSubscriptions(sub => sub
                        .AddSubscription<GreetingEvent>(s => s
                            .SetSubscriptionName("greeting-sub-name")
                            .SetTopic("greeting")
                            .SetConsumerGroup("greeting-consumer-group")
                            .UseReactorMode()
                        ))));
    })
    .Build();
    
await host.StartAsync();

while (true)
{
    await Task.Delay(TimeSpan.FromSeconds(2));
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
}


await host.StopAsync();