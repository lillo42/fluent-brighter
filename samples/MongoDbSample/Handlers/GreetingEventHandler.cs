using MongoDbSample.Commands;

using Paramore.Brighter;

namespace MongoDbSample.Handlers;

public class GreetingEventHandler : RequestHandler<GreetingEvent>
{
    public override GreetingEvent Handle(GreetingEvent @event)
    {
        Console.WriteLine(">>>>>>" + @event.Greeting);
        return base.Handle(@event);
    }
}