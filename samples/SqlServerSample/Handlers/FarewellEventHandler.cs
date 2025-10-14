using Paramore.Brighter;

using SqlServerSample.Commands;

namespace SqlServerSample.Handlers;

public class FarewellEventHandler : RequestHandler<FarewellEvent>
{
    public override FarewellEvent Handle(FarewellEvent @event)
    {
        Console.WriteLine(">>>>>> {0}", @event.Farewell);
        return base.Handle(@event);
    }
}