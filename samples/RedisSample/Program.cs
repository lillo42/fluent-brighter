// See https://aka.ms/new-console-template for more information

using Fluent.Brighter;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Paramore.Brighter;
using Paramore.Brighter.ServiceActivator.Extensions.Hosting;

using RedisSample.Commands;

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
                    .UsingRedis(cfg =>
                    {
                        cfg.SetConnection(c => c
                            .SetRedisConnectionString("redis://localhost:6379?ConnectTimeout=1000&SendTimeout=1000")
                            .SetMaxPoolSize(10)
                            .SetMessageTimeToLive(TimeSpan.FromMinutes(10))
                            .SetDefaultRetryTimeout(3_000));

                        cfg
                            .UsePublications(pp => pp
                                .AddPublication<GreetingEvent>(p => p.SetTopic("greeting.topic"))
                                .AddPublication<FarewellEvent>(p => p.SetTopic("farewell.topic")))
                            .UseSubscriptions(sb => sb
                                .AddSubscription<GreetingEvent>(s => s
                                    .SetTopic("greeting.topic")
                                    .SetQueue("greeting.queue")
                                    .SetMessagePumpType(MessagePumpType.Reactor))
                                .AddSubscription<FarewellEvent>(s => s
                                    .SetTopic("farewell.topic")
                                    .SetQueue("farewell.queue")
                                    .SetMessagePumpType(MessagePumpType.Reactor)));
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
