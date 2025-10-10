using MongoDbSample.Commands;

using Paramore.Brighter;

namespace MongoDbSample.Handlers;

public class FarewellEventHandler : RequestHandler<FarewellEvent>
{
    public override FarewellEvent Handle(FarewellEvent @event)
    {
        Console.WriteLine("============" + @event.Farewell);
        return base.Handle(@event);
    }
}