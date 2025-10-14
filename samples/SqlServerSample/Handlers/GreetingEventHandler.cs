using Paramore.Brighter;

using SqlServerSample.Commands;

namespace SqlServerSample.Handlers;

public class GreetingEventHandler : RequestHandler<GreetingEvent>
{
    public override GreetingEvent Handle(GreetingEvent @event)
    {
        Console.WriteLine("====== {0}", @event.Greeting);
        return base.Handle(@event);
    }
}