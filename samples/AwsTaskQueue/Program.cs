using Amazon;
using Amazon.Runtime;

using AwsTaskQueue.Commands;

using Fluent.Brighter;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Paramore.Brighter;
using Paramore.Brighter.ServiceActivator.Extensions.Hosting;

using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var host = new HostBuilder()
    .UseSerilog()
    .ConfigureServices((_, services) =>
    {
        services
            .AddHostedService<ServiceActivatorHostedService>()
            .AddFluentBrighter(brighter => brighter
                // .UseOutboxSweeper()
                .UsingAws(rabbitmq => rabbitmq
                    .SetConnection(conn => conn
                        .SetCredentials(new BasicAWSCredentials("test", "test"))
                        .SetRegion(RegionEndpoint.USEast1)
                        .SetClientConfigAction(cfg => cfg.ServiceURL = "http://localhost:4566"))
                    .UseSnsPublication(pb => pb
                        .AddPublication<GreetingEvent>(p => p
                            .SetTopic("greeting-event-topic")
                            .CreateTopicIfMissing()))
                    .UseSqsPublication(pb => pb
                        .AddPublication<FarewellEvent>(p => p
                            .SetQueue("farewell-event-queue")
                            .CreateIfMissing()))
                    .UseSqsSubscription(sb => sb
                        .AddSubscription<GreetingEvent>(s => s
                            .SetSubscriptionName("paramore.example.greeting")
                            .SetQueue("greeting-event-queue")
                            .SetTopic("greeting-event-topic")
                            .SetMessagePumpType(MessagePumpType.Proactor)
                            .SetMakeChannels(OnMissingChannel.Create))
                        .AddSubscription<FarewellEvent>(s => s
                            .SetSubscriptionName("paramore.example.farewell")
                            .SetQueue("farewell-event-queue")
                            .SetMessagePumpType(MessagePumpType.Proactor)
                            .SetMakeChannels(OnMissingChannel.Create))
                    )
                    // .UseDynamoDbOutbox()
                    // .UseDynamoDbInbox()
                ));
    })
    .Build();


await host.StartAsync();

using var cts = new  CancellationTokenSource();

Console.CancelKeyPress += (_, _) => cts.Cancel();

while (!cts.IsCancellationRequested)
{
    await Task.Delay(TimeSpan.FromSeconds(1));
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