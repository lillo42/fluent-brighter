using Paramore.Brighter;

using RedisSample.Commands;

namespace RedisSample.Handlers;

public class GreetingEventHandler : RequestHandler<GreetingEvent>
{
    public override GreetingEvent Handle(GreetingEvent @event)
    {
        Console.WriteLine("====== {0}", @event.Greeting);
        return base.Handle(@event);
    }
}