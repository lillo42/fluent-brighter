using Fluent.Brighter;

using KafkaSample.Commands;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;
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
            .AddFluentBrighter(builder =>
            {
                builder
                    .UseDbTransactionOutboxArchive()
                    .UseOutboxSweeper()
                    .UsingKafka(cfg =>
                    {
                        cfg.SetConnection(c => c
                            .SetBootstrapServers("localhost:9092")
                            .SetName("sample")
                            .SetSaslUsername("admin")
                            .SetSaslPassword("admin-secret")
                            .SetSecurityProtocol(SecurityProtocol.Plaintext)
                            .SetSaslMechanisms(SaslMechanism.Plain));

                        cfg
                            .UsePublications(pp => pp
                                .AddPublication<GreetingEvent>(p => p.SetTopic("greeting"))
                                .AddPublication<FarewellEvent>(p => p.SetTopic("farewell")))
                            .UseSubscriptions(sb => sb
                                .AddSubscription<GreetingEvent>(s => s
                                    .SetTopic("greeting")
                                    .SetConsumerGroupId("greeting.consumer")
                                    .SetMessagePumpType(MessagePumpType.Reactor))
                                .AddSubscription<FarewellEvent>(s => s
                                    .SetTopic("farewell.queue")
                                    .SetConsumerGroupId("farewell.consumer")
                                    .SetMessagePumpType(MessagePumpType.Reactor)));
                    });
            });
    })
    .Build();

await host.StartAsync();

while (true)
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