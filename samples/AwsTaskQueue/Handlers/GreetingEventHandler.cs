using AwsTaskQueue.Commands;

using Paramore.Brighter;

namespace AwsTaskQueue.Handlers;

public class GreetingEventHandler : RequestHandlerAsync<GreetingEvent>
{
    public override Task<GreetingEvent> HandleAsync(GreetingEvent command, CancellationToken cancellationToken = new CancellationToken())
    {
        Console.WriteLine($"========Hello {command.Greeting}========");
        return base.HandleAsync(command, cancellationToken);
    }
}