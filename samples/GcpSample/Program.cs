using Fluent.Brighter;

using GcpSample.Commands;

using Google.Apis.Auth.OAuth2;

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
                    // .UseFirestoreDistributedLock("locking")
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
