using Fluent.Brighter;

using GcpSample.Commands;

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
                .UseOutboxSweeper()
                .UsingGcp(gcp => gcp
                    .SetProjectId("my-gcp-project-id")
                    .UsePubSubPublication(pb => pb
                        .AddPublication<GreetingEvent>(p => p
                            .SetTopicAttributes(t => t.SetName("greeting-event-topic"))
                            .SetSource("https://example.com/greeting")))
                    .UsePubSubSubscription(sb => sb
                        .AddSubscription<GreetingEvent>(s => s
                            .SetSubscriptionName("paramore.example.greeting")
                            .SetTopicAttributes(t => t.SetName("greeting-event-topic"))
                            .SetNoOfPerformers(5))
                        .AddSubscription<FarewellEvent>(s => s
                            .SetSubscriptionName("paramore.example.farewell")
                            .SetTopicAttributes(t => t.SetName("farewell-event-topic"))
                            .SetNoOfPerformers(5)))
                    .UseFirestoreOutbox("outbox")
                    .UseFirestoreInbox("inbox")
                    .UseFirestoreDistributedLock("locking")
                    .UseFirestoreOutboxArchive()
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
    await process.DepositPostAsync(new GreetingEvent(name));
    await process.DepositPostAsync(new FarewellEvent(name));
}


await host.StopAsync();
