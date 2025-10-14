using Fluent.Brighter;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Paramore.Brighter;
using Paramore.Brighter.Inbox.MsSql;
using Paramore.Brighter.Outbox.MsSql;
using Paramore.Brighter.ServiceActivator.Extensions.Hosting;

using Serilog;

using SqlServerSample.Commands;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

const string ConnectionString = "Server=127.0.0.1,11433;Database=BrighterTests;User Id=sa;Password=Password123!;Application Name=BrighterTests;Connect Timeout=60;Encrypt=false";

var builder = new SqlConnectionStringBuilder(ConnectionString);
var databaseName = builder.InitialCatalog;
builder.InitialCatalog = "master";
using (var connection = new SqlConnection(builder.ConnectionString))
{
    await connection.OpenAsync();
    await using var command = connection.CreateCommand();
    command.CommandText =
        $"""
        IF DB_ID('{databaseName}') IS NULL
        BEGIN
            CREATE DATABASE {databaseName};
        END;
        """;
    await command.ExecuteNonQueryAsync();
}

using (var connection = new SqlConnection(ConnectionString))
{
    try
    {

        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = SqlInboxBuilder.GetDDL("InboxTable");
        await command.ExecuteNonQueryAsync();
    }
    catch (Exception)
    {
        // Ignoring
    }
}

using (var connection = new SqlConnection(ConnectionString))
{
    try
    {
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = SqlOutboxBuilder.GetDDL("OutboxTable");
        await command.ExecuteNonQueryAsync();
    }
    catch (Exception)
    {
        // Ignoring
    }
}


using (var connection = new SqlConnection(ConnectionString))
{
    try
    {
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE [dbo].[QueueData](
                [Id] [bigint] IDENTITY(1,1) NOT NULL,
                [Topic] [nvarchar](255) NOT NULL,
                [MessageType] [nvarchar](1024) NOT NULL,
                [Payload] [nvarchar](max) NOT NULL,
                CONSTRAINT [PK_QueueData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
            """;
        await command.ExecuteNonQueryAsync();

        command.CommandText =
            """
            CREATE NONCLUSTERED INDEX [IX_Topic] ON [dbo].[QueueData]
            (
                [Topic] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            """;
        await command.ExecuteNonQueryAsync();
    }
    catch (Exception)
    {
        // Ignoring
    }
}

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
                    .UsingMicrosoftSqlServer(cfg =>
                    {
                        cfg.SetConnection(new RelationalDatabaseConfiguration(ConnectionString, 
                            databaseName: "BrighterTests", 
                            queueStoreTable: "QueueData"));

                        cfg
                            .UseDistributedLock()
                            .UseInbox(i => i.SetInboxTableName("InboxTable"))
                            .UseOutbox(o => o.SetOutboxTableName("OutboxTable"))
                            .UsePublications(pp => pp
                                .AddPublication<GreetingEvent>(p => p.SetQueue("greeting.queue"))
                                .AddPublication<FarewellEvent>(p => p.SetQueue("farewell.queue")))
                            .UseSubscriptions(sb => sb
                                .AddSubscription<GreetingEvent>(s => s
                                    .SetQueue("greeting.queue")
                                    .SetMessagePumpType(MessagePumpType.Reactor))
                                .AddSubscription<FarewellEvent>(s => s
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
