using Fluent.Brighter;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Paramore.Brighter;
using Paramore.Brighter.ServiceActivator.Extensions.Hosting;

using PostgreFullSample.Commands;

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
            .AddFluentBrighter(builder =>
            {
                builder
                    .UseDbTransactionOutboxArchive()
                    .UseOutboxSweeper()
                    .UsingPostgres(cfg =>
                {
                    cfg.SetConnection(new RelationalDatabaseConfiguration("Host=localhost;Username=postgres;Password=password;Database=brightertests;"));

                    cfg
                        // .UseDistributedLock()
                        // .UseInbox()
                        // .UseOutbox()
                        .UsePublications(pp => pp
                            .AddPublication<GreetingEvent>(p => p.SetQueue("greeting.queue"))
                            .AddPublication<FarewellEvent>(p => p.SetQueue("farewell.queue"))
                        )
                        .UseSubscriptions(sb => sb
                            .AddSubscription<GreetingEvent>(s => s
                                .SetQueue("greeting.queue")
                                .SetMessagePumpType(MessagePumpType.Reactor))
                            .AddSubscription<FarewellEvent>(s => s
                                .SetQueue("farewell.queue")
                                .SetMessagePumpType(MessagePumpType.Reactor))
                        );
                });
            });
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