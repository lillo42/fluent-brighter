using GcpSample.Commands;

using Paramore.Brighter;

namespace GcpSample.Handlers;

public class FarewellEventHandler : RequestHandlerAsync<FarewellEvent>
{
    public override Task<FarewellEvent> HandleAsync(FarewellEvent command, CancellationToken cancellationToken = new CancellationToken())
    {
        Console.WriteLine($"---------Bye bye {command.Farewell}---------");
        return base.HandleAsync(command, cancellationToken);
    }
}